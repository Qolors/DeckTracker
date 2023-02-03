﻿using DeckTracker.Models;
using DeckTracker.Services;
using DeckTracker.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace DeckTracker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase content;
        TodoListViewModel todoListViewModel;
        InitialViewModel loadingView;
        ReadyViewModel readyViewModel;
        Database dataBase;
        CardDataBase cardDataBase;

        private Judge Judge;

        const string WAITING = "Waiting for game to start.";
        const string OBSERVING = "Observing Opponent Cards..";

        private static Timer pingTimer;
        public MainWindowViewModel()
        {
            Content = LoadingView = new InitialViewModel();
            Task.Run(() => LoadDataBases()).ConfigureAwait(false);
            this.SetTimer();
        }
        private Task LoadDataBases()
        {
            Judge = new Judge();
            Judge.GetMetaDecks();
            return Task.CompletedTask;
        }
        private void SetTimer()
        {
            pingTimer = new Timer(2000);
            pingTimer.Elapsed += async (sender, e) => await PingForUpdates();
            pingTimer.AutoReset = true;
            pingTimer.Enabled = true;
            pingTimer.Start();
        }
        private Task PingForUpdates()
        {
            try
            {
                TheDataBase = Task.Run(() => Database.PingClientAsync()).Result;
                if (TheDataBase != null)
                {
                    Judge.CheckForNew(TheDataBase.GetItems());

                    if (TheDataBase.CardSeen)
                    {
                        Content = List = new TodoListViewModel(Judge.CurrentGuess);
                    }
                    else
                    {
                        Content = TheDataBase.BoardState.GameState != "InProgress" ?
                            Connected = new ReadyViewModel(WAITING) : Connected = new ReadyViewModel(OBSERVING);
                        Judge.OpponentDeck.DeckList.Clear();
                    }
                        

                }
            }
            catch (Exception)
            {
               return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }
        public Database TheDataBase
        {
            get => dataBase;
            set => dataBase= value;
        }
        public CardDataBase TheCardDataBase
        {
            get => cardDataBase;
            set => cardDataBase= value;
        }
        public TodoListViewModel List 
        { 
            get => todoListViewModel;
            private set => this.RaiseAndSetIfChanged(ref todoListViewModel, value);
        }

        public InitialViewModel LoadingView
        {
            get => loadingView;
            private set => this.RaiseAndSetIfChanged(ref loadingView, value);
        }

        public ReadyViewModel Connected
        {
            get => readyViewModel;
            private set => this.RaiseAndSetIfChanged(ref readyViewModel, value);
        }

        public void AddItem()
        {
            var vm = new AddItemViewModel();

            Observable.Merge(
                vm.Ok,
                vm.Cancel.Select(_ => (TodoItem)null))
                .Take(1)
                .Subscribe(model =>
                {
                    if (model != null)
                    {
                        List.Items.Add(model);
                    }
                    Content = List;
                });
            Content = vm;
        }
    }
}
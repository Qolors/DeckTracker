using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using DeckTracker.Models;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        private Bitmap observedCard;
        public TodoListViewModel(IEnumerable<TodoItem> items)
        {

            Items = new ObservableCollection<TodoItem>();
            Task.Run(() => LoadImages(items));

        }

        public async void LoadImages(IEnumerable<TodoItem> items)
        {
            HttpClient httpClient = new HttpClient();
            var tmpObserv = new Collection<TodoItem>();
            foreach(TodoItem item in items)
            {
                await item.LoadImage(httpClient);
                tmpObserv.Add(item);
            }
            var arr = tmpObserv.ToArray();
            Items.AddRange(arr);
            TheObservedCard = Items.First().Cover;
        }

        public Bitmap TheObservedCard
        {
            get => observedCard;
            set => this.RaiseAndSetIfChanged(ref observedCard, value);
        }
        public ObservableCollection<TodoItem> Items { get; set; }

    }
}

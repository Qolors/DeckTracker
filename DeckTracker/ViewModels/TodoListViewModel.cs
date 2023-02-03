using Avalonia.Media.Imaging;
using DeckTracker.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        public TodoListViewModel(IEnumerable<TodoItem> items)
        {

            Items = new ObservableCollection<TodoItem>();
            Task.Run(() => LoadImages(items));

        }

        public async void LoadImages(IEnumerable<TodoItem> items)
        {
            foreach(TodoItem item in items)
            {
                await item.LoadImage();
                Items.Add(item);
            }
        }

        public ObservableCollection<TodoItem> Items { get; set; }
    }
}

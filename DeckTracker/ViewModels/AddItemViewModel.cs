using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using DeckTracker.Models;
using ReactiveUI;

namespace DeckTracker.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        private string description;

        public AddItemViewModel()
        {
            //var okEnabled = this.WhenAnyValue(
            //    x => x.Description,
            //    x => !string.IsNullOrWhiteSpace(x));

            //Ok = ReactiveCommand.Create(
            //    () => new TodoItem { Face = Description },
            //    okEnabled);

            Cancel = ReactiveCommand.Create(() => { });
        }
        public string Description 
        {
            get => description;
            set => this.RaiseAndSetIfChanged(ref description, value);
        }

        public ReactiveCommand<Unit, TodoItem> Ok { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        
    }
}

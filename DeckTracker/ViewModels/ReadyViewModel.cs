using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.ViewModels
{
    public class ReadyViewModel : ViewModelBase
    {
        private string text;
        public ReadyViewModel(string text) { Text = text; }

        public string Text
        {
            get => text;
            private set => this.RaiseAndSetIfChanged(ref text, value);
        }
    }
}

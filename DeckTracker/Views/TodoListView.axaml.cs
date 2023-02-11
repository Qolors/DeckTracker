using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using DeckTracker.Models;
using DeckTracker.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeckTracker.Views;

public partial class TodoListView : UserControl
{
    
    public TodoListView()
    {
        InitializeComponent();
    }

    public void OnPointerEnter(object sender, PointerEventArgs e)
    {
        var theTextBlock = sender as TextBlock;
        var viewModel = (TodoListViewModel)this.DataContext;
        viewModel.TheObservedCard = viewModel.Items
            .Where(i => i.Description == theTextBlock.Name)
            .Select(i => i.Cover)
            .First();
    }

}
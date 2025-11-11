using MauiNoteTaker3._0.ViewModels;

namespace MauiNoteTaker3._0.Views;

public partial class NotesListPage : ContentPage
{
    public NotesListPage(NotesListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is NotesListViewModel viewModel)
        {
            viewModel.LoadNotesCommand.Execute(null);
        }
    }
}
using MauiNoteTaker3._0.ViewModels;

namespace MauiNoteTaker3._0.Views;

public partial class NoteDetailPage : ContentPage
{
    public NoteDetailPage(NoteDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
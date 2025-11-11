using MauiNoteTaker3._0.Views;

namespace MauiNoteTaker3._0;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(NoteDetailPage), typeof(NoteDetailPage));
    }
}
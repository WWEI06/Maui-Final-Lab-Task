using Microsoft.Extensions.Logging;
using MauiNoteTaker3._0.Services;
using MauiNoteTaker3._0.ViewModels;
using MauiNoteTaker3._0.Views;

namespace MauiNoteTaker3._0;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register Services
        builder.Services.AddSingleton<INoteService, NoteService>();

        // Register ViewModels
        builder.Services.AddTransient<NotesListViewModel>();
        builder.Services.AddTransient<NoteDetailViewModel>();

        // Register Views
        builder.Services.AddTransient<NotesListPage>();
        builder.Services.AddTransient<NoteDetailPage>();

        return builder.Build();
    }
}
using System.Windows.Input;
using MauiNoteTaker3._0.Models;
using MauiNoteTaker3._0.Services;

namespace MauiNoteTaker3._0.ViewModels;

public class NoteDetailViewModel : BaseViewModel
{
    private readonly INoteService _noteService;
    private Note? _note;
    private string? _noteId;

    public Note? Note
    {
        get => _note;
        set => SetProperty(ref _note, value);
    }

    public string? NoteId
    {
        get => _noteId;
        set
        {
            SetProperty(ref _noteId, value);
            LoadNote(value);
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ChangeColorCommand { get; }

    public NoteDetailViewModel(INoteService noteService)
    {
        _noteService = noteService;
        Note = new Note();

        SaveCommand = new Command(async () => await SaveNoteAsync());
        DeleteCommand = new Command(async () => await DeleteNoteAsync());
        ChangeColorCommand = new Command<string>(async (color) => await ChangeColorAsync(color));
    }

    private async void LoadNote(string? id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            var loadedNote = await _noteService.GetNoteAsync(id);
            Note = loadedNote ?? new Note();
        }
    }

    private async Task SaveNoteAsync()
    {
        if (Note != null)
        {
            Note.UpdatedDate = DateTime.Now;
            await _noteService.SaveNoteAsync(Note);
            if (Shell.Current is not null)
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }

    private async Task DeleteNoteAsync()
    {
        if (Note != null && !string.IsNullOrEmpty(Note.Id) && Shell.Current is not null)
        {
            bool answer = await Shell.Current.DisplayAlert(
                "Delete Note",
                $"Are you sure you want to delete '{Note.Title}'?",
                "Yes", "No");

            if (answer)
            {
                await _noteService.DeleteNoteAsync(Note.Id);
                await Shell.Current.GoToAsync("..");
            }
        }
    }

    private Task ChangeColorAsync(string? color)
    {
        if (Note != null && !string.IsNullOrEmpty(color))
        {
            Note.Color = color;
            OnPropertyChanged(nameof(Note));
        }
        return Task.CompletedTask;
    }
}
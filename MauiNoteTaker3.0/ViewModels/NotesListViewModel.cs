using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiNoteTaker3._0.Models;
using MauiNoteTaker3._0.Services;

namespace MauiNoteTaker3._0.ViewModels;

public class NotesListViewModel : BaseViewModel
{
    private readonly INoteService _noteService;
    private string? _searchText;
    private bool _isRefreshing;

    public ObservableCollection<Note> Notes { get; } = new();
    public ICommand LoadNotesCommand { get; }
    public ICommand AddNoteCommand { get; }
    public ICommand EditNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand TogglePinCommand { get; }

    public string? SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            SearchNotes();
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public NotesListViewModel(INoteService noteService)
    {
        _noteService = noteService;

        LoadNotesCommand = new Command(async () => await LoadNotesAsync());
        AddNoteCommand = new Command(async () => await AddNoteAsync());
        EditNoteCommand = new Command<Note>(async (note) => await EditNoteAsync(note));
        DeleteNoteCommand = new Command<Note>(async (note) => await DeleteNoteAsync(note));
        SearchCommand = new Command(async () => await SearchNotesAsync());
        TogglePinCommand = new Command<Note>(async (note) => await TogglePinAsync(note));

        LoadNotesCommand.Execute(null);
    }

    private async Task LoadNotesAsync()
    {
        IsRefreshing = true;

        try
        {
            var notes = await _noteService.GetNotesAsync();
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private async Task AddNoteAsync()
    {
        var newNote = new Note { Title = "New Note", Content = "Start writing..." };
        await _noteService.SaveNoteAsync(newNote);
        await LoadNotesAsync();

        // Navigate to edit page
        if (Shell.Current is not null)
        {
            await Shell.Current.GoToAsync($"NoteDetailPage?NoteId={newNote.Id}");
        }
    }

    private async Task EditNoteAsync(Note? note)
    {
        if (note != null && Shell.Current is not null)
        {
            await Shell.Current.GoToAsync($"NoteDetailPage?NoteId={note.Id}");
        }
    }

    private async Task DeleteNoteAsync(Note? note)
    {
        if (note == null) return;

        bool answer = await Shell.Current.DisplayAlert(
            "Delete Note",
            $"Are you sure you want to delete '{note.Title}'?",
            "Yes", "No");

        if (answer)
        {
            await _noteService.DeleteNoteAsync(note.Id);
            await LoadNotesAsync();
        }
    }

    private async Task SearchNotesAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await LoadNotesAsync();
            return;
        }

        IsRefreshing = true;
        try
        {
            var notes = await _noteService.SearchNotesAsync(SearchText);
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private void SearchNotes()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            LoadNotesCommand.Execute(null);
        }
        else
        {
            SearchCommand.Execute(null);
        }
    }

    private async Task TogglePinAsync(Note? note)
    {
        if (note is null) return;

        note.IsPinned = !note.IsPinned;
        await _noteService.SaveNoteAsync(note);
        await LoadNotesAsync(); // Reload to maintain order
    }
}
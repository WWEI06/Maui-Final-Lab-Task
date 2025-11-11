using System.Text.Json;
using MauiNoteTaker3._0.Models;

namespace MauiNoteTaker3._0.Services;

public class NoteService : INoteService
{
    private readonly string _filePath;
    private List<Note> _notes;

    public NoteService()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, "notes.json");
        _notes = new List<Note>();
        LoadNotes();
    }

    private void LoadNotes()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
            }
        }
        catch (Exception)
        {
            _notes = new List<Note>();
        }
    }

    private void SaveNotes()
    {
        try
        {
            var json = JsonSerializer.Serialize(_notes);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception)
        {
            // Handle serialization errors
        }
    }

    public Task<List<Note>> GetNotesAsync()
    {
        return Task.FromResult(_notes.OrderByDescending(n => n.UpdatedDate).ToList());
    }

    public Task<Note?> GetNoteAsync(string id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        return Task.FromResult(note);
    }

    public Task SaveNoteAsync(Note note)
    {
        var existingNote = _notes.FirstOrDefault(n => n.Id == note.Id);

        if (existingNote != null)
        {
            // Update existing note
            existingNote.Title = note.Title;
            existingNote.Content = note.Content;
            existingNote.UpdatedDate = DateTime.Now;
            existingNote.Color = note.Color;
            existingNote.IsPinned = note.IsPinned;
        }
        else
        {
            // Add new note
            note.CreatedDate = DateTime.Now;
            note.UpdatedDate = DateTime.Now;
            _notes.Add(note);
        }

        SaveNotes();
        return Task.CompletedTask;
    }

    public Task DeleteNoteAsync(string id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null)
        {
            _notes.Remove(note);
            SaveNotes();
        }
        return Task.CompletedTask;
    }

    public Task<List<Note>> SearchNotesAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return GetNotesAsync();

        var filteredNotes = _notes.Where(n =>
            n.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
            n.Content.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(n => n.UpdatedDate)
            .ToList();

        return Task.FromResult(filteredNotes);
    }
}
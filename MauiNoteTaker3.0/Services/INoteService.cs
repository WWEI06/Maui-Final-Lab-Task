using MauiNoteTaker3._0.Models;

namespace MauiNoteTaker3._0.Services;

public interface INoteService
{
    Task<List<Note>> GetNotesAsync();
    Task<Note?> GetNoteAsync(string id);
    Task SaveNoteAsync(Note note);
    Task DeleteNoteAsync(string id);
    Task<List<Note>> SearchNotesAsync(string searchText);
}
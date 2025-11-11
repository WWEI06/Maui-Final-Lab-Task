namespace MauiNoteTaker3._0.Utilities;

public static class PageHelper
{
    public static Page? GetCurrentPage()
    {
        return Application.Current?.Windows.FirstOrDefault()?.Page;
    }
}
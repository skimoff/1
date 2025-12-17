using System.IO;
using System.Text.Json;
using System.Windows;

namespace TypingTest.Core;

public class SettingsManager
{
   private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TypingTest");
    private static readonly string FilePath = Path.Combine(FolderPath, "settings.json");
    public static string UserName { get; set; }

    public static string Language { get; set; } = "ukr";
    public static string Theme { get; set; } = "Dark"; // "Dark" или "Light"

    static SettingsManager()
    {
        Load();
    }

    public static void Save()
    {
        try
        {
            // ВАЖНО: Если папки "TypingTest" в AppData нет, создаем её
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var settings = new
            {
                Language = Language,
                Theme = Theme,
                UserName = UserName 
            };

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        
            // Для отладки (можно потом удалить)
            System.Diagnostics.Debug.WriteLine($"Файл сохранен по пути: {FilePath}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА СОХРАНЕНИЯ: {ex.Message}");
        }
    }

    public static void Load()
    {
        if (!File.Exists(FilePath))
        {
            ApplyTheme();
            return;
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            using var doc = JsonDocument.Parse(json);
            
            // Читаем старые свойства
            Language = doc.RootElement.GetProperty("Language").GetString() ?? "ukr";
            Theme = doc.RootElement.GetProperty("Theme").GetString() ?? "Dark";
            
            // Читаем НОВОЕ свойство (имя). 
            // TryGetProperty нужен, чтобы программа не вылетала, если файла еще нет или там нет этого поля
            if (doc.RootElement.TryGetProperty("UserName", out var nameProp))
            {
                UserName = nameProp.GetString();
            }

            ApplyTheme();
        }
        catch 
        { 
            ApplyTheme(); 
        }
    }

    public static void ApplyTheme()
    {
        string themeFile = Theme == "Light" ? "LightStyle.xaml" : "DarkStyle.xaml";

        try
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;
        
            // Очищаем старые словари
            dictionaries.Clear(); 

            var uri = new Uri($"/TypingTest;component/Resources/Styles/{themeFile}", UriKind.RelativeOrAbsolute);
        
            // Загружаем новый словарь
            if (Application.LoadComponent(uri) is ResourceDictionary resDict)
            {
                dictionaries.Add(resDict);
            }

            // ВАЖНО: Если у тебя открыто несколько окон, иногда нужно явно сказать UI обновиться
            foreach (Window window in Application.Current.Windows)
            {
                window.InvalidateVisual();
            }

            System.Diagnostics.Debug.WriteLine($"Тема успешно изменена на: {themeFile}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА ЗАГРУЗКИ ТЕМЫ: {ex.Message}");
        }
    }
}
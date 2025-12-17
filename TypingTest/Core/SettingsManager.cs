using System.IO;
using System.Text.Json;
using System.Windows;

namespace TypingTest.Core;

public class SettingsManager
{
   private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TypingTest");
    private static readonly string FilePath = Path.Combine(FolderPath, "settings.json");

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
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            
            var data = new { Language, Theme };
            File.WriteAllText(FilePath, JsonSerializer.Serialize(data));
            
            ApplyTheme(); // Мгновенно обновляем цвета в окнах
        }
        catch (Exception ex) 
        {
            System.Diagnostics.Debug.WriteLine("Ошибка сохранения настроек: " + ex.Message);
        }
    }

    private static void Load()
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
            Language = doc.RootElement.GetProperty("Language").GetString() ?? "ukr";
            Theme = doc.RootElement.GetProperty("Theme").GetString() ?? "Dark";
            ApplyTheme();
        }
        catch 
        { 
            ApplyTheme(); 
        }
    }

    public static void ApplyTheme()
    {
        string themeFile = Theme == "Light" ? "LigthStyle.xaml" : "DarkStyle.xaml";
    
        // Формат: /НазваниеСборки;component/ПутьОтКорняПроекта
        // Замени "TypingTest" на точное название твоего проекта (Assembly Name)
        var uri = new Uri($"/TypingTest;component/Resources/Styles/{themeFile}", UriKind.RelativeOrAbsolute);

        try
        {
            ResourceDictionary resDict = Application.LoadComponent(uri) as ResourceDictionary;
        
            if (resDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resDict);
            }
        }
        catch (Exception ex)
        {
            // Если что-то пошло не так, выводим ошибку в консоль дебага
            System.Diagnostics.Debug.WriteLine($"КРИТИЧЕСКАЯ ОШИБКА ТЕМЫ: {ex.Message}");
            // Не даем приложению упасть молча
        }
    }
}
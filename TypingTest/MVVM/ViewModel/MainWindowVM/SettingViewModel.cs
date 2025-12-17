using TypingTest.Core;

namespace TypingTest.MVVM.ViewModel.MainWindowVM;

public class SettingViewModel : ObservableObject
{
    // Свойства для ЯЗЫКА
    public bool IsUkrSelected
    {
        get => SettingsManager.Language == "ukr";
        set
        {
            if (value)
            {
                SettingsManager.Language = "ukr";
                SettingsManager.Save();
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEngSelected));
            }
        }
    }

    public bool IsEngSelected
    {
        get => SettingsManager.Language == "eng";
        set
        {
            if (value)
            {
                SettingsManager.Language = "eng";
                SettingsManager.Save();
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUkrSelected));
            }
        }    
    }

    // Свойства для ТЕМЫ
    public bool IsDarkTheme
    {
        get => SettingsManager.Theme == "Dark";
        set
        {
            if (value)
            {
                SettingsManager.Theme = "Dark";
                SettingsManager.ApplyTheme(); // ДОБАВЬ ЭТУ СТРОКУ
                SettingsManager.Save();
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLightTheme));
            }
        }
    }

    public bool IsLightTheme
    {
        get => SettingsManager.Theme == "Light";
        set
        {
            if (value)  
            {
                SettingsManager.Theme = "Light";
                SettingsManager.ApplyTheme(); // ДОБАВЬ ЭТУ СТРОКУ
                SettingsManager.Save();
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDarkTheme));
            }
        }
    }
}
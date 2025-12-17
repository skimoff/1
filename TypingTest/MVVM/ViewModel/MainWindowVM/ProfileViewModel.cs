using System;
using System.ComponentModel;
using System.Windows.Input;
using TypingTest.Core;
using TypingTest.MVVM.Model;

namespace TypingTest.MVVM.ViewModel.MainWindowVM;

public class ProfileViewModel:ObservableObject
{
    private UserModel _currentUser;
    private string _userPhotoPath;
    private TestResult _bestStats;

    // Свойства (оставляем как есть)
    public UserModel CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); OnPropertyChanged(nameof(UserDisplayName)); } }
    public string UserPhotoPath { get => _userPhotoPath; set { _userPhotoPath = value; OnPropertyChanged(); } }
    public TestResult BestStats { get => _bestStats; set { _bestStats = value; OnPropertyChanged(); OnPropertyChanged(nameof(BestWPM)); OnPropertyChanged(nameof(BestAccuracy)); } }

    // Текстовые свойства для View
    public string UserDisplayName => CurrentUser?.Username ?? "Гость";
    public string BestWPM => $"Найкраща швидкість: {BestStats?.WPM ?? 0} СЛОВ/ХВ";
    public string BestAccuracy => $"Найкраща точність: {BestStats?.Accuracy.ToString("F2") ?? "0.00"} %";
    public string TotalTests => $"Всього тестів пройдено: {StatisticsManager.GetTotalTestsCount()}";

    
    public ICommand LoadDataCommand { get; }

    public ProfileViewModel()
    {
        
        LoadDataCommand = new RelayCommand(param => LoadData());

        // Подписываемся на обновление статистики
        StatisticsManager.OnStatisticsChanged += Refresh; 
        
        LoadData();
    }

    public void LoadData()
    {
        UserPhotoPath = "pack://application:,,,/Resources/Images/profileImages.png";
        SettingsManager.Load();

        // 2. Инициализируем юзера данными из настроек
        if (CurrentUser == null) 
        {
            CurrentUser = new UserModel(); 
        }

        // Заменяем "Skimoff" на имя из настроек. Если пусто — пишем "Гість"
        CurrentUser.Username = !string.IsNullOrEmpty(SettingsManager.UserName) 
            ? SettingsManager.UserName 
            : "Гість";

        // Уведомляем интерфейс, что имя изменилось
        OnPropertyChanged(nameof(UserDisplayName));

        // 3. Берем свежие статы
        BestStats = StatisticsManager.GetBestStats(); 
    
        // ПРИНУДИТЕЛЬНО уведомляем UI об изменениях
        OnPropertyChanged(nameof(TotalTests));
        OnPropertyChanged(nameof(BestWPM));
        OnPropertyChanged(nameof(BestAccuracy));
    }

    public void Refresh()
    {
        // Вызывается автоматически, когда StatisticsManager сохраняет новый результат
        LoadData(); 
    }
    
}
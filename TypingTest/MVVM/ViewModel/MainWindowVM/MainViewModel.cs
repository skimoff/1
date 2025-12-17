using System.Windows.Input;
using TypingTest.Core;

namespace TypingTest.MVVM.ViewModel.MainWindowVM;

public class MainViewModel:ObservableObject
{
    public RelayCommand ProfileViewCommand { get; set; }
    public RelayCommand SettingViewCommand { get; set; }
    public RelayCommand TestViewCommand { get; set; }
    public RelayCommand GuideViewCommand { get; set; }

    private ProfileViewModel ProfileVm { get; set; }
    private SettingViewModel SettingVm { get; set; }
    private TestViewModel TestVm { get; set; }
    private GuideViewModel GuideVm { get; set; }
    
    private object _currentView;
    
    private double _blurRadius;
    private bool _isRegistrationVisible;
    private string _newUserName;

    public object CurrentView
    {
        get{return _currentView;}
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        // 1. Создаем объекты (Оставляем как есть, это верно)
        ProfileVm = new ProfileViewModel();
        SettingVm = new SettingViewModel();
        TestVm = new TestViewModel();
        GuideVm = new GuideViewModel();

        // 2. Загружаем настройки
        SettingsManager.Load(); 

        // 3. ПРОВЕРКА (Добавил принудительное обновление)
        if (string.IsNullOrEmpty(SettingsManager.UserName))
        {
            BlurRadius = 15; 
            IsRegistrationVisible = true; 
        }
        else
        {
            // Если имя есть, гасим всё сразу
            BlurRadius = 0;
            IsRegistrationVisible = false;
            ProfileVm.LoadData();
        }

        // 4. Команда сохранения (Добавил проверку на обновление интерфейса)
        SaveNameCommand = new RelayCommand(o =>
        {
            if (!string.IsNullOrWhiteSpace(NewUserName))
            {
                // Сохраняем
                SettingsManager.UserName = NewUserName;
                SettingsManager.Save();

                // Обновляем ProfileVm ДО того, как уберем блюр, 
                // чтобы пользователь сразу увидел свое имя
                ProfileVm.LoadData();

                // Выключаем оверлей
                BlurRadius = 0;
                IsRegistrationVisible = false;
            }
        });

        // 5. Стартовый экран и команды (Верно)
        CurrentView = ProfileVm;
    
        ProfileViewCommand = new RelayCommand(o => CurrentView = ProfileVm);
        SettingViewCommand = new RelayCommand(o => CurrentView = SettingVm);
        TestViewCommand = new RelayCommand(o => CurrentView = TestVm);
        GuideViewCommand = new RelayCommand(o => CurrentView = GuideVm);
    }
    public double BlurRadius 
    { 
        get => _blurRadius; 
        set { _blurRadius = value; OnPropertyChanged(); } 
    }

    public bool IsRegistrationVisible 
    { 
        get => _isRegistrationVisible; 
        set { _isRegistrationVisible = value; OnPropertyChanged(); } 
    }

    public string NewUserName 
    { 
        get => _newUserName; 
        set { _newUserName = value; OnPropertyChanged(); } 
    }

    public ICommand SaveNameCommand { get; }
}
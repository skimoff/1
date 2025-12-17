using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TypingTest.MVVM.ViewModel.MainWindowVM;

namespace TypingTest.MVVM.View;

public partial class TestView : UserControl
{
    public TestView()
    {
        InitializeComponent();
        Loaded += (s, e) => FocusInput();
            
        // Если пользователь кликнул по области теста — возвращаем фокус
        MouseDown += (s, e) => FocusInput();

        // Автофокус при нажатии кнопок (например, "Обновить текст")
        PreviewMouseLeftButtonDown += (s, e) => 
        {
            if (e.OriginalSource is Button)
            {
                Dispatcher.BeginInvoke(new Action(() => FocusInput()), DispatcherPriority.Background);
            }
        };
    }
    private void FocusInput()
    {
        if (InputBox != null)
        {
            InputBox.Focus();
            Keyboard.Focus(InputBox);
        }
    }
}
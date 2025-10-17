using System.IO;
using System.Windows;
using System.Windows.Input;


namespace TypingTest.MVVM.View;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }
    
    
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void Button_Quit(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
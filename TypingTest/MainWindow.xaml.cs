using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace TypingTest;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ShowExitConfirmation()
    {
        Window confirmWindow = new Window
        {
            Title = "Подтверждение",
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ResizeMode = ResizeMode.NoResize,
            WindowStyle = WindowStyle.ToolWindow,
            Owner = Application.Current.MainWindow
        };

        StackPanel panel = new StackPanel
        {
            Margin = new Thickness(10),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        TextBlock text = new TextBlock
        {
            Text = "Вы действительно хотите выйти?",
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 0, 0, 15)
        };

        StackPanel buttonsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        Button yesButton = new Button
        {
            Content = "Да",
            Width = 70,
            Margin = new Thickness(5, 0, 5, 0)
        };
        yesButton.Click += (s, e) =>
        {
            confirmWindow.DialogResult = true;
            confirmWindow.Close();
        };

        // кнопка Нет
        Button noButton = new Button
        {
            Content = "Нет",
            Width = 70,
            Margin = new Thickness(5, 0, 5, 0)
        };
        noButton.Click += (s, e) =>
        {
            confirmWindow.DialogResult = false;
            confirmWindow.Close();
        };
        
        buttonsPanel.Children.Add(yesButton);
        buttonsPanel.Children.Add(noButton);
        
        panel.Children.Add(text);
        panel.Children.Add(buttonsPanel);
        
        confirmWindow.Content = panel;
        
        bool? result = confirmWindow.ShowDialog();

        if (result == true)
        {
            Application.Current.Shutdown();
        }
    }

    private void Button_Quit_Click(object sender, RoutedEventArgs e)
    {
        ShowExitConfirmation();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}
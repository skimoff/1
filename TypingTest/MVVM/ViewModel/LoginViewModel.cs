using TypingTest.Core;

namespace TypingTest.MVVM.ViewModel;

public class LoginViewModel
{
    private readonly Database _db = new();

    public string Username { get; set; }
    public string Password { get; set; }

    public void Login()
    {
        if (_db.CheckUser(Username, Password))
        {
            MessageBox.Show("Добро пожаловать, " + Username + "!");
            // открыть главное окно приложения
        }
        else
        {
            MessageBox.Show("Неверное имя или пароль!");
        }
    }
}
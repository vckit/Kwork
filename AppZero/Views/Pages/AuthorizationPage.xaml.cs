using AppZero.Context;
using AppZero.Views.Pages.AdminPages;
using AppZero.Views.Pages.EmployePages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AppZero.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentUser = AppData.db.SignIn.FirstOrDefault(item => item.Username == txbUsername.Text && item.Password == psbPassword.Password);

                if (currentUser != null)
                {
                    switch (currentUser.IDRole)
                    {

                        case "A":
                            NavigationService.Navigate(new ViewPage(currentUser.User.FirstOrDefault(item => item.IDSignIn == currentUser.ID)));
                            break;
                        case "U":
                            NavigationService.Navigate(new ViewPageEmp());
                            break;
                        default:
                            throw new Exception("Неверный логин или пароль!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

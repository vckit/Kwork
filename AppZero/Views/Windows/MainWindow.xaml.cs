using AppZero.Context;
using AppZero.Views.Pages;
using System;
using System.Windows;

namespace AppZero
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new AuthorizationPage());
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтвердите.", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AppData.db.SaveChanges();
                mainFrame.Navigate(new AuthorizationPage());
            }
        }

        private void mainFrame_ContentRendered(object sender, EventArgs e)
        {
            btnSignOut.Visibility = mainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

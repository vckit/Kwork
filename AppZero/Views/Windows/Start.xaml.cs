using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppZero.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(5000); // ожидание 5 секунд
            progressBar.IsIndeterminate = false; // отключение анимации
            var newWindow = new MainWindow(); // создание нового окна
            newWindow.Show(); // открытие нового окна
            Close(); // закрытие текущего окна

        }
    }
}

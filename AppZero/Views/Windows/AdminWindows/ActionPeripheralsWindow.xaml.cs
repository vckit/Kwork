using AppZero.Context;
using AppZero.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AppZero.Views.Windows.AdminWindows
{
    /// <summary>
    /// Логика взаимодействия для ActionPeripheralsWindow.xaml
    /// </summary>
    public partial class ActionPeripheralsWindow : Window
    {
        public Peripherals Peripherals { get; set; }
        public ActionPeripheralsWindow(Peripherals peripherals)
        {
            InitializeComponent();
            Peripherals = peripherals;
            this.DataContext = this;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCount.Text == "0" || txbDescription.Text == "" || txbRackNumber.Text == "" || txbShelfNumber.Text == "")
                    throw new Exception("ВНИМАНИЕ! Пустые значения не допустимы.");

                if (Peripherals.ID == 0)
                {
                    if (AppData.db.Peripherals.Count(item => item.RackNumber == txbRackNumber.Text || item.ShelfNumber == txbShelfNumber.Text) > 0)
                    {
                        throw new Exception("ВНИМАНИЕ! Данные номера стеллажа или номера шкафа повторяются.");
                    }
                    else
                    {
                        Peripherals.DateAdded = DateTime.Now;
                        AppData.db.Peripherals.Add(Peripherals);
                    }
                }
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены в базе данных!", "Операция прошла успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Запрещаем вводить всё, кроме перечисленных цифр
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "1234567890".IndexOf(e.Text) < 0;
        }
    }
}

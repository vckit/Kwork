using AppZero.Context;
using AppZero.Model;
using AppZero.Views.Windows.AdminWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;

namespace AppZero.Views.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ViewPage.xaml
    /// </summary>
    public partial class ViewPage : Page
    {
        public List<SpareParts> SparePartsDestination = new List<SpareParts>();
        public List<Peripherals> PeripheralsDestination = new List<Peripherals>();
        public User User { get; set; }
        public SignIn SignIn { get; set; }
        public List<Position> Positions { get; set; }
        public List<Rule> Rules { get; set; }
        public User CurrentUser { get; set; }

        public ViewPage(User currentUser)
        {
            InitializeComponent();
            User = new User();
            SignIn = new SignIn();
            Positions = AppData.db.Position.ToList();
            Rules = AppData.db.Rule.ToList();
            CurrentUser = currentUser;
            this.DataContext = this;

            MessageBox.Show($"Добро пожаловать {currentUser.FirstName} {CurrentUser.LastName}! Вы авторизовались правами Администратора.",
                "Систеиное уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Добавляем сотрудника в БД
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbFirstName.Text == "" ||
                     txbLastName.Text == "" ||
                     txbMiddleName.Text == "" ||
                     txbPassword.Text == "" ||
                     txbUsername.Text == "" ||
                     cmbPosition.Text == "" ||
                     cmbRule.Text == "")
                    throw new Exception("Заполните поля!");

                if (User.ID == 0 && SignIn.ID == 0)
                {
                    if (AppData.db.SignIn.Count(item => item.Username == SignIn.Username) > 0)
                        throw new Exception("Пользователь: " + SignIn.Username + " уже существует!");
                    SignIn.IDRole = AppData.db.Rule.FirstOrDefault(item => item.Title == cmbRule.Text).ID;
                    AppData.db.SignIn.Add(SignIn);
                    AppData.db.User.Add(User);
                }
                if (MessageBox.Show("Вы уверены в правильности данных пользователя? Учтите, данные пользователей нельзя редактировать!", "Подтвердите действие.",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены в базу данных!", "Данные сохранены.", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearUserData();
                    Page_Loaded(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearUserData()
        {
            txbFirstName.Text = "";
            txbLastName.Text = "";
            txbMiddleName.Text = "";
            txbPassword.Text = "";
            txbUsername.Text = "";
            cmbPosition.Text = "";
            cmbRule.Text = "";
        }

        // Выгружаем данные из БД
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListDataEmp.ItemsSource = AppData.db.User.ToList();
            ListDataSpareParts.ItemsSource = AppData.db.SpareParts.ToList();
            listDataPeripher.ItemsSource = AppData.db.Peripherals.ToList();
        }

        // Поиск сотрудников
        private void txbSearchEmp_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Поиск по следующим критериям: ID, Имя, Фамилия и Имя пользователя
            ListDataEmp.ItemsSource = AppData.db.User.Where(item => item.FirstName.Contains(txbSearchEmp.Text) ||
            item.LastName.Contains(txbSearchEmp.Text) ||
            item.SignIn.Username.Contains(txbSearchEmp.Text) ||
            item.ID.ToString().Contains(txbSearchEmp.Text)).ToList();
        }

        // Поиск Запчастей и Устройств
        private void txbSearchDevice_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Поиск по следующим критериям: ID, Номер стеллажа, Номер полки и Количество на складе
            ListDataSpareParts.ItemsSource = AppData.db.SpareParts.Where(item => item.ID.ToString().Contains(txbSearchDevice.Text) ||
            item.RackNumber.Contains(txbSearchDevice.Text) || item.ShelfNumber.Contains(txbSearchDevice.Text) ||
            item.Count.ToString().Contains(txbSearchDevice.Text)).ToList();
        }

        private void sortDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ListDataSpareParts.ItemsSource = AppData.db.SpareParts.Where(item => item.DateAdded == sortDate.SelectedDate).ToList();
        }

        // Поиск Периферии
        private void txbSearchPeripher_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Поиск по следующим критериям: ID, Номер стеллажа, Номер полки и Количество на складе
            listDataPeripher.ItemsSource = AppData.db.Peripherals.Where(item => item.ID.ToString().Contains(txbSearchPeripher.Text) ||
            item.RackNumber.Contains(txbSearchPeripher.Text) || item.ShelfNumber.Contains(txbSearchPeripher.Text) ||
            item.Count.ToString().Contains(txbSearchPeripher.Text)).ToList();
        }

        private void sortDatePeripher_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            listDataPeripher.ItemsSource = AppData.db.Peripherals.Where(item => item.DateAdded == sortDatePeripher.SelectedDate).ToList();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            sortDate.SelectedDate = null;
            sortDatePeripher.SelectedDate = null;
            Page_Loaded(null, null);
        }

        // Запчасти
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpStartDate.SelectedDate != null && dtpEndDate.SelectedDate != null)
                {
                    SparePartsDestination = AppData.db.SpareParts.Where(item => item.DateAdded >= dtpStartDate.SelectedDate && item.DateAdded <= dtpEndDate.SelectedDate).ToList();
                    ExportSparePartsDataPDF();
                }
                else
                {
                    throw new Exception("Укажите дату!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void dtpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ExportSparePartsDataPDF()
        {
            var word = new Word.Application();
            try
            {
                var document = word.Documents.Add();
                var paragrah = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragrah.Range;
                var listDataSpareParts = SparePartsDestination;
                var table = document.Tables.Add(tableRange, listDataSpareParts.Count + 1, 6);
                table.Range.Font.Size = 10;
                table.Borders.Enable = 1;
                table.Title = "Данные";
                table.Cell(1, 1).Range.Text = "Номер стеллажа";
                table.Cell(1, 2).Range.Text = "Номер полки";
                table.Cell(1, 3).Range.Text = "Описание";
                table.Cell(1, 4).Range.Text = "Тип";
                table.Cell(1, 5).Range.Text = "Количество";
                table.Cell(1, 6).Range.Text = "Дата";

                int i = 2;
                foreach (var item in listDataSpareParts)
                {
                    table.Cell(i, 1).Range.Text = item.RackNumber;
                    table.Cell(i, 2).Range.Text = item.ShelfNumber;
                    table.Cell(i, 3).Range.Text = item.Description;
                    table.Cell(i, 4).Range.Text = item.TypeObject.Title;
                    table.Cell(i, 5).Range.Text = item.Count.ToString();
                    table.Cell(i, 6).Range.Text = item.DateAdded.ToString();
                    i++;
                }
                document.SaveAs2($"{Environment.CurrentDirectory}\\Data.pdf", Word.WdSaveFormat.wdFormatPDF);
                //document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                MessageBox.Show($"Документ успешно сформирован, расположение: {Environment.CurrentDirectory}\\Data.pdf!", "Документ успешно сформирован.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source + "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
            }
        }
        private void ExportPeripherDataPDF()
        {
            var word = new Word.Application();
            try
            {
                var document = word.Documents.Add();
                var paragrah = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragrah.Range;
                var listDataSpareParts = PeripheralsDestination;
                var table = document.Tables.Add(tableRange, listDataSpareParts.Count + 1, 5);
                table.Range.Font.Size = 10;
                table.Borders.Enable = 1;
                table.Title = "Данные";
                table.Cell(1, 1).Range.Text = "Номер стеллажа";
                table.Cell(1, 2).Range.Text = "Номер полки";
                table.Cell(1, 3).Range.Text = "Описание";
                table.Cell(1, 4).Range.Text = "Количество";
                table.Cell(1, 5).Range.Text = "Дата";

                int i = 2;
                foreach (var item in listDataSpareParts)
                {
                    table.Cell(i, 1).Range.Text = item.RackNumber;
                    table.Cell(i, 2).Range.Text = item.ShelfNumber;
                    table.Cell(i, 3).Range.Text = item.Description;
                    table.Cell(i, 4).Range.Text = item.Count.ToString();
                    table.Cell(i, 5).Range.Text = item.DateAdded.ToString();
                    i++;
                }
                document.SaveAs2($"{Environment.CurrentDirectory}\\DataPeripher.pdf", Word.WdSaveFormat.wdFormatPDF);
                //document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                MessageBox.Show($"Документ успешно сформирован, расположение: {Environment.CurrentDirectory}\\DataPeripher.pdf!", "Документ успешно сформирован.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source + "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
            }
        }
        // Периферия
        private void btnPrintPeripher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpStartDatePeripher.SelectedDate != null && dtpEndDatePeripher.SelectedDate != null)
                {
                    PeripheralsDestination = AppData.db.Peripherals.Where(item => item.DateAdded >= dtpStartDatePeripher.SelectedDate && item.DateAdded <= dtpEndDatePeripher.SelectedDate).ToList();
                    ExportPeripherDataPDF();
                }
                else
                {
                    throw new Exception("Укажите дату!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Переход в окно добавления данных запчастей и устройств
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ActionSparePartsWindow actionSparePartsWindow = new ActionSparePartsWindow(new SpareParts());
            actionSparePartsWindow.ShowDialog();
        }
        // Переход в окно редактирования данных запчастей и устройств
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedSpareParts = (SpareParts)ListDataSpareParts.SelectedItem;
            if (selectedSpareParts != null)
            {
                ActionSparePartsWindow actionSparePartsWindow = new ActionSparePartsWindow(selectedSpareParts);
                actionSparePartsWindow.ShowDialog();
            }
        }
        // Удаление данных запчастей и устройств
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedSpareParts = (SpareParts)ListDataSpareParts.SelectedItem;
                if (selectedSpareParts != null)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить выбранный объект из Базы данных?", "Внимание! Данные удалятся навсегда.",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        AppData.db.SpareParts.Remove(selectedSpareParts);
                        AppData.db.SaveChanges();
                        Page_Loaded(null, null);
                        MessageBox.Show("Данные успешно удалились!", "Операция выполнена", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    throw new Exception("Выберите запись, чтобы удалить!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Переход в окно добавления данных периферии
        private void btnAddPeripherals_Click(object sender, RoutedEventArgs e)
        {
            ActionPeripheralsWindow actionPeripheralsWindow = new ActionPeripheralsWindow(new Peripherals());
            actionPeripheralsWindow.ShowDialog();
        }
        // Переход в окно редактирования данных периферии
        private void btnEditPeripherals_Click(object sender, RoutedEventArgs e)
        {
            var selectedPeripherals = (Peripherals)listDataPeripher.SelectedItem;
            if (selectedPeripherals != null)
            {
                ActionPeripheralsWindow actionPeripheralsWindow = new ActionPeripheralsWindow(selectedPeripherals);
                actionPeripheralsWindow.ShowDialog();
            }
        }
        // Удаление данных периферии
        private void btnRemovePeripherals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPeripherals = (Peripherals)listDataPeripher.SelectedItem;
                if (selectedPeripherals != null)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить выбранный объект из Базы данных?", "Внимание! Данные удалятся навсегда.",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        AppData.db.Peripherals.Remove(selectedPeripherals);
                        AppData.db.SaveChanges();
                        Page_Loaded(null, null);
                        MessageBox.Show("Данные успешно удалились!", "Операция выполнена", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new Exception("Выберите запись, чтобы удалить!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Удаление данных сотрудников
        private void btnDeleteEMP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItemEmp = (User)ListDataEmp.SelectedItem;
                if (selectedItemEmp != null)
                {
                    if (selectedItemEmp.ID == CurrentUser.ID)
                        throw new Exception("Вы не можете удалить свои данные!");
                    if (MessageBox.Show("Вы действительно хотите удалить выбранный объект из Базы данных?", "Внимание! Данные удалятся навсегда.",
                       MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var selectedSignInDate = AppData.db.SignIn.FirstOrDefault(item => item.ID == selectedItemEmp.IDSignIn);
                        AppData.db.User.Remove(selectedItemEmp);
                        AppData.db.SignIn.Remove(selectedSignInDate);
                        AppData.db.SaveChanges();
                        Page_Loaded(null, null);
                        MessageBox.Show("Данные успешно удалились!", "Операция выполнена", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    throw new Exception("Выберите запись, чтобы удалить!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListDataEmp_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var userData = (User)ListDataEmp.SelectedItem;
                if (userData != null)
                {
                    MessageBox.Show($"Имя пользователя: {userData.SignIn.Username} Пароль: {userData.SignIn.Username} Роль: {userData.SignIn.Rule.Title}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using ExportToExcelTools;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Adam_new
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        enum Mods { Main = 0, Brief = 1, Distribution = 2 };
        int WindowMode;        
        Single sum_report = 0;
        Size OldSize = new Size(800, 600);
        
        public MainWindow(int type)
        {
            InitializeComponent();
            ActiveUser.wnDataWork = this;
            ActiveUser.dt.Start();
            ActiveUser.dt.Interval = new TimeSpan(0, 0, 1);
            ActiveUser.dt.Tick += dt_Tick;
            ShowMeMainWindow();
            lTitle.Content = "LiveMonitor | " + ActiveUser.login;
            if (ActiveUser.UserType == 4) 
                MIsearch.Visibility = Visibility.Collapsed;
        }

        void dt_Tick(object sender, EventArgs e)
        {
            ActiveUser.tick_count++;
            if (ActiveUser.tick_count == 300)
                LogOut(null, null);
        }


        private void SearchBarAppear(object sender, RoutedEventArgs e)
        {
            if(WindowMode == 0 && ActiveUser.UserType != 4)
            {
            var w = new wnSearch(ActiveUser.UserType);//argument is a type of user
            w.Owner = this;
            w.Show();
            this.IsEnabled = true;
            }
        }       

        #region Buttons       

        private void AddNew(object sender, RoutedEventArgs e)
        {
            var w = new AddMenu(ActiveUser.UserType);//argument is a type of user
            w.Owner = this;
            w.ShowDialog();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            DataWork.CheckOut(ActiveUser.login, ActiveUser.password);
            Window w = new UserLogin();
            w.Show();
            this.Close();
        }

        private void GoToSettings(object sender, RoutedEventArgs e)
        {
            //this.IsEnabled = false;
            Window w = new wnSettings(ActiveUser.UserType);
            w.Owner = this;
            w.ShowDialog();
        }

        private void CallDeleteMenu(object sender, RoutedEventArgs e)
        {
            Window w = new wnDeleteTransaction();
            w.Owner = this;
            w.ShowDialog();
        }

        public void RefreshTable(object sender, EventArgs e)//сбивается поиск тут
        {
            /*if ((ActiveUser.wnAddMenu == null) && (ActiveUser.wnDistribution == null) &&
                (ActiveUser.wnDeleteTransaction == null) && (ActiveUser.wnSettings == null))
            {
                this.IsEnabled = true;
            }*/
            //if (!search_start)
            //{
            lStatusBar.Content = "Обновление таблиц...";
                switch (ActiveUser.UserType)
                {
                    case 1:
                        dgUser1.ItemsSource = DataWork.GetInfoUser1();
                        break;

                    case 2:
                        dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                        break;

                    case 3:
                        dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                        break;

                    case 22:
                        dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                        break;
                }
                lStatusBar.Content = "Таблицы обновлены";
            //}
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

       /* private void Refresh(object sender, RoutedEventArgs e)
        {
            switch (ActiveUser.UserID)
            {
                case 1:
                    dgUser1.ItemsSource = DataWork.GetInfoUser1();
                    break;

                case 2:
                    dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                    break;

                case 3:
                    dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                    break;
                case 5:
                    dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                    break;
            }
        }*/

        private void wMainWindow_Closed(object sender, EventArgs e)
        {
            DataWork.CheckOut(ActiveUser.login, ActiveUser.password);
            ActiveUser.wnDataWork = null;
        }

        private void Mouse_move(object sender, MouseEventArgs e)
        {
            ActiveUser.tick_count = 0;
        }

        /*private void wMainWindow_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if ((ActiveUser.wnAddMenu == null) && (ActiveUser.wnDistribution == null) &&
               (ActiveUser.wnDeleteTransaction == null) && (ActiveUser.wnSettings == null))
            
                this.IsEnabled = true;
            
        }*/

        /*private void wMainWindow_LayoutUpdated(object sender, EventArgs e)
        {
            if ((ActiveUser.wnAddMenu == null) && (ActiveUser.wnDistribution == null) &&
               (ActiveUser.wnDeleteTransaction == null) && (ActiveUser.wnSettings == null))            
                this.IsEnabled = true;
            
        }//3 раза запускается*/

        private void Admimsettings(object sender, RoutedEventArgs e)
        {
            var w = new wnAdminSettings();
            w.Owner = this;
            w.Show();
        }

        #endregion

        #region ReportFunctions
        //Report Functions
        
        List<ComboBoxItem> lst = new List<ComboBoxItem>();
        //Кнопка BriefReport
        private void GoToBriefReport(object sender, RoutedEventArgs e)
        {            
            ShowMeBriefReport();
            bReport.Background = SystemColors.ControlLightLightBrush;

            bDReport.Background = SystemColors.ControlLightBrush;
            bMain.Background = SystemColors.ControlLightBrush;            
        }
        private void GoToDistributionReports(object sender, RoutedEventArgs e)
        {           
                ShowMeDistributionReport();
                bDReport.Background = SystemColors.ControlLightLightBrush;                
                bMain.Background = SystemColors.ControlLightBrush;            
                bReport.Background = SystemColors.ControlLightBrush;            
        }
        private void GoToMainWindow(object sender, RoutedEventArgs e)
        {
            if (ActiveUser.UserType != 4) 
            {
                ShowMeMainWindow();
                bMain.Background = SystemColors.ControlLightLightBrush;
            }
            bReport.Background = SystemColors.ControlLightBrush;
            bDReport.Background = SystemColors.ControlLightBrush;
        }

        private void ClearWorkSpace()
        {            
            gBriefReport.Visibility = Visibility.Collapsed;
            dgUser1.Visibility = Visibility.Hidden;
            dgUser2.Visibility = Visibility.Hidden;
            dgReportDistribution.Visibility = Visibility.Collapsed;
            cStartDate.Visibility = Visibility.Collapsed;
            cFinishDate.Visibility = Visibility.Collapsed;
            //bSearch.Visibility = Visibility.Collapsed;
            //bAdd.Visibility = Visibility.Collapsed;
            //bDelete.Visibility = Visibility.Collapsed;
            cbProjectName.Visibility = Visibility.Collapsed;
            gDistributionSum.Visibility = Visibility.Collapsed;
            //tbAlltime.Visibility = Visibility.Hidden;
            cbAlltime.Visibility = Visibility.Hidden;
            cbAlltime.IsChecked = false;
            cbMarks_Report.Visibility = Visibility.Hidden;
            cbCurrency_Report.Visibility = Visibility.Hidden;
            this.Width = OldSize.Width;
            this.Height = OldSize.Height;
            this.MinWidth = 800;
            this.MinHeight = 600;
            lStatusBar.Content = "Готов";
        }
        private void ShowMeMainWindow()
        {
            ClearWorkSpace();
            WindowMode = (int)Mods.Main;
            //отобразить нужные элементы
            //bSearch.Visibility = Visibility.Visible;
            //bAdd.Visibility = Visibility.Visible;
            //bDelete.Visibility = Visibility.Visible;            
            switch (ActiveUser.UserType)
            {
                case 1:
                    //добавить, настройки пользователя, выход, обновить, поиск.
                    bDReport.Visibility = Visibility.Collapsed;
                    //bDelete.Visibility = Visibility.Collapsed;
                    dgUser1.Visibility = Visibility.Visible;
                    dgUser1.ItemsSource = DataWork.GetInfoUser1();                                      
                    Distribution_menu.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    dgUser2.Visibility = Visibility.Visible;
                    dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);                    
                    //все кнопки на панели(добавить, удалить, поиск, обновить, настройки пользователя, выход, оба отчета, печать) В окне "настройки пользователя" отсутствует кнопка "к настройкам администратора"
                    break;
                case 3:                    
                    dgUser2.Visibility = Visibility.Visible;
                    dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                    AdminSettings.Visibility = Visibility.Visible;
                    break;
                case 4:
                    ShowMeBriefReport();
                    bReport.Background = SystemColors.ControlLightLightBrush;
                    //bAdd.Visibility = Visibility.Collapsed;
                    bMain.Visibility = Visibility.Collapsed;
                    bReport.Margin = new Thickness(83,0,0,0);
                    bDReport.Margin = new Thickness(203,0,0,0);
                    //bDelete.Visibility = Visibility.Collapsed;
                    //bSearch.Visibility = Visibility.Collapsed;
                    Distribution_menu.Visibility = Visibility.Collapsed;
                    Transaction.Visibility = Visibility.Collapsed;
                    //оба отчета, обновить, печать, настройки пользователя, выход
                    break;
                case 22:
                    //Projects.Visibility = Visibility.Visible;
                    bDReport.Visibility = Visibility.Collapsed;
                    bDReport.IsEnabled = false;
                    dgUser2.Visibility = Visibility.Visible;
                    dgUser2.ItemsSource = DataWork.GetInfoUser2(ActiveUser.UserID);
                    Distribution_menu.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void ShowMeBriefReport()
        {
            ClearWorkSpace();
            tbTotalMinus.Text = "";
            tbTotalPlus.Text = "";
            tbTotalShort.Text = "";
            dgDistributionPercents.ItemsSource = null;
            dgPlus.ItemsSource = null;
            dgMinus.ItemsSource = null;
            WindowMode = (int)Mods.Brief;
            //cbProjectName.FontSize = 19;
            //Подготовка среды           
            
            lst.Clear();
            cbProjectName.Items.Clear();
            //ComboBoxItem tmp = new ComboBoxItem();
            //tmp.Content = "Введите проект";
            cbProjectName.Items.Insert(0,"Введите проект");            
            
            if (ActiveUser.UserType == 4)            
                lst = DataWork.GetCollection("project_name_user4");            
            else            
                lst = DataWork.GetCollection("name_of_project");
            //lst.Insert(0, tmp);
            foreach (ComboBoxItem cbi in lst)
                cbProjectName.Items.Add(cbi);
            
            cbProjectName.SelectedIndex = 0;

            //Отображение
            cbMarks_Report.Visibility = Visibility.Visible;
            cbCurrency_Report.Visibility = Visibility.Visible;
            gBriefReport.Visibility = Visibility.Visible;
            cStartDate.Visibility = Visibility.Visible;
            cFinishDate.Visibility = Visibility.Visible;
            cbProjectName.Visibility = Visibility.Visible;            
            cbAlltime.Visibility = Visibility.Visible;

            if ((ActiveUser.UserType == 1) || (ActiveUser.UserType == 22))
                dgDistributionPercents.Visibility = Visibility.Hidden;

            else
            {
                Thickness marginThickness = gSum.Margin;
                gSum.Width = 371;
                gSum.HorizontalAlignment = HorizontalAlignment.Right;
                marginThickness.Right = 10;
                marginThickness.Bottom = 0;                
            }
            
        }
        private void ShowMeDistributionReport()
        {
            ClearWorkSpace();
            tbTotalSum.Text = "";
            WindowMode = (int)Mods.Distribution;
            cbProjectName.Items.Clear();
            cbProjectName.FontSize = 16;
            dgReportDistribution.ItemsSource = null;
            //Отображение
            cStartDate.Visibility = Visibility.Visible;
            cFinishDate.Visibility = Visibility.Visible;
            dgReportDistribution.Visibility = Visibility.Visible;
            gDistributionSum.Visibility = Visibility.Visible;
            //tbAlltime.Visibility = Visibility.Visible;
            cbAlltime.Visibility = Visibility.Visible;
            cbProjectName.Visibility = Visibility.Visible;
            //OldSize = new Size(this.Width, this.Height);
            //this.MinWidth = 535;
            //this.Width = 535;
            //this.MinHeight = 435;
            //this.Height = 435;
            //комбобокс с именами акционеров
            lst.Clear();            
            lst = DataWork.GetDistributionNamesForReport(ActiveUser.UserID);            
            cbProjectName.Items.Add("Введите акционера");
            foreach (ComboBoxItem cbi in lst)                            
                if ((!cbProjectName.Items.Contains(cbi.Content))&&(cbi.Content!=""))
                    cbProjectName.Items.Add(cbi.Content);           

            cbProjectName.SelectedIndex = 0;            
        }        

        private void ReportStart(object sender, SelectionChangedEventArgs e)
        {
            if (cFinishDate.SelectedDate.HasValue & WindowMode != (int)Mods.Main)// проверить на вызовы
            {
                cFinishDate.DisplayDateStart = cStartDate.SelectedDate;
                if (cStartDate.SelectedDate > cFinishDate.SelectedDate)
                    cFinishDate.SelectedDate = cStartDate.SelectedDate;
                Start();
            }
        }

        private void Start()
        {
            switch (WindowMode)
            {
                case 1:
                    {
                        if (cbProjectName.SelectedIndex > 0)
                        {
                            //DateTime StartDate = Convert.ToDateTime(cStartDate.SelectedDate);
                            //DateTime FinishDate = Convert.ToDateTime(cFinishDate.SelectedDate);
                            //string name = cbProjectName.SelectedItem.ToString().Remove(0, 38);
                            List<DistributionPerson> lst_dp;
                            List<InfoUser2> lst_2 = DataWork.GetInfoReportShortWithSearch(Convert.ToDateTime(cStartDate.SelectedDate), Convert.ToDateTime(cFinishDate.SelectedDate), true, cbProjectName.SelectedItem.ToString().Remove(0, 38), cbCurrency_Report.SelectedValue.ToString().Remove(0, 38), cbMarks_Report.SelectedValue.ToString().Remove(0, 38), ActiveUser.UserID);
                            dgPlus.ItemsSource = lst_2;
                            List<InfoUser2> lst_1 = DataWork.GetInfoReportShortWithSearch(Convert.ToDateTime(cStartDate.SelectedDate), Convert.ToDateTime(cFinishDate.SelectedDate), false, cbProjectName.SelectedItem.ToString().Remove(0, 38), cbCurrency_Report.SelectedValue.ToString().Remove(0, 38), cbMarks_Report.SelectedValue.ToString().Remove(0, 38), ActiveUser.UserID);
                            dgMinus.ItemsSource = lst_1;
                            Single sum = 0;
                            Single sum_p = 0;
                            Single sum_m = 0;

                            foreach (InfoUser2 iu2 in lst_1)
                            {
                                sum_m += iu2.sum;
                                sum += iu2.sum;
                            }

                            foreach (InfoUser2 iu2 in lst_2)
                            {
                                sum_p += iu2.sum;
                                sum += iu2.sum;
                            }
                            sum_report = sum;
                            tbTotalPlus.Text = String.Format("{0:#,##0.00 $;-#,##0.00 $;0}", sum_p);
                            tbTotalMinus.Text = String.Format("{0:#,##0.00 $;-#,##0.00 $;0}", sum_m);

                            lst_dp = DataWork.GetDistributionList(cbProjectName.SelectedItem.ToString().Remove(0, 38));
                            tbTotalShort.Text = String.Format("{0:#,##0.00 $;-#,##0.00 $;0}", sum);
                            foreach (DistributionPerson dp in lst_dp)                            
                                if (dp.Percent != "")
                                    dp.sum = sum * Convert.ToInt32(dp.Percent) / 100;
                            
                            if (cbProjectName.SelectedIndex == 0)
                                lst.Clear();
                            dgDistributionPercents.ItemsSource = lst_dp;
                        }
                    }
                    break;
                case 2:
                    {
                        if (cbProjectName.SelectedIndex > 0)
                        {
                            //DateTime StartDate = Convert.ToDateTime(cStartDate.SelectedDate);
                            //DateTime FinishDate = Convert.ToDateTime(cFinishDate.SelectedDate);
                            List<ReportDistributionData> lst = DataWork.GetReportDistribution(cbProjectName.SelectedItem.ToString(), Convert.ToDateTime(cStartDate.SelectedDate), Convert.ToDateTime(cFinishDate.SelectedDate));
                            dgReportDistribution.ItemsSource = lst;
                            Single sum_d = 0;
                            foreach (ReportDistributionData rdd in lst)                            
                                sum_d += Convert.ToSingle(rdd.Sum);
                            
                            tbTotalSum.Text = String.Format("{0:#,##0.00 $; -#,##0.00 $;0}", sum_d);
                            //if (cbProjectName.SelectedIndex == 0)
                            // lst.Clear();
                        }
                    }
                    break;
            }
        }


        private void cbCurrency_Report_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((WindowMode == (int)Mods.Brief) && (cFinishDate.SelectedDate.HasValue) && (cStartDate.SelectedDate.HasValue))
                Start();
        }


        //Brief Report Functions end
        #endregion

        #region Focus

        /*private void tbProjectName_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbProjectName.Text == "Проект") tbProjectName.Text = "";
        }

        private void tbProjectName_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbProjectName.Text == "") tbProjectName.Text = "Проект";
        }

        private void tbReceiver_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbReceiver.Text == "Получатель") tbReceiver.Text = "";
        }

        private void tbReceiver_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbReceiver.Text == "") tbReceiver.Text = "Получатель";
        }

        private void tbWhoReceived_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbWhoReceived.Text == "На кого получено") tbWhoReceived.Text = "";
        }

        private void tbWhoReceived_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbWhoReceived.Text == "") tbWhoReceived.Text = "На кого получено";
        }

        private void tbBank_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbBank.Text == "Банк") tbBank.Text = "";
        }

        private void tbBank_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbBank.Text == "") tbBank.Text = "Банк";
        }*/

        #endregion

        private void cbAlltime_Checked(object sender, RoutedEventArgs e)
        {
            cStartDate.SelectedDate = new DateTime(2000, 01, 01);
            cStartDate.IsEnabled = false;
            cFinishDate.SelectedDate = DateTime.Now;
            cFinishDate.IsEnabled = false;
        }

        private void cbAlltime_Unchecked(object sender, RoutedEventArgs e)
        {
            cStartDate.IsEnabled = true;
            cFinishDate.IsEnabled = true;
        }

        private void GoToDistributions(object sender, RoutedEventArgs e)
        {
            //this.IsEnabled = false;
            Window w = new wnDistributions();
            w.Show();
        }
        private void FullSize(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                var brush = new ImageBrush();
                //var uriImageSource = new Uri(@"/WPFApp;component/Images/PIcon.png", UriKind.RelativeOrAbsolute);
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/LiveMonitor;component/Maximise_1.png", UriKind.RelativeOrAbsolute));//проблема
                
                Maximize.Background = brush;
                Maximize.ToolTip = "Развернуть";
            }
            else 
            {
                this.WindowState = WindowState.Maximized;
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/LiveMonitor;component/Maximise_2.png", UriKind.RelativeOrAbsolute));//проблема
                Maximize.Background = brush;
                Maximize.ToolTip = "Восстановить";
            }
        }

        private void Print(object sender, RoutedEventArgs e)
        {
            lStatusBar.Content = "Экспорт в Excel..";
            if ((WindowMode == (int)Mods.Distribution) && (cbProjectName.SelectedIndex > 0) && (cStartDate.SelectedDate.HasValue) && (cFinishDate.SelectedDate.HasValue))
            {
                DataGridExcelTools dget1 = new DataGridExcelTools();
                dget1.AddDataGrid(dgReportDistribution);
                string d = "Итого:^" + tbTotalSum.Text;
                dget1.AddText(d);
                dget1.Clean();
                dget1 = null;
            }
            if ((WindowMode == (int)Mods.Brief) && (cbProjectName.SelectedIndex > 0) && (cStartDate.SelectedDate.HasValue) && (cFinishDate.SelectedDate.HasValue))
            {
                DataGridExcelTools dget = new DataGridExcelTools();

                dget.AddDataGrid(dgPlus);
                string s = "Доходы:^" + tbTotalPlus.Text;

                dget.AddText(s);

                dget.AddDataGrid(dgMinus);
                s = "Расходы:^" + tbTotalMinus.Text;
                dget.AddText(s);
                if ((ActiveUser.UserType != 22) & (ActiveUser.UserType != 1))
                    dget.AddDataGrid(dgDistributionPercents);

                s = "Итого:^" + tbTotalShort.Text;
                dget.AddText(s);

                s = tbTotalMinus.Text;
                //Экспорт линии в эксель, разделяется запятой, пробелы не ставятся (иначе будут и в ячейках).
                dget.Clean(); //Обязательно!!!!        
                dget = null;
            }
            if (WindowMode == (int)Mods.Main)
            {
                DataGridExcelTools dg = new DataGridExcelTools();
                if (ActiveUser.UserType >= 2) dg.AddDataGrid(dgUser2);
                else dg.AddDataGrid(dgUser1);
                dg.Clean(); //Обязательно!!!! Чистит + отображает эксель
                dg = null;
            }
            lStatusBar.Content = "Экспортировано в Excel";
        }

        private void bLogOut_Copy1_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DataWork.CheckOut(ActiveUser.login, ActiveUser.password);
            ActiveUser.wnDataWork = null;
            this.Close();
        }

        //hotkeys
        bool ctrl = false;
        private void wMainWindow_KeyDown(object sender, KeyEventArgs e)
        {            
            switch (e.Key) 
            {
                case Key.F5 :
                    RefreshTable(null,null);
                    break;
                case Key.RightCtrl:
                case Key.LeftCtrl :
                     ctrl = true;                    
                    break;

                case Key.P:
                    if(ctrl)Print(null, null);
                    ctrl = false;
                    break;

                case Key.F:
                    if(ctrl) SearchBarAppear(null,null);
                    ctrl = false;
                    break;

                case Key.N:
                    if (ctrl) AddNew(null, null);
                    ctrl = false;
                    break;

                case Key.D:
                    if (ctrl) CallDeleteMenu(null, null);
                    ctrl = false;
                    break;

                case Key.B:
                    if (ctrl) Change(null, null);
                    ctrl = false;
                    break;
            }
        }

        private void Change(object sender, RoutedEventArgs e)
        {
            var w = new AddMenu(ActiveUser.UserType);//argument is a type of user
            w.Owner = this;
            w.tcID.SelectedIndex = 1;
            w.ShowDialog();            
        }
    }
}
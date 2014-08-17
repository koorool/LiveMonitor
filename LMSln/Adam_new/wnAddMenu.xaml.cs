using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Adam_new
{
    /// <summary>
    /// Interaction logic for AddMenu.xaml
    /// </summary>
    public partial class AddMenu : Window
    {
        string projectName;
        List<Control> Controls = new List<Control>();

        public AddMenu(int user_atr)
        {
            InitializeComponent();
            ActiveUser.wnAddMenu = this;
            projectName = "";
            cDate.SelectedDate = DateTime.Today;
            AddID(cbIDu1);
            tbID.Text = DataWork.GetNexID("id_of_transaction").ToString();
            update_comboboxes(ActiveUser.UserType);
            tbID.IsReadOnly = true;
            if (user_atr == 1)
            {
                scrvAdd.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                this.Height = 370;
            }
            else Show_second_user();

            //Заполнение Controls
            tbID.PreviewKeyDown += MoveBetweenControls;
            cbProjectName.PreviewKeyDown += MoveBetweenControls;
            cDate.PreviewKeyDown += MoveBetweenControls;
            tbSum.PreviewKeyDown += MoveBetweenControls;
            cbCurrency.PreviewKeyDown += MoveBetweenControls;
            tbMarks.PreviewKeyDown += MoveBetweenControls;
            tbNotes.PreviewKeyDown += MoveBetweenControls;
            tbRates.PreviewKeyDown += MoveBetweenControls;
            tbRKO.PreviewKeyDown += MoveBetweenControls;
            cbReceiver.PreviewKeyDown += MoveBetweenControls;
            сbWhoReceived.PreviewKeyDown += MoveBetweenControls;
            tbBank.PreviewKeyDown += MoveBetweenControls;
            //end Controls
            Controls.Add(tbID);
            Controls.Add(cbProjectName);
            Controls.Add(cDate);
            Controls.Add(tbSum);
            Controls.Add(cbCurrency);
            Controls.Add(tbMarks);
            Controls.Add(tbNotes);
            if (ActiveUser.UserType != 1)
            {
                Controls.Add(tbRates);
                Controls.Add(tbRKO);
            }
            Controls.Add(cbReceiver);
            Controls.Add(сbWhoReceived);
            if (ActiveUser.UserType != 1)
                Controls.Add(tbBank);
        }

        void Show_second_user()
        {
            spOnWhom.Visibility = System.Windows.Visibility.Visible;
            spRates.Visibility = System.Windows.Visibility.Visible;
            spRKO.Visibility = System.Windows.Visibility.Visible;
            spReceiver.Visibility = System.Windows.Visibility.Visible;
            spBank.Visibility = System.Windows.Visibility.Visible;
        }

        void update_comboboxes(int type)
        {
            if (type != 1)
            {
                List<ComboBoxItem> lst1 = new List<ComboBoxItem>();
                lst1.Clear();
                cbReceiver.Items.Clear();
                lst1 = DataWork.GetCollection("receiver");
                //var sortedList = lst1.OrderBy(x => x);
                foreach (ComboBoxItem cbi in lst1)                
                    if (!cbReceiver.Items.Contains(cbi.Content) & cbi.Content.ToString() != "")
                        cbReceiver.Items.Add(cbi.Content);
                

                /* cbReceiver.Items.SortDescriptions.Add(
         new SortDescription("Content", ListSortDirection.Ascending));*/
                cbReceiver.Items.Add("Новый");

                tbReceiver.Visibility = Visibility.Hidden;
                List<ComboBoxItem> lst2 = new List<ComboBoxItem>();
                lst2.Clear();
                сbWhoReceived.Items.Clear();
                lst2 = DataWork.GetCollection("who_received");

                foreach (ComboBoxItem cbi in lst2)                
                    if (!сbWhoReceived.Items.Contains(cbi.Content) & cbi.Content.ToString() != "")
                        сbWhoReceived.Items.Add(cbi.Content);
                
                /*сbWhoReceived.Items.SortDescriptions.Add(
        new SortDescription("Items", ListSortDirection.Ascending));
               lst2.Sort();*/
                сbWhoReceived.Items.Add("Новый");
            }

            List<ComboBoxItem> lst = new List<ComboBoxItem>();
            lst.Clear();
            cbProjectName.Items.Clear();
            lst = DataWork.GetCollection("name_of_project");
            foreach (ComboBoxItem cbi in lst)           
                cbProjectName.Items.Add(cbi);
            
            //lst.Sort();
        }

        void AddID(ComboBox cb)
        {
            cbIDu1.Items.Clear();
            List<ComboBoxItem> tmp = DataWork.GetIdCollection(ActiveUser.UserID);

            foreach (ComboBoxItem cbi in tmp)            
                cb.Items.Add(cbi);
            
        }


        private void SubmitAdd(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if (ActiveUser.UserType == 1)
            {
                if (tcID.SelectedIndex == 0)
                    if ((projectName != "") && (cDate.SelectedDate < DateTime.Now) && (cbCurrency.SelectedIndex != -1) && (tbMarks.SelectedIndex != -1) && (tbSum.Text != ""))
                    {
                        if (tbNotes.Text == "")                        
                            tbNotes.Text = " ";
                        
                        count = DataWork.GetNexID("id_of_transaction");
                        DataWork.InsertUser1Info(count, projectName, Convert.ToDateTime(cDate.SelectedDate.ToString()),
                            cbCurrency.SelectedItem.ToString().Remove(0, 38), Convert.ToSingle(tbSum.Text), tbMarks.Text, tbNotes.Text, ActiveUser.UserID);
                        DataWork.InsertUser2Info(count, projectName, Convert.ToDateTime(cDate.SelectedDate), cbCurrency.SelectedItem.ToString().Remove(0, 38), Convert.ToSingle(tbSum.Text), tbMarks.Text, tbNotes.Text, 0, 0, tbReceiver.Text, tbWhoReceived.Text, tbBank.Text, ActiveUser.ActiveDistribution_id, false);
                        Window wn = new wnError("Транзакция создана успешно", 2);
                        wn.ShowDialog();
                        tbID.Text = DataWork.GetNexID("id_of_transaction").ToString();
                        ClearFields();
                        update_comboboxes(ActiveUser.UserType);
                        cbProjectName.SelectedIndex = -1;
                    }
                    else
                    {
                        Window wn = new wnError("Не все поля были заполнены", 2);
                        wn.ShowDialog();
                    }
                else
                {
                    int id = Convert.ToInt32(cbIDu1.SelectedItem.ToString().Remove(0, 38));
                    if ((projectName != "") && (cDate.SelectedDate < DateTime.Now) && (cbCurrency.SelectedIndex != -1) && (tbMarks.SelectedIndex != -1) && (tbSum.Text != ""))
                    {
                        if (tbNotes.Text == "")                        
                            tbNotes.Text = " ";
                                                
                        DataWork.UpdateUser1Info(id, projectName, Convert.ToDateTime(cDate.SelectedDate), cbCurrency.SelectedItem.ToString().Remove(0, 38), Convert.ToSingle(tbSum.Text), tbMarks.Text);
                        DataWork.InsertUser2Info(Convert.ToInt32(tbID.Text), projectName, Convert.ToDateTime(cDate.SelectedDate), cbCurrency.SelectedItem.ToString().Remove(0, 38), Convert.ToSingle(tbSum.Text), tbMarks.Text, tbNotes.Text, 0, 0, tbReceiver.Text, tbWhoReceived.Text, tbBank.Text, ActiveUser.ActiveDistribution_id, false);
                        Window wn = new wnError("Данные изменены", 2);
                        wn.ShowDialog();
                        ClearFields();
                        cbIDu1.SelectedIndex = -1;
                    }
                    else
                    {
                        Window wn = new wnError("Не все поля были заполнены", 2);
                        wn.ShowDialog();
                    }
                }
            }
            else
            {
                if (tcID.SelectedIndex == 1)
                {
                    if ((projectName != "") && (cDate.SelectedDate < DateTime.Now) && (cbCurrency.SelectedIndex != -1) && (tbMarks.SelectedIndex != -1) && (tbReceiver.Text != "") && (tbWhoReceived.Text != "") && (tbBank.Text != "") && (tbSum.Text != ""))
                    {
                        //string str = cbIDu1.SelectedItem.ToString().Remove(0, 38);
                        if (tbNotes.Text == "")                        
                            tbNotes.Text = " ";


                        DataWork.InsertUser2Info(Convert.ToInt32(cbIDu1.SelectedItem.ToString().Remove(0, 38)), projectName, Convert.ToDateTime(cDate.SelectedDate), cbCurrency.SelectedItem.ToString().Remove(0, 38), Convert.ToSingle(tbSum.Text), tbMarks.Text, tbNotes.Text, Convert.ToSingle(tbRKO.Text), Convert.ToSingle(tbRates.Text), tbReceiver.Text, tbWhoReceived.Text, tbBank.Text, ActiveUser.ActiveDistribution_id, true);
                        Window wn = new wnError("Данные изменены", 2);
                        wn.ShowDialog();
                        ClearFields();
                        cbIDu1.SelectedIndex = -1;

                    }
                    else
                    {
                        Window wn = new wnError("Не все поля были заполнены", this);
                        wn.ShowDialog();
                    }
                }
                else
                {
                    if ((projectName != "") && (cDate.SelectedDate < DateTime.Now) && (cbCurrency.SelectedIndex != -1) && (tbMarks.SelectedIndex != -1) && (tbReceiver.Text != "") && (tbWhoReceived.Text != "") && (tbBank.Text != "") && (tbSum.Text != ""))
                    {
                        if (tbNotes.Text == "")                        
                            tbNotes.Text = " ";
                        

                        count = DataWork.GetNexID("id_of_transaction");
                        DataWork.InsertUser1Info(count, projectName, Convert.ToDateTime(cDate.SelectedDate), cbCurrency.SelectedItem.ToString().Remove(0, 38), Convert.ToSingle(tbSum.Text), tbMarks.Text, tbNotes.Text, ActiveUser.UserID);
                        DataWork.InsertUser2Info(count, projectName, Convert.ToDateTime(cDate.SelectedDate), cbCurrency.SelectedItem.ToString().Remove(0, 38), Convert.ToSingle(tbSum.Text), tbMarks.Text, tbNotes.Text, Convert.ToSingle(tbRKO.Text), Convert.ToSingle(tbRates.Text), tbReceiver.Text, tbWhoReceived.Text, tbBank.Text, ActiveUser.ActiveDistribution_id, false);
                        Window wn = new wnError("Транзакция создана успешно", 2);
                        wn.ShowDialog();
                        ClearFields();
                        update_comboboxes(ActiveUser.UserType);
                        tbID.Text = DataWork.GetNexID("id_of_transaction").ToString();
                        cbProjectName.SelectedIndex = -1;
                    }
                    else
                    {
                        Window wn = new wnError("Не все поля были заполнены", 2);
                        wn.ShowDialog();
                    }
                }

            }
        }

       /* private void CancelAdd(object sender, RoutedEventArgs e)
        {
            this.Close();
        } */       

        private void U1_ChangeInfo(object sender, SelectionChangedEventArgs e)
        {
            if (cbIDu1.SelectedIndex != -1)
            {
                if (ActiveUser.UserType == 1)
                {
                    //string str = cbIDu1.SelectedItem.ToString().Remove(0, 38);
                    List<string> lst = DataWork.GetCollection(Convert.ToInt32(cbIDu1.SelectedItem.ToString().Remove(0, 38)));
                    tbProjectName.Text = lst[0];
                    projectName = lst[0];
                    cDate.SelectedDate = Convert.ToDateTime(lst[1]);
                    tbSum.Text = lst[2];
                    if (lst[3] == "UAH")
                        cbCurrency.SelectedIndex = 0;
                    else
                        if (lst[3] == "USD")
                            cbCurrency.SelectedIndex = 1;
                        else
                            cbCurrency.SelectedIndex = 2;
                    tbMarks.Text = lst[4];
                    tbNotes.Text = lst[5];
                }
                else
                {
                    List<string> lst = DataWork.GetCollection(Convert.ToInt32(cbIDu1.SelectedItem.ToString().Remove(0, 38)));
                    tbSum.Text = lst[2];
                    tbProjectName.Text = lst[0];
                    projectName = lst[0];
                    cDate.SelectedDate = Convert.ToDateTime(lst[1]);
                    if (lst[3] == "UAH")
                        cbCurrency.SelectedIndex = 0;
                    else
                        if (lst[3] == "USD")
                            cbCurrency.SelectedIndex = 1;
                        else
                            cbCurrency.SelectedIndex = 2;
                    tbMarks.Text = lst[4];
                    tbNotes.Text = lst[5];
                    tbRKO.Text = lst[6];
                    tbRates.Text = lst[7];
                    tbReceiver.Text = lst[8];
                    tbWhoReceived.Text = lst[9];
                    tbBank.Text = lst[10];
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ActiveUser.wnAddMenu = null;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            ActiveUser.tick_count = 0;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if ((ActiveUser.wnDistribution == null) && (ActiveUser.wnError == null))            
                this.IsEnabled = true;
            
        }

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            if ((ActiveUser.wnDistribution == null) && (ActiveUser.wnError == null))            
                this.IsEnabled = true;            
        }


        void ClearFields()
        {
            tbBank.Text = "";
            tbMarks.Text = "";
            tbNotes.Text = "";            
            tbRates.Text = "";
            tbReceiver.Text = "";
            tbRKO.Text = "";
            tbSum.Text = "";
            tbWhoReceived.Text = "";
            cbCurrency.SelectedIndex = -1;
        }

        private void tcID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcID.SelectedIndex == 0)
            {

                ClearFields();
                cbProjectName.Visibility = Visibility.Visible;
                cbIDu1.Visibility = Visibility.Hidden;
                tbID.Visibility = Visibility.Visible;
                tbProjectName.Visibility = Visibility.Hidden;
                tbID.Width = 200;
                cbIDu1.Width = 0;
                cbProjectName.Width = 200;
                tbProjectName.Margin = new Thickness(30, 3, 0, 3);
                tbProjectName.Width = 75;
                cbProjectName.SelectedIndex = -1;
                cbReceiver.Visibility = Visibility.Visible;
                сbWhoReceived.Visibility = Visibility.Visible;
                tbReceiver.Visibility = Visibility.Hidden;
                tbWhoReceived.Visibility = Visibility.Hidden;
                cbReceiver.SelectedIndex = -1;
                сbWhoReceived.SelectedIndex = -1;
                tbReceiver.Margin = new Thickness(43, 3, 0, 3);
                tbWhoReceived.Margin = new Thickness(10, 3, 0, 3);
                tbReceiver.Width = 130;
                cbReceiver.Width = 122;
                tbWhoReceived.Width = 130;
                сbWhoReceived.Width = 122;
            }
            else if (tcID.SelectedIndex == 1)
            {
                ClearFields();
                cbProjectName.Visibility = Visibility.Hidden;
                tbID.Visibility = Visibility.Hidden;
                tbProjectName.Visibility = Visibility.Visible;
                cbIDu1.Visibility = Visibility.Visible;
                tbID.Width = 0;
                cbIDu1.Width = 200;
                cbProjectName.Width = 0;
                tbProjectName.Width = 200;
                tbProjectName.Margin = new Thickness(114, 3, 0, 3);

                cbReceiver.Visibility = Visibility.Hidden;
                сbWhoReceived.Visibility = Visibility.Hidden;
                tbReceiver.Visibility = Visibility.Visible;
                tbWhoReceived.Visibility = Visibility.Visible;

                tbReceiver.Width = 200;
                cbReceiver.Width = 0;
                tbReceiver.Margin = new Thickness(106, 3, 0, 3);//new "105,3,0,3"

                tbWhoReceived.Width = 200;
                сbWhoReceived.Width = 0;
                tbWhoReceived.Margin = new Thickness(70, 3, 2, 3);//new "70,3,2,3"           

            }
        }

        private void NumbersOnly(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Decimal || e.Key == Key.OemComma || e.Key == Key.OemPeriod || e.Key == Key.Oem2)
            {
                if (((TextBox)sender).Text == "")                
                    ((TextBox)sender).Text = "0,";

                
                else if (!((TextBox)sender).Text.Contains(','))
                    ((TextBox)sender).Text = ((TextBox)sender).Text + ",";
                ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
                e.Handled = true;
            }
            if (!(e.Key >= Key.D0 & e.Key <= Key.D9) & !(e.Key >= Key.NumPad0 & e.Key <= Key.NumPad9) & e.Key != Key.Back & e.Key != Key.Right & e.Key != Key.Left & e.Key != Key.Delete & e.Key != Key.Enter & e.Key != Key.Up & e.Key != Key.Down)            
                e.Handled = true;
            
        }
        private void NumbersAndMinusOnly(object sender, KeyEventArgs e)
        {
            //0 в начале, запятая в начале или в любом месте
            if (e.Key == Key.Decimal || e.Key == Key.OemComma || e.Key == Key.OemPeriod || e.Key == Key.Oem2)
            {
                if (((TextBox)sender).Text == "")                
                    ((TextBox)sender).Text = "0,";
                
                else if (!((TextBox)sender).Text.Contains(','))
                    ((TextBox)sender).Text = ((TextBox)sender).Text + ",";
                ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
                e.Handled = true;
            }
            //Обработка минуса
            if ((e.Key == Key.Subtract || e.Key == Key.OemMinus))
                if (((TextBox)sender).Text == "")
                {
                    ((TextBox)sender).Text = "-";
                    ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
                    e.Handled = true;
                }
            //Ввод цифр на клавиатуре, дополнительной клавиатуре, нажатие стрелок "влево" и "вправо", Delete, BackSpace,
            if (!(e.Key >= Key.D0 & e.Key <= Key.D9) & !(e.Key >= Key.NumPad0 & e.Key <= Key.NumPad9) & e.Key != Key.Back & e.Key != Key.Right & e.Key != Key.Left & e.Key != Key.Delete & e.Key != Key.Enter & e.Key != Key.Up & e.Key != Key.Down)            
                e.Handled = true;
            
        }

        private void MoveBetweenControls(object sender, KeyEventArgs e)
        {
            //Переход между полями
            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Enter)
            {
                //Перемещение вниз
                if (e.Key == Key.Down || e.Key == Key.Enter)
                {
                    if (Controls.IndexOf((Control)sender) != (Controls.Count - 1)) Controls[Controls.IndexOf((Control)sender) + 1].Focus();
                    else if (e.Key == Key.Enter) SubmitAdd(null, null);
                }
                //Перемещение вверх
                if (e.Key == Key.Up)
                {
                    if (Controls.IndexOf((Control)sender) != (0)) Controls[Controls.IndexOf((Control)sender) - 1].Focus();
                }
                e.Handled = true;
            }
        }

        private void cbProjectName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cbProjectName.SelectedIndex != -1)
                projectName = cbProjectName.SelectedValue.ToString().Remove(0, 38);
        }

        private void cbCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCurrency.SelectedIndex == 1) tbRates.Text = "1";
        }

        private void cbReceiver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbReceiver.Text = "";
            if (cbReceiver.SelectedIndex != -1)
            {
                if (cbReceiver.SelectedValue.ToString() == "Новый")                
                    tbReceiver.Visibility = Visibility.Visible;
                
                else
                {
                    tbReceiver.Visibility = Visibility.Hidden;
                    tbReceiver.Text = cbReceiver.SelectedValue.ToString();
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbWhoReceived.Text = "";
            if (сbWhoReceived.SelectedIndex != -1)
            {
                if (сbWhoReceived.SelectedValue.ToString() == "Новый")                
                    tbWhoReceived.Visibility = Visibility.Visible;
                
                else
                {
                    tbWhoReceived.Visibility = Visibility.Hidden;
                    tbWhoReceived.Text = сbWhoReceived.SelectedValue.ToString();
                }
            }
        }
    }
}
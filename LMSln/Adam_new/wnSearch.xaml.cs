using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Adam_new
{
    /// <summary>
    /// Interaction logic for wnSearch.xaml
    /// </summary>
    public partial class wnSearch : Window
    {
        public string str;
        public List<string> lst = new List<string>();
        
        public wnSearch(int user_atr)
        {
            InitializeComponent();
            if (user_atr == 1) window_search.Height = 140;
        }

        #region Search       
           

        private void NewSearch(object sender, TextChangedEventArgs e) //несколько раз запускается
        {           
            if (((TextBox)sender).Text != "Проект" & ((TextBox)sender).Text != "На кого получено" & ((TextBox)sender).Text != "Получатель" & ((TextBox)sender).Text != "Банк" & ((TextBox)sender).Text != "")
            {
                MainWindow main = this.Owner as MainWindow;                
                addlist();
                str = DataWork.GetRequestForSearch(lst, ActiveUser.UserID);
                //main.Search(str, lst);
                if(ActiveUser.UserType ==1)
                    main.dgUser1.ItemsSource = DataWork.SearchFirstUser(str, lst);
                else
                main.dgUser2.ItemsSource = DataWork.SearchFirstUser(str, lst);
                //lst.Clear();
            }            
        }

        void addlist()
        {
            if (tbProjectName.Text != "Проект")
                //lst.Add(tbProjectName.Text);
                lst.Insert(0, tbProjectName.Text);

            else lst.Insert(0, "");// lst.Add("");
            if (tbCurrency.SelectedItem != null && tbCurrency.SelectedItem.ToString().Remove(0, 38) != "$€₴")
                lst.Insert(1,tbCurrency.SelectedItem.ToString().Remove(0, 38));
            else lst.Insert(1,"");

            if (tbMarks.SelectedItem != null && tbMarks.SelectedItem.ToString().Remove(0, 38) != "Признак")
            
                //string str1 = tbMarks.SelectedItem.ToString().Remove(0, 38);
                lst.Insert(2,tbMarks.SelectedItem.ToString().Remove(0, 38));
            
            else lst.Insert(2,"");
            if (tbReceiver.Text != "Получатель")
                lst.Insert(3,tbReceiver.Text);
            else lst.Insert(3,"");
            if (tbWhoReceived.Text != "На кого получено")
                lst.Insert(4,tbWhoReceived.Text);
            else lst.Insert(4,"");
            if (tbBank.Text != "Банк")
                lst.Insert(5,tbBank.Text);
            else lst.Insert(5,"");
        }
        
        #endregion

        #region Focus

        private void tbProjectName_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbProjectName.Text == "Проект") tbProjectName.Text = "";
        }

        private void tbProjectName_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbProjectName.Text == "") 
            {
                tbProjectName.Text = "Проект";
                MainWindow main = this.Owner as MainWindow;
                addlist();
                str = DataWork.GetRequestForSearch(lst, ActiveUser.UserID);
                //main.Search(str, lst);
                if (str == "SELECT * FROM Transactions WHERE ORDER BY id_of_transaction DESC")
                    main.RefreshTable(null, null);
                else
                {if(ActiveUser.UserType==1)
                    main.dgUser1.ItemsSource = DataWork.SearchFirstUser(str, lst);
                    else
                    main.dgUser2.ItemsSource = DataWork.SearchFirstUser(str, lst);
                }
            }
        }

        private void tbReceiver_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbReceiver.Text == "Получатель") tbReceiver.Text = "";
        }

        private void tbReceiver_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbReceiver.Text == "") 
            {
                tbReceiver.Text = "Получатель";
                MainWindow main = this.Owner as MainWindow;
                addlist();
                str = DataWork.GetRequestForSearch(lst, ActiveUser.UserID);
                //main.Search(str, lst);
                if (str == "SELECT * FROM Transactions WHERE ORDER BY id_of_transaction DESC")
                    main.RefreshTable(null, null);
                else
                {if(ActiveUser.UserType==1)
                    main.dgUser1.ItemsSource = DataWork.SearchFirstUser(str, lst);
                    else
                    main.dgUser2.ItemsSource = DataWork.SearchFirstUser(str, lst);
                }
            }
        }

        private void tbWhoReceived_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbWhoReceived.Text == "На кого получено") tbWhoReceived.Text = "";
        }

        private void tbWhoReceived_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbWhoReceived.Text == "")
            {
                tbWhoReceived.Text = "На кого получено";
                MainWindow main = this.Owner as MainWindow;
                addlist();
                str = DataWork.GetRequestForSearch(lst, ActiveUser.UserID);
                //main.Search(str, lst);
                if (str == "SELECT * FROM Transactions WHERE ORDER BY id_of_transaction DESC")
                    main.RefreshTable(null, null);
                else
                {
                    if (ActiveUser.UserType == 1)
                        main.dgUser1.ItemsSource = DataWork.SearchFirstUser(str, lst);
                    else
                        main.dgUser2.ItemsSource = DataWork.SearchFirstUser(str, lst);
                }
            }
        }

        private void tbBank_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbBank.Text == "Банк") tbBank.Text = "";           
        }

        private void tbBank_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tbBank.Text == "") 
            {
                tbBank.Text = "Банк";
                MainWindow main = this.Owner as MainWindow;
                addlist();
                str = DataWork.GetRequestForSearch(lst, ActiveUser.UserID);
                //main.Search(str, lst);
                if (str == "SELECT * FROM Transactions WHERE ORDER BY id_of_transaction DESC")
                    main.RefreshTable(null, null);
                else
                {
                    if (ActiveUser.UserType == 1)
                        main.dgUser1.ItemsSource = DataWork.SearchFirstUser(str, lst);
                    else
                        main.dgUser2.ItemsSource = DataWork.SearchFirstUser(str, lst);
                }
            }
        }

        #endregion

        private void Window_Activated(object sender, EventArgs e)
        {
            this.IsEnabled = true;
        }

        private void NewSearch_combobox(object sender, SelectionChangedEventArgs e)
        {
            
            if (((ComboBox)sender).Text != "")
            {
                MainWindow main = this.Owner as MainWindow;
                //if (gSearch.IsVisible == true)
                //main.Search(str, lst);
                addlist();
                str = DataWork.GetRequestForSearch(lst, ActiveUser.UserID);
                if (str == "SELECT * FROM Transactions WHERE ORDER BY id_of_transaction DESC")                
                    main.RefreshTable(null, null);
                else
                {
                    if (ActiveUser.UserType == 1)
                        main.dgUser1.ItemsSource = DataWork.SearchFirstUser(str, lst);
                    else
                        main.dgUser2.ItemsSource = DataWork.SearchFirstUser(str, lst);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;
            main.RefreshTable(null, null);
        }
    }
}

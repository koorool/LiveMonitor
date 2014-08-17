using System;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace Adam_new
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        //int count = 0;
        //bool atr_of_moving = false;
        //Point mouse_point = new Point();

        public UserLogin()
        {
            InitializeComponent();
            ActiveUser.wnUser = this;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)            
                Login(null, null);            
        }

        private void Login( object sender, RoutedEventArgs args )
        {
            
            //here login
            try
            {
                //DataWork._ConnectionString = ConfigurationManager.ConnectionStrings
                //["AdamDbCon"].ConnectionString;
                 DataWork._ConnectionString = "Data Source=10.111.0.101\\SQLEXPRESS;Initial Catalog=AdamDB;Persist Security Info=True;User ID=AdminNew;Password=ddaedeqq";
                int type = DataWork.GetUsersType(tbLogin.Text, tbPassword.Password);
                if (type == 0) throw new Exception("Логин/пароль не совпадают");
                if (type == -1) throw new Exception("Нет соединения. Запустите Kerio VPN Client и выполните подключение к серверу. Затем, попробуйте снова.");
                    if (!DataWork.If_checked(tbLogin.Text, tbPassword.Password))
                    {
                        ActiveUser.dt_log.Start();
                        DataWork.CheckIn(tbLogin.Text, tbPassword.Password);
                        ActiveUser.login = tbLogin.Text;
                        ActiveUser.password = tbPassword.Password;
                        ActiveUser.UserType = type;
                        
                        Window wn = new MainWindow(type);
                        wn.Show();
                        this.Close();
                    }
            }
            catch (Exception e)
            {
                Window wn = new wnError(e.Message, 1);
                wn.ShowDialog();
            }

        }

        private void Move( object sender, MouseButtonEventArgs e )
        {
            this.DragMove();        
        }        

        private void Window_Closed( object sender, EventArgs e )
        {
            ActiveUser.wnUser = null;
        }

        private void Window_Activated( object sender, EventArgs e )
        {
            if ( ActiveUser.wnError == null )            
                this.IsEnabled = true;
        }

        private void Window_MouseMove( object sender, MouseEventArgs e )
        {
            ActiveUser.tick_count = 0;
        }

        private void Activate( object sender, EventArgs e )
        {
            if ( ActiveUser.wnError == null )//убрать скобки, проверить, запускается несколько раз            
                this.IsEnabled = true;  
        }

       /* private void tbLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == "Логин") tbLogin.Text = "";
        }

        private void tbLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == "") tbLogin.Text = "Логин";
        }

        private void tbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Password == "") tbPassword.Password = "Пароль";
        }

        private void tbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Password == "Пароль") tbPassword.Password = "";
        } */  
    }
}

using System;
using System.Windows;
using System.Windows.Input;

namespace Adam_new
{
    /// <summary>
    /// Interaction logic for wnAdminSettings.xaml
    /// </summary>
    public partial class wnAdminSettings : Window
    {
        public wnAdminSettings()
        {
            InitializeComponent();
            ActiveUser.wnAdminSettings = this;
            dgUsers.ItemsSource = DataWork.FillUsersTable();         
        }

        private void AddUser( object sender, RoutedEventArgs e )
        {
            Window w = new wnAdminUsers();
            w.Show();
        }

        private void Cancel( object sender, RoutedEventArgs e )
        {
            this.Close();
        }

        private void DeleteUser( object sender, RoutedEventArgs e )
        {
            this.IsEnabled = false;
            Window w = new wnDeleteUser();
            w.Show();
        }

        private void ClearDB( object sender, RoutedEventArgs e )
        {
            if ( pbPass.Password == ActiveUser.password )
            {
                DataWork.DELETE_DB();
                gDeleteDB.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /*private void Window_Closed( object sender, EventArgs e )
        {

        }*/

        private void Window_MouseMove( object sender, MouseEventArgs e )
        {
            ActiveUser.tick_count = 0;
        }

        private void Window_Activated( object sender, EventArgs e )
        {
            dgUsers.ItemsSource = DataWork.FillUsersTable();
            if ( ActiveUser.wnAdminUsers == null )            
                this.IsEnabled = true;
            
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            gDeleteDB.Visibility = System.Windows.Visibility.Collapsed;
            pbPass.Password = "";
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            gDeleteDB.Visibility = System.Windows.Visibility.Visible;
        }

        private void Window_LayoutUpdated( object sender, EventArgs e )
        {
            if ( ActiveUser.wnAdminUsers == null )            
                this.IsEnabled = true;
            
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
        {

            ActiveUser.wnAdminSettings = null;
        }

       /* private void Close( object sender, RoutedEventArgs e )
        {

            ActiveUser.wnAdminSettings = null;
            this.Close();
        }

        private void Hide( object sender, RoutedEventArgs e )
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }*/
    }
}
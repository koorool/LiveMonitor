using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Adam_new
{
    /// <summary>
    /// Interaction logic for wnDeleteTransaction.xaml
    /// </summary>
    public partial class wnDeleteTransaction : Window
    {
        public wnDeleteTransaction()
        {
            InitializeComponent();
            ActiveUser.wnDeleteTransaction = this;
            Update();
        }

        void Update()
        {
            cbID.Items.Clear();
            List<ComboBoxItem> lst = DataWork.GetIdCollection(ActiveUser.UserID);
            foreach ( ComboBoxItem cbi in lst )
            {
                cbID.Items.Add( cbi );
            }
        }

        private void Delete( object sender, RoutedEventArgs e )
        {
            if ( cbID.SelectedIndex > -1 )
            {
                string str = cbID.SelectedItem.ToString().Remove( 0, 38 );
                DataWork.Delete( Convert.ToInt32( str ), "Transactions");
                Update();
                this.Close();
            }
            /*else
            {
                Window wn = new wnError("Не все поля были заполнены", this);
                wn.ShowDialog();
                //ActiveUser.wnError= wn;
                //this.IsEnabled=false;
                //ActiveUser.wnError = wn;
            }*/
        }

        private void Window_Closed( object sender, EventArgs e )
        {

            ActiveUser.wnDeleteTransaction = null;
        }

        private void Window_MouseMove( object sender, MouseEventArgs e )
        {

            ActiveUser.tick_count = 0;
        }

        private void Close_w( object sender, RoutedEventArgs e )
        {

            ActiveUser.wnDeleteTransaction = null;
            this.Close();
        }

        private void Close( object sender, RoutedEventArgs e )
        {
            ActiveUser.wnDeleteTransaction = null;
            this.Close();
        }

        private void Hide( object sender, RoutedEventArgs e )
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Activate( object sender, EventArgs e )
        {
            if ( ActiveUser.wnError == null )
            {
                this.IsEnabled = true;
            }
        }
    }
}

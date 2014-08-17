using System.Windows;

namespace Adam_new
{
    /// <summary>
    /// Interaction logic for wnError.xaml
    /// </summary>
    public partial class wnError : Window
    {
        
        public wnError( string err, Window win)
        {
            InitializeComponent();            
            tbErrorM.Text = err;
            
        }
        public wnError (string msg, int Type)
        {
            InitializeComponent();
            tbErrorM.Text = msg;
            switch (Type)
            {
                case 1:
                    this.Title = " LiveMonitor | Ошибка";
                    break;
                case 2:
                    this.Title = " LiveMonitor | Уведомление";
                    break;
            }

            
        }
        private void Close( object sender, RoutedEventArgs e )
        {
            ActiveUser.wnError = null;
            this.Close();
        }
        
    }
}

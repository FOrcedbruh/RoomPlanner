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

namespace RoomPlanner
{
    

    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text != "Matvey" || password.Password != "qwerty")
            {
                MessageBox.Show("Ошибка введенных данных");
            } else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            } 
        }
    }
}

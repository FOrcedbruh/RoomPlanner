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
    public partial class AddFurnitureWindow : Window
    {
        public string FurnitureName { get; private set; }
        public double FurnitureWidth { get; private set; }
        public double FurnitureHeight { get; private set; }

        public AddFurnitureWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка введённых данных
            if (string.IsNullOrWhiteSpace(FurnitureNameTextBox.Text) ||
                !double.TryParse(FurnitureWidthTextBox.Text, out double width) ||
                !double.TryParse(FurnitureHeightTextBox.Text, out double height))
            {
                MessageBox.Show("Пожалуйста, введите корректные значения.");
                return;
            }

            FurnitureName = FurnitureNameTextBox.Text;
            FurnitureWidth = width;
            FurnitureHeight = height;

            DialogResult = true; // Закрыть окно и вернуть результат
        }
    }
}

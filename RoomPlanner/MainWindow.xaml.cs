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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoomPlanner
{
    public partial class MainWindow : Window
    {
        private double roomWidth = 500;
        private double roomHeight = 400;

        public MainWindow()
        {
            InitializeComponent();
            UpdateRoomSize();
        }

        private void UpdateRoomSize_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(RoomWidthTextBox.Text, out double width) &&
                double.TryParse(RoomHeightTextBox.Text, out double height))
            {
                roomWidth = width;
                roomHeight = height;
                UpdateRoomSize();
            }
            else
            {
                MessageBox.Show("Введите корректные размеры комнаты.");
            }
        }

        private void UpdateRoomSize()
        {
            RoomCanvas.Width = roomWidth;
            RoomCanvas.Height = roomHeight;
        }

        
        private void AddFurniture_Click(object sender, RoutedEventArgs e)
        {
            var addFurnitureWindow = new AddFurnitureWindow();
            if (addFurnitureWindow.ShowDialog() == true)
            {
                AddFurniture(addFurnitureWindow.FurnitureName, addFurnitureWindow.FurnitureWidth, addFurnitureWindow.FurnitureHeight);
            }
        }

        private void AddFurniture(string name, double width, double height)
        {
            
            var rect = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = Brushes.Brown,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Tag = name
            };

            
            var textBlock = new TextBlock
            {
                Text = name,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            };

           
            Canvas.SetLeft(rect, 10);
            Canvas.SetTop(rect, 10);
            Canvas.SetLeft(textBlock, 10 + (width / 2) - (textBlock.ActualWidth / 2));
            Canvas.SetTop(textBlock, 10 + (height / 2) - (textBlock.ActualHeight / 2));

           
            rect.MouseLeftButtonDown += Furniture_MouseLeftButtonDown;
            rect.MouseMove += Furniture_MouseMove;
            rect.MouseLeftButtonUp += Furniture_MouseLeftButtonUp;

           
            rect.Tag = textBlock;

          
            RoomCanvas.Children.Add(rect);
            RoomCanvas.Children.Add(textBlock);
        }

        private bool isDragging = false;
        private Point clickPosition;

        private void Furniture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                isDragging = true;
                clickPosition = e.GetPosition(rectangle);
                rectangle.CaptureMouse();
            }
        }

        private void Furniture_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && sender is Rectangle rectangle)
            {
                var mousePos = e.GetPosition(RoomCanvas);
                double left = mousePos.X - clickPosition.X;
                double top = mousePos.Y - clickPosition.Y;

                
                Canvas.SetLeft(rectangle, left);
                Canvas.SetTop(rectangle, top);

               
                if (rectangle.Tag is TextBlock textBlock)
                {
                    Canvas.SetLeft(textBlock, left + (rectangle.Width / 2) - (textBlock.ActualWidth / 2));
                    Canvas.SetTop(textBlock, top + (rectangle.Height / 2) - (textBlock.ActualHeight / 2));
                }
            }
        }

        private void Furniture_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                isDragging = false;
                rectangle.ReleaseMouseCapture();
            }
        }

        private void ResetRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomCanvas.Children.Clear(); 
        }
    }
}

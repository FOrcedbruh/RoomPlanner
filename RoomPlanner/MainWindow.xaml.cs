using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace RoomPlanner
{
    public partial class MainWindow : Window
    {
        private double roomWidth = 500;
        private double roomHeight = 250;
        private double roomLength = 400;
        private const double gridSpacing = 100;

        public MainWindow()
        {
            InitializeComponent();
            UpdateRoomSize();
            DrawGrid();
        }

        private void UpdateRoomSize_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(RoomWidthTextBox.Text, out double width) &&
                double.TryParse(RoomLengthTextBox.Text, out double length))
            {
                roomWidth = width;
                roomLength = length;
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
            RoomCanvas.Height = roomLength;
            DrawGrid();
        }

        private void DrawGrid()
        {
            for (double x = 0; x <= roomWidth; x += gridSpacing)
            {
                Line verticalLine = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = roomLength,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                RoomCanvas.Children.Add(verticalLine);
            }
            for (double y = 0; y <= roomLength; y += gridSpacing)
            {
                Line horizontalLine = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = roomWidth,
                    Y2 = y,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                RoomCanvas.Children.Add(horizontalLine);
            }

        }
        private void AddFurniture_Click(object sender, RoutedEventArgs e)
        {
            
            var addFurnitureWindow = new AddFurnitureWindow();
            
            if (addFurnitureWindow.ShowDialog() == true)
            {
                if (addFurnitureWindow.FurnitureZIndex > roomHeight)
                {
                    MessageBox.Show("Мебель не поместится по высоте");
                    return;
                }
                AddFurniture(addFurnitureWindow.FurnitureName, addFurnitureWindow.FurnitureWidth, addFurnitureWindow.FurnitureHeight);
            }
        }

        private void AddFurniture(string name, double width, double height)
        {
            var rect = new Rectangle
            {
                Width = width,
                Height = height,
                Tag = name
            };


            var textBlock = new TextBlock
            {
                Text = name,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            };

            var imageBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(string.Format("pack://application:,,,/Images/{0}.jpg", name), UriKind.RelativeOrAbsolute)),
                Stretch = Stretch.UniformToFill
            };


            Canvas.SetLeft(rect, 10);
            Canvas.SetTop(rect, 10);
            Canvas.SetLeft(textBlock, 10 + (width / 2) - (textBlock.ActualWidth / 2));
            Canvas.SetTop(textBlock, 10 + (height / 2) - (textBlock.ActualHeight / 2));


            rect.MouseLeftButtonDown += Furniture_MouseLeftButtonDown;
            rect.MouseMove += Furniture_MouseMove;
            rect.MouseLeftButtonUp += Furniture_MouseLeftButtonUp;


            rect.Tag = textBlock;
            rect.Fill = imageBrush;

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
            roomWidth = 500;
            roomLength = 400;
            RoomCanvas.Width = roomWidth;
            RoomCanvas.Height = roomLength;
            DrawGrid();
        }
    }
}

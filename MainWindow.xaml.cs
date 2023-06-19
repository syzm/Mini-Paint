using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
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

namespace WPFPOPRAWA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDrawing = false;
        private bool boardExists = false;
        private Shape chosenShape;
        private Point startPoint;

        public MainWindow()
        {
            InitializeComponent();

            SizeChanged += MainWindow_SizeChanged;

            Board.MouseLeftButtonDown += Board_MouseLeftButtonDown;
            Board.MouseLeftButtonUp += Board_MouseLeftButtonUp;
            Board.MouseMove += Board_MouseMove;
            Board.MouseRightButtonDown += Board_MouseRightButtonDown;

            ClearButton.Click += ClearButton_Click;
            RectangleButton.Click += RectangleButton_Click;
            EllipseButton.Click += EllipseButton_Click;
        }


        // https://stackoverflow.com/questions/39178929/draw-shapes-on-random-places-in-c-sharp
        // https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/shapes-and-basic-drawing-in-wpf-overview?view=netframeworkdesktop-4.8
        private void CreateRandomShapes()
        {
            Random rnd = new Random();

            for (int i = 0; i < 4; ++i)
            {
                Shape shape;
                if (rnd.NextInt64() % 2 == 0)
                {
                    shape = new Ellipse();
                }
                else
                {
                    shape = new Rectangle();
                }

                var color = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));
                shape.Fill = new SolidColorBrush(color);

                double maxWidth = Board.ActualWidth;
                double maxHeight = Board.ActualHeight;
                double width = rnd.Next(1, (int)maxWidth);
                double height = rnd.Next(1, (int)maxHeight);
                shape.Width = width;
                shape.Height = height;

                double left = rnd.NextDouble() * (maxWidth - width);
                double top = rnd.NextDouble() * (maxHeight - height);

                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);

                Board.Children.Add(shape);

            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!boardExists)
            {
                CreateRandomShapes();
                boardExists = true;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Board.Children.Clear();
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            chosenShape = new Rectangle();
            isDrawing = true;
            Board.Cursor = Cursors.Cross;
        }

        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            chosenShape = new Ellipse();
            isDrawing = true;
            Board.Cursor = Cursors.Cross;
        }

        private void Board_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                startPoint = e.GetPosition(Board);
                Random rnd = new Random();

                var color = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));
                chosenShape.Fill = new SolidColorBrush(color);

                Canvas.SetLeft(chosenShape, startPoint.X);
                Canvas.SetTop(chosenShape, startPoint.Y);

                Board.Children.Add(chosenShape);
            }
        }

        private void Board_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                Board.Children.Remove(chosenShape);
                isDrawing = false;
                Board.Cursor = Cursors.Arrow;
            }
        }

        private void Board_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                Board.Cursor = Cursors.Arrow;
            }
        }

        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(Board);

                chosenShape.Width = Math.Abs(currentPoint.X - startPoint.X);
                chosenShape.Height = Math.Abs(currentPoint.Y - startPoint.Y);

                Canvas.SetLeft(chosenShape, startPoint.X);
                Canvas.SetTop(chosenShape,  startPoint.Y);
            }
        }
    }
}

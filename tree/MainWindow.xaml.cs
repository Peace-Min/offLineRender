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

namespace tree
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Canvas canvas;
        Grid grid;
        public MainWindow()
        {
            InitializeComponent();

            // simple visual definition
            grid = new Grid { Width = 300, Height = 300 };
            var text = new TextBlock
            {
                Text = "Y DON'T I WORK???",
                FontSize = 100,
                FontWeight =
                             FontWeights.Bold
            };
            grid.Children.Add(text);

            // update the layout so everything is awesome cool
            //grid.Measure(grid.DesiredSize);
            //grid.Arrange(new Rect(grid.DesiredSize));

            grid.Measure(new Size(grid.Width, grid.Height));
            grid.Arrange(new Rect(new Size(grid.Width, grid.Height)));
            //grid.UpdateLayout();

            // create a BitmapSource from the visual
            var rtb = new RenderTargetBitmap(
                                        (int)grid.Width,
                                        (int)grid.Height,
                                        96,
                                        96,
                                        PixelFormats.Pbgra32);
            rtb.Render(grid);

            // Slap it in the window
            this.Content = new Image { Source = rtb, Width = 300, Height = 300 };
            //InitializeComponent();

            //canvas = new Canvas();
            //Content = canvas;

            //// Load and render data in the background
            LoadAndRenderChartData();
        }

        private async void LoadAndRenderChartData()
        {
            await Task.Run(() =>
            {
               
                // Dispatch the UI update after the background work is done
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // UI-related operations must be on the main/UI thread
                    CreateAndAddRectangle();
                    
                });

            });
            SaveCanvasAsBitmap("output.png");
        }

        private void CreateAndAddRectangle()
        {
            // This method is executed on the main/UI thread

            // Create the rectangle
            Rectangle rectangle = new Rectangle
            {
                Width = 100,
                Height = 50,
                Fill = Brushes.Blue
            };
            
            // Add the rendered element to the UI
            grid.Children.Add(rectangle);

        }
        private void SaveCanvasAsBitmap(string fileName)
        {
            // This method is executed on the main/UI thread

            // Create a RenderTargetBitmap to render the canvas content
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)grid.Width,
                (int)grid.Height,
                96, // DPI
                96, // DPI
                PixelFormats.Pbgra32);

            // Render the canvas content to the RenderTargetBitmap
            renderTargetBitmap.Render(grid);

            // Create a BitmapEncoder and save the RenderTargetBitmap to a file
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = System.IO.File.Create(fileName))
            {
                pngEncoder.Save(stream);
            }
        }
    }
}

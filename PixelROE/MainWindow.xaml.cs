using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace PixelROE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PixelRoeWebServer _webServer;
        
        public PixelData[] Images { get; }

        public MainWindow()
        {
            InitializeComponent();

            Images = Directory.GetFiles("Assets\\Images", "*.gif")
                .Select((filename) => new PixelData(filename))
                .ToArray();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _webServer = new PixelRoeWebServer();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _webServer.Dispose();
            base.OnClosing(e);
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("http://localhost:4000/index")
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Clipboard.SetText("http://localhost:4000/index");
                MessageBox.Show("웹브라우저 실행에 실패하여 클립보드로 복사했습니다. 웹브라우저 주소표시줄에 붙여넣기하여 이동하세요.");
            }
        }

        private async void ToggleButton_CheckUnCheck(object sender, RoutedEventArgs e)
        {
            _webServer.Controller.ShowImageList.Clear();
            foreach (var pixelData in Images)
            {
                if (pixelData.IsChecked)
                    _webServer.Controller.ShowImageList.Add(pixelData.Path);
            }

            await _webServer.DoRefreshAsync();
        }
    }
}
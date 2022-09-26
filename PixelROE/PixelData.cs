using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixelROE;

public class PixelData
{
    public bool IsChecked { get; set; }
    public ImageSource Bitmap { get; set; }
    public string Path { get; }

    public PixelData(string path)
    {
        IsChecked = false;
        Bitmap = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path), UriKind.Absolute));
        Path = path;
    }
}
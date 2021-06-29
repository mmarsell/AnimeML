using System.Drawing.Imaging;
using System.Drawing;
using System;
using System.Runtime.InteropServices;
using System.Threading;

public class Screenshot
{
    const string filePath = "filePath";

    //https://stackoverflow.com/questions/1163761/capture-screenshot-of-active-window
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetDesktopWindow();

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

    public static Image CaptureDesktop()
    {
        return CaptureWindow(GetDesktopWindow());
    }

    public static Bitmap CaptureActiveWindow()
    {
        return CaptureWindow(GetForegroundWindow());
    }

    public static Bitmap CaptureWindow(IntPtr handle)
    {
        var rect = new Rect();
        GetWindowRect(handle, ref rect);
        var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        var result = new Bitmap(bounds.Width, bounds.Height);

        using (var graphics = Graphics.FromImage(result))
        {
            graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        }

        return result;
    }

    public Screenshot()
    {

    }
    public static void StartScreenCapture()
    {
        int count = 0;
        while (true)
        {
            var image = CaptureDesktop();
            string imgName = filePath + count + ".png";
            image.Save(imgName, ImageFormat.Png);
            Thread.Sleep(1000);
            count++;
        }

    }
    public static void Main()
    {
        StartScreenCapture();
    }
}

using Microsoft.Xna.Framework;

namespace MpDemo.Client;

public static class MainWindow
{
    private static ViewportInfo _info = new();

    public static ViewportInfo ViewportInfo => _info;
    public static int Width => _info.Width;
    public static int Height => _info.Height;

    public static void SetBounds(int width, int height)
    {
        _info.Width = width;
        _info.Height = height;
    }

    public static void SetBounds(Point bounds) => SetBounds(bounds.X, bounds.Y);
}

public class ViewportInfo
{
    public int Width;
    public int Height;
    public int Zoom;

    public Point VisibleArea => new (Width / Zoom, Height / Zoom);

    public Rectangle VisibleRectangle()
    {
        return new (0, 0, Width / Zoom, Height / Zoom);
    }
}

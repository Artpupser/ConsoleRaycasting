namespace ConsoleRaycasting.Components;
using ConsoleRaycasting.Main;

public class Screen
{
    public static Vector2Int scale { get; private set; }
    public static Vector2Int center { get; private set; }
    public const ConsoleColor foregroundColor = ConsoleColor.White;
    public const ConsoleColor backgroundColor = ConsoleColor.Black;
    public static void Init()
    {
        var height = Console.LargestWindowHeight;
        var width = Console.LargestWindowWidth;
        if (height % 2 != 0)
            height--;
        if (width % 2 != 0)
            width--;
        scale = new(width, height);
        center = new(scale.X / 2, scale.Y / 2);
        Console.SetWindowSize(scale.X, scale.Y);
        Console.SetBufferSize(scale.X, scale.Y);
    }
}
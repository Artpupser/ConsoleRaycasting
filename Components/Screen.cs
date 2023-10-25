namespace ConsoleRaycasting.Components;
using static ConsoleRaycasting.Lib.Tools;
public class Screen
{
    public static Vector2Int scale { get; private set; }
    public static Vector2Int center { get; private set; }
    public const ConsoleColor foregroundColor = ConsoleColor.White;
    public const ConsoleColor backgroundColor = ConsoleColor.Black;
    private static string clearBuffer = "";
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
        Console.SetWindowSize(scale.X - 1, scale.Y - 1);
        Console.SetBufferSize(scale.X, scale.Y);
        clearBuffer = new(' ', scale.X * scale.Y);
    }
    public static void Cls()
    {
        Cursor();
        Print(clearBuffer);
        Cursor();
    }
}
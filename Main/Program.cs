using System.Text;
using static ConsoleRaycasting.Main.Tools;

namespace ConsoleRaycasting.Main;

public class Program
{
    static void Main(string[] args) => new Program().Main();
    private void Main()
    {
        Screen.Init();
        Game game = new();
        game.Start();
    }
}
public class Game
{
    private string clearScreen = new string(' ', Screen.scale.X * Screen.scale.Y);
    private StringBuilder consoleScreen = new StringBuilder();
    private ConsoleKey currentKeyPressed { get; set; }
    private const byte targetFrameRate = 60;
    private int oneFrameTime = 1000 / 60;
    public void Start()
    {
        Thread thread = new(new ThreadStart(Controller));
        thread.Start();
        Update();
    }
    public async void Update()
    {
        while (true)
        {
            await Task.Delay(oneFrameTime);
            Draw();
        }
    }
    public void Draw()
    {
        Console.CursorVisible = false;
        consoleScreen.Clear();
        for (int y = -Screen.center.Y; y < Screen.center.Y; y++)
        {
            for (int x = -Screen.center.X; x < Screen.center.X; x++)
            {
                var x1 = (float)x / Screen.scale.X * 2;
                var y1 = (float)y / Screen.scale.Y * 2;
                var xy = x1 * x1 + y1 * y1;
                if (xy < 0.5f && xy > 0.4f)
                {
                    consoleScreen.Append('1');
                }
                else
                    consoleScreen.Append(' ');
            }
        }
        Cursor();
        Print(consoleScreen.ToString());
    }
    public void Clear()
    {
        Cursor();
        Print(clearScreen);
        Cursor();
    }
    private void Controller()
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                currentKeyPressed = Console.ReadKey(true).Key;
                continue;
            }
            currentKeyPressed = ConsoleKey.NoName;
        }

    }
}
public interface IGame
{
    public void Start();
    public void Update();
}
public class Math
{
    public static float GetRadian(float angle)
    {
        return angle * MathF.PI / 180;
    }
    public static float GetAngle(float radian)
    {
        return radian * 180 / MathF.PI;
    }
}
public class Tools
{
    public static void Print(string text) => Console.Write(text);
    public static void Print(char text) => Console.Write(text);
    public static void PrintL(string text) => Console.WriteLine(text);
    public static void Cursor(int x = 0, int y = 0) =>
        Console.SetCursorPosition(x, y);
}
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
public struct Vector2Int
{
    public int X;
    public int Y;

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }
}
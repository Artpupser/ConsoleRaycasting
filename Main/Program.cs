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
    private StringBuilder map = new();
    private ConsoleKey currentKeyPressed { get; set; }
    private const byte targetFrameRate = 60;
    private int oneFrameTime = 1000 / 60;
    public void Init()
    {
        consoleScreen.Append("11111111111111111111111");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1                1    1");
        consoleScreen.Append("1                1    1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1                1    1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1                1    1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1                     1");
        consoleScreen.Append("1                     1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("1          1          1");
        consoleScreen.Append("11111111111111111111111");
    }
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
    //Need create raycast 
    //create map
    //
    float rangeMin = 0;
    float rangeMax = 0;
    float x0 = -Screen.center.X;
    float y0 = 0;
    public void Draw()
    {
        Console.CursorVisible = false;
        consoleScreen.Clear();
        for (int y = -Screen.center.Y; y < Screen.center.Y; y++)
        {
            for (int x = -Screen.center.X; x < Screen.center.X; x++)
            {
                var x1 = x - MathF.Sin(x0) * 100;
                var y1 = y - y0;
                var formula = x1 * x1 + y1 * y1 * 3.8;
                if (formula < MathF.Abs(MathF.Cos(rangeMax)) * 1000 && formula > MathF.Abs(MathF.Cos(rangeMin)) * 500)
                {
                    consoleScreen.Append('1');
                }
                else
                    consoleScreen.Append(' ');
            }
        }
        Cursor();
        Print(consoleScreen.ToString());
        rangeMax += 0.01f;
        rangeMin += 0.01f;
        x0 += 0.01f;
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
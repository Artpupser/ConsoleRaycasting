using System.Collections.Generic;
using System.Numerics;
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
        game.Init();
        game.Start();
    }
}
public class Player
{
    public Vector2 Position;
    public float Rotate;
    public const float SpeedMove = 1;
    public const float SpeedRotate = .1f;
}
public class Game
{
    private string clearScreen = new string(' ', Screen.scale.X * Screen.scale.Y);
    private StringBuilder consoleScreen = new();
    //map stats >
    private StringBuilder map = new();
    private int widthMap = 20;
    private int heightMap = 20;
    //map stats <
    private Random rnd = new();
    private const byte targetFrameRate = 60;
    private int oneFrameTime = 1000 / targetFrameRate;
    private Player player = new();
    public void Init()
    {
        //198
        map.Append("####################");
        map.Append("#       #          #");
        map.Append("#       #          #");
        map.Append("#             #    #");
        map.Append("#             #    #");
        map.Append("#       #          #");
        map.Append("#       #          #");
        map.Append("#       #          #");
        map.Append("#             #    #");
        map.Append("#       #          #");
        map.Append("#       #          #");
        map.Append("#             #    #");
        map.Append("#       #          #");
        map.Append("#       #          #");
        map.Append("#       #          #");
        map.Append("#                  #");
        map.Append("#                  #");
        map.Append("#       #          #");
        map.Append("#       #          #");
        map.Append("####################");
        player.Rotate = 0;
        widthMap = 20;
        heightMap = 20;
        bool isCorrectSpawn = false;
        while (!isCorrectSpawn)
        {
            player.Position = new(rnd.Next(0, widthMap), rnd.Next(0, heightMap));
            isCorrectSpawn = map[(int)player.Position.Y * widthMap + (int)player.Position.X] == ' ';
        }
    }
    public void Start()
    {
        Thread thread = new(new ThreadStart(Controller));
        thread.Start();
        Update();
    }
    public void Update()
    {
        while (true)
        {
            Thread.Sleep(oneFrameTime);
            Draw();
        }
    }
    float rangeMin = 0;
    float rangeMax = 0;
    float x0 = -Screen.center.X;
    float y0 = 0;
    public void Draw()
    {
        Console.CursorVisible = false;
        consoleScreen.Clear();
        //draw game
        for (int y = -Screen.center.Y; y < Screen.center.Y; y++)
        for (int x = -Screen.center.X; x < Screen.center.X; x++)
        {
            var x1 = x - MathF.Sin(x0) * 100;
            var y1 = y - y0;
            var formula = x1 * x1 + y1 * y1 * 3.8;
            if (formula < MathF.Abs(MathF.Cos(rangeMax)) * 1000 && formula > MathF.Abs(MathF.Cos(rangeMin)) * 500)
            {
                consoleScreen.Append('x');
                continue;
            }
            consoleScreen.Append(' ');
        }
        //draw gui
        //stats
        var stats = $"PX = {player.Position.X} PY = {player.Position.Y} PA = {player.Rotate}";
        for (int i = 0; i < stats.Length; i++)
        {
            consoleScreen[i] = stats[i];
        }
        //map
        for (int y = 0; y < heightMap; y++)
        for (int x = 0; x < widthMap; x++)
        {
            consoleScreen[(y + 1) * Screen.scale.X + x] = map[y * widthMap + x];
            consoleScreen[(int)(player.Position.Y + 1) * Screen.scale.X + (int)player.Position.X] = 'p';
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
        void MoveFront()
        {
            player.Position.X += MathF.Cos(player.Rotate) * Player.SpeedMove;
            player.Position.Y += MathF.Sin(player.Rotate) * Player.SpeedMove;
        }
        void MoveDown()
        {
            player.Position.X -= MathF.Cos(player.Rotate) * Player.SpeedMove;
            player.Position.Y -= MathF.Sin(player.Rotate) * Player.SpeedMove;
        }
        void Rotate(sbyte right) => player.Rotate += right * Player.SpeedRotate;
        Dictionary<ConsoleKey, Action> keyToAction = new()
        {
            [ConsoleKey.W] = () => 
            {
                MoveFront();
                if (map[(int)player.Position.Y * widthMap + (int)player.Position.X] == '#')
                    MoveDown();
            },
            [ConsoleKey.S] = () => {
                MoveDown();
                if (map[(int)player.Position.Y * widthMap + (int)player.Position.X] == '#')
                    MoveFront();
            },
            [ConsoleKey.RightArrow] = () => { Rotate(1); },
            [ConsoleKey.LeftArrow] = () => { Rotate(-1); }
        };
        while (true)
            if (keyToAction.TryGetValue(Console.ReadKey(true).Key, out var action))
                action.Invoke();
    }
}
public interface IGame
{
    public void Start();
    public void Update();
}
public class Math
{
    public static float GetRadian(float angle) => angle * MathF.PI / 180;
    public static float GetAngle(float radian) => radian * 180 / MathF.PI;
}
public class Tools
{
    public static void Print(string text) => Console.Write(text);
    public static void Print(char text) => Console.Write(text);
    public static void PrintL(string text) => Console.WriteLine(text);
    public static void Cursor(int x = 0, int y = 0) => Console.SetCursorPosition(x, y);
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
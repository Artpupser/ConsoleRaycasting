using ConsoleRaycasting.Components;
using ConsoleRaycasting.Interfaces;
using ConsoleRaycasting.Main;
using System.Text;
using static ConsoleRaycasting.Main.Tools;

public class Game : IGame
{
    private Player player = new();
    private Map map = new();
    private Random rnd = new();
    private StringBuilder consoleScreen = new();
    private const byte targetFrameRate = 60;
    private int oneFrameTime = 1000 / targetFrameRate;
    public Game()
    {
        bool isCorrectSpawn = false;
        while (!isCorrectSpawn)
        {
            player.Position = new(rnd.Next(0, map.widthMap), rnd.Next(0, map.heightMap));
            isCorrectSpawn = map.content[(int)player.Position.Y * map.widthMap + (int)player.Position.X] == ' ';
        }
    }
    public void Start()
    {
        var controller = new Thread(new ThreadStart(Controller));
        controller.Start();
        Print($"{map.widthMap} {map.heightMap}");
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
        for (int y = 0; y < map.heightMap; y++)
            for (int x = 0; x < map.widthMap; x++)
            {
                consoleScreen[(y + 1) * Screen.scale.X + x] = map.content[y * map.widthMap + x];
                consoleScreen[(int)(player.Position.Y + 1) * Screen.scale.X + (int)player.Position.X] = 'p';
            }
        Cursor();
        Print(consoleScreen.ToString());
        rangeMax += 0.01f;
        rangeMin += 0.01f;
        x0 += 0.01f;
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
                if (map.content[(int)player.Position.Y * map.widthMap + (int)player.Position.X] == '#')
                    MoveDown();
            },
            [ConsoleKey.S] = () => {
                MoveDown();
                if (map.content[(int)player.Position.Y * map.widthMap + (int)player.Position.X] == '#')
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
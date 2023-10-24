using ConsoleRaycasting.Components;
using ConsoleRaycasting.Interfaces;
using ConsoleRaycasting.Main;
using System.Text;
using static ConsoleRaycasting.Main.Tools;

public class Game : IGame
{
    private Raycasting raycasting;
    private Player player;
    private Map map = new();
    private Random rnd = new();
    private StringBuilder consoleScreen = new();
    private const byte targetFrameRate = 60;
    private int oneFrameTime = 1000 / targetFrameRate;
    public Game()
    {
        player = new(new(0));
        raycasting = new(map, player);
        var isCorrectSpawn = false;
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
        //Print($"{map.widthMap} {map.heightMap}");
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
    public void Draw()
    {
        Console.CursorVisible = false;
        consoleScreen.Clear();
        consoleScreen.Append(new string(' ', Screen.scale.X * Screen.scale.Y));
        //get Rays

        List<Ray> rays = new();
        for (var x =  0; x < Screen.scale.X; x++)
            rays.Add(raycasting.GetRay(x));
        raycasting.DrawScreen(rays, consoleScreen);
        //draw gui
        //stats
        var stats = $"PX = {player.Position.X} PY = {player.Position.Y} PA = {player.Rotate}";
        for (int i = 0; i < stats.Length; i++)
            consoleScreen[i] = stats[i];
        //map
        for (int y = 0; y < map.heightMap; y++)
            for (int x = 0; x < map.widthMap; x++)
            {
                consoleScreen[(y + 1) * Screen.scale.X + x] = map.content[y * map.widthMap + x];
                consoleScreen[(int)(player.Position.Y + 1) * Screen.scale.X + (int)player.Position.X] = 'p';
            }
        Cursor();
        Print(consoleScreen.ToString());
    }
    private void Controller()
    {
        void MoveFront()
        {
            player.Position.X += MathF.Sin(player.Rotate) * Player.SpeedMove;
            player.Position.Y += MathF.Cos(player.Rotate) * Player.SpeedMove;
        }
        void MoveDown()
        {
            player.Position.X -= MathF.Sin(player.Rotate) * Player.SpeedMove;
            player.Position.Y -= MathF.Cos(player.Rotate) * Player.SpeedMove;
        }
        void Rotate(sbyte right) => player.Rotate += right * Player.SpeedRotate;
        Dictionary<ConsoleKey, Action> keyToAction = new()
        {
            [ConsoleKey.W] = () =>
            {
                MoveFront();
                if (map.content[(int)player.Position.Y * map.widthMap + (int)player.Position.X] == '.')
                    MoveDown();
            },
            [ConsoleKey.S] = () => {
                MoveDown();
                if (map.content[(int)player.Position.Y * map.widthMap + (int)player.Position.X] == '.')
                    MoveFront();
            },
            [ConsoleKey.RightArrow] = () => { Rotate(-1); },
            [ConsoleKey.LeftArrow] = () => { Rotate(1); }
        };
        while (true)
            if (keyToAction.TryGetValue(Console.ReadKey(true).Key, out var action))
                action.Invoke();
    }
}
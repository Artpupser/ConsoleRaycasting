using ConsoleRaycasting.Components;
using ConsoleRaycasting.Main;
using System.Text;
using static ConsoleRaycasting.Lib.Tools;

public class Game
{
    private Player player = new();
    private Map map = new();
    private Random rnd = new();
    private StringBuilder consoleScreen = new();
    private Raycasting raycasting;
    private const byte targetFrameRate = 60;
    private const string controllerHelper = "W -> Front, S -> Down, A -> LeftRotate, D -> RightRotate, Escape -> Exit";
    private int oneFrameTime = 1000 / targetFrameRate;
    private bool stopGame = false;
    public Game()
    {
        raycasting = new(map, player);
        var isCorrectSpawn = false;
        var atempt = map.scale.GetMultiplication();
        while (!isCorrectSpawn || atempt is 0)
        {
            player.Position = new(rnd.Next(0, map.scale.X), rnd.Next(0, map.scale.Y));
            isCorrectSpawn = map.content[(int)player.Position.Y * map.scale.Y + (int)player.Position.X] == ' ';
        }
        if (atempt is 0 && !isCorrectSpawn)
            return;
    }
    public void Start()
    {
        new Thread(new ThreadStart(Controller)).Start();
        Update();
    }
    public void Update()
    {
        while (!stopGame)
            WaitThread(oneFrameTime, Draw);
    }
    public async void Draw()
    {
        map.Update();
        consoleScreen.Clear();
        consoleScreen.Append(new string(' ', Screen.scale.X * Screen.scale.Y));
        var rays = new List<Ray>();
        await Task.Run(() => {
            for (var x = 0; x < Screen.scale.X; x++)
            rays.Add(raycasting.GetRay(x)); 
        });
        raycasting.DrawScreen(rays, consoleScreen);
        await Task.Run(() =>
        {
            var stats = $"PX = {player.Position.X} PY = {player.Position.Y} PA = {player.Rotate}";
            for (int i = 0; i < stats.Length; i++)
                consoleScreen[i] = stats[i];
            for (int i = 0; i < controllerHelper.Length; i++)
                consoleScreen[(Screen.scale.Y * Screen.scale.X) - Screen.scale.X + i] = controllerHelper[i];
        });
        for (int y = 0; y < map.scale.Y; y++)
            for (int x = 0; x < map.scale.X; x++)
            {
                consoleScreen[(y + 1) * Screen.scale.X + x] = map.content[y * map.scale.X + x];
                consoleScreen[(int)(player.Position.Y + 1) * Screen.scale.X + (int)player.Position.X] = 'P';
            }
        Cursor();
        Print(consoleScreen.ToString());
    }
    private void Controller()
    {
        void Move(float direction)
        {
            Cursor(false);
            player.Position.X += (MathF.Sin(player.Rotate) * Player.SpeedMove) * direction;
            player.Position.Y += (MathF.Cos(player.Rotate) * Player.SpeedMove) * direction;
            if (map.content[(int)player.Position.Y * map.scale.X + (int)player.Position.X] == '.')
                Move(-direction);
        }
        void Rotate(sbyte right) => player.Rotate += right * Player.SpeedRotate;
        Dictionary<ConsoleKey, Action> keyToAction = new()
        {
            [ConsoleKey.W] = () => { Move(1); },
            [ConsoleKey.S] = () => { Move(-1); },
            [ConsoleKey.D] = () => { Rotate(-1); },
            [ConsoleKey.A] = () => { Rotate(1); },
            [ConsoleKey.Escape] = () => { stopGame = true; }
        };
        while (!stopGame)
            if (keyToAction.TryGetValue(Console.ReadKey(true).Key, out var action))
                action.Invoke();
    }
}
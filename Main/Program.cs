namespace ConsoleRaycasting.Main;
using ConsoleRaycasting.Components;
using static ConsoleRaycasting.Lib.Tools;
public class Program
{
    static void Main(string[] args) => new Program().Main();
    private void Main()
    {
        Console.Title = "Artpupser application";
        Screen.Init();
        while (true)
        {
            if (Console.WindowWidth < Screen.scale.X || Console.WindowHeight < Screen.scale.Y)
            {
                WaitThread(500, () =>
                {
                    Cursor(false);
                    Screen.Cls();
                    Print("Please press to [Alt + Enter] for FullScreen mode\nElse game will not worked!\nПожалуйста, нажмите [Alt + Enter] для перехода в полноэкранный режим\nИначе игра не будет работать!");
                });
                continue;
            }
            break;
        }
        Game game = new();
        game.Start();
        Screen.Cls();
        GetKey();
    }
}

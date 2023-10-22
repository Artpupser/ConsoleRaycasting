namespace ConsoleRaycasting.Main;
using ConsoleRaycasting.Components;
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

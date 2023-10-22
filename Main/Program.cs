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
        game.Start();
    }
}

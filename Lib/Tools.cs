namespace ConsoleRaycasting.Lib;
public class Tools
{
    public static void Print(string text) => Console.Write(text);
    public static void Print(char text) => Console.Write(text);
    public static void PrintL(string text) => Console.WriteLine(text);
    public static void Cursor(int x = 0, int y = 0) => Console.SetCursorPosition(x, y);
    public static void Cursor(bool enable) => Console.CursorVisible = enable;
    public static void WaitThread(int miliseconds, Action action)
    {
        Thread.Sleep(miliseconds);
        action.Invoke();
    }
    public static ConsoleKey GetKey(bool hide = true) => Console.ReadKey(hide).Key;
}
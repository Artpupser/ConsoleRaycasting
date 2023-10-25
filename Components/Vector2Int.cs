namespace ConsoleRaycasting.Components;
public struct Vector2Int
{
    public int X;
    public int Y;
    public int GetMultiplication() => X * Y;
    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }
}
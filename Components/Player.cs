namespace ConsoleRaycasting.Components;
using System.Numerics;

public class Player
{
    public Vector2 Position;
    public float Rotate = 0f;
    public const float SpeedMove = 1;
    public const float SpeedRotate = .1f;
}
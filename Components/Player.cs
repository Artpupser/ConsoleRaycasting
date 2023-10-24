namespace ConsoleRaycasting.Components;
using System.Numerics;

public class Player
{
    public Vector2 Position;
    public float Rotate = 0f;
    public const float SpeedMove = .1f;
    public const float SpeedRotate = .1f;
    public PlayerData playerData {  get; private set; }

    public Player(PlayerData playerData)
    {
        this.playerData = playerData;
    }
}

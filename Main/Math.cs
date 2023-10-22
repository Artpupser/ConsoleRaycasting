namespace ConsoleRaycasting.Main;
public class Math
{
    public static float GetRadian(float angle) => angle * MathF.PI / 180;
    public static float GetAngle(float radian) => radian * 180 / MathF.PI;
}
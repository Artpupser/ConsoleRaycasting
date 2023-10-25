using System.Numerics;
using System.Text;
using ConsoleRaycasting.Components;

namespace ConsoleRaycasting.Main;

public class Raycasting
{
    private const float depth = 10;
    private const float rayStep = 0.1f;
    private const string gradient = "█▓▒░ ";
    private const string floorGradient = "▓;:,.";
    private float fixLengthGradient = (gradient.Length - 1) / depth;
    private float fixLengthGradientFloor = (floorGradient.Length - 1) / depth;
    private float FOV = MathF.PI / 3f;
    private Map map;
    private Player player;
    private Dictionary<char, TypeGameObject> typesGameObject = new()
    {
        [' '] = TypeGameObject.Empty,
        ['x'] = TypeGameObject.Empty,
        ['.'] = TypeGameObject.Wall,
    };
    public static Dictionary<TypeGameObject, GameObject> gameObjects = new()
    {
        [TypeGameObject.Empty] = new GameObject(false, TypeGameObject.Empty),
        [TypeGameObject.Wall] = new GameObject(true, TypeGameObject.Wall),
    };

    public Raycasting(Map map, Player player)
    {
        this.map = map;
        this.player = player;
    }

    public char GetGradient(float length) => gradient[(int)Math.Clamp(length * fixLengthGradient, 0, gradient.Length - 1)];
    public char GetGradientFloor(float length) => floorGradient[(int)Math.Clamp(length * fixLengthGradientFloor, 0, floorGradient.Length - 1)];
    public Ray GetRay(int x)
    {
        var rayObj = new Ray(0);
        var rayAngle = player.Rotate + FOV / 2 - x * FOV / Screen.scale.X;
        var rayVec = new Vector2(MathF.Sin(rayAngle), MathF.Cos(rayAngle));
        while (rayObj.Distance < depth)
        {
            rayObj.Distance += rayStep;
            var check = new Vector2Int((int)(player.Position.X + rayVec.X * rayObj.Distance), (int)(player.Position.Y + rayVec.Y * rayObj.Distance));
            var symbol = map.content[check.Y * map.scale.X + check.X];
            var typeGameObject = typesGameObject[symbol];
            var gameObject = gameObjects[typeGameObject];
            if (gameObject.StopRay)
            {
                rayObj.Stop = gameObject;
                return rayObj;
            }
            map.content[check.Y * map.scale.X + check.X] = 'x';
        }
        rayObj.Stop ??= gameObjects[TypeGameObject.Empty];
        return rayObj;
    }
    public void DrawScreen(List<Ray> rays, StringBuilder screen)
    {
        for (var x = 0; x < rays.Count; x++)
        {
            var ray = rays[x];
            var ceiling = (int)(Screen.scale.Y / 1.7 - Screen.scale.Y * FOV / ray.Distance);
            var floor = Screen.scale.Y - ceiling;
            var gradient = GetGradient(ray.Distance);
            for (var y = 0; y < Screen.scale.Y; y++)
            {
                if (y <= ceiling)
                {
                    screen[y * Screen.scale.X + x] = ' ';
                    continue;
                }
                if (y > ceiling && y <= floor)
                {
                    screen[y * Screen.scale.X + x] = gradient;
                    continue;
                }
                var b = (y - Screen.scale.Y / 2f) / (Screen.scale.Y / 2f);
                screen[y * Screen.scale.X + x] = GetGradientFloor(ray.Distance / b);
            }
        }
    }
}

using System.Numerics;
using System.Text;

namespace ConsoleRaycasting.Components;

public class Raycasting
{
    private const float depth = 10;
    private const float rayStep = 0.1f;
    private const string gradient = "█▓▒░ ";
    private float fixLengthGradient = (gradient.Length - 1) / depth;
    private float FOV = MathF.PI / 2.5f;
    private Map map;
    private Player player;
    private Dictionary<char, TypeGameObject> typesGameObject = new()
    {
        [' '] = TypeGameObject.Empty,
        ['.'] = TypeGameObject.Wall,
        ['0'] = TypeGameObject.Coin,
    };
    public static Dictionary<TypeGameObject, GameObject> gameObjects = new()
    {
        [TypeGameObject.Empty] = new GameObject(false, false, TypeGameObject.Empty),
        [TypeGameObject.Coin] = new GameObject(false, true, TypeGameObject.Coin),
        [TypeGameObject.Wall] = new GameObject(true, false, TypeGameObject.Wall),
    };

    public Raycasting(Map map, Player player)
    {
        this.map = map;
        this.player = player;
    }

    public char GetGradient(float length)
    {
        var clamped = (int)Math.Clamp(length * fixLengthGradient, 0, gradient.Length - 1);
        return gradient[clamped];
    }
    public Ray GetRay(int x)
    {
        var rayObj = new Ray(0, new());
        var rayAngle = player.Rotate + FOV / 2 - x * FOV / Screen.scale.X;
        var rayVec = new Vector2(MathF.Sin(rayAngle), MathF.Cos(rayAngle));
        while (rayObj.Distance < depth)
        {
            rayObj.Distance += rayStep;
            var check = new Vector2Int((int)(player.Position.X + rayVec.X * rayObj.Distance), (int)(player.Position.Y + rayVec.Y * rayObj.Distance));
            var symbol = map.content[check.Y * map.widthMap + check.X];
            var typeGameObject = typesGameObject[symbol];
            var gameObject = gameObjects[typeGameObject];
            if (gameObject.StopRay)
            {
                rayObj.Stop = gameObject;
                return rayObj;
            }
            if (gameObject.Solid)
                rayObj.Solids.Add(gameObject);
        }
        rayObj.Stop ??= gameObjects[TypeGameObject.Empty];
        return rayObj;
    }
    public void DrawScreen(List<Ray> rays, StringBuilder screen)
    {
        for (var x = 0; x < rays.Count; x++)
        {
            var ray = rays[x];
            var ceiling = (int)(Screen.scale.Y / 2f - Screen.scale.Y * FOV / ray.Distance);
            var floor = Screen.scale.Y - ceiling;
            var gradient = GetGradient(ray.Distance); //█▓▒░ 
            for (var y = 0; y < Screen.scale.Y; y++)
            {
                if (y <= ceiling)
                {
                    //sky
                    screen[y * Screen.scale.X + x] = ' ';
                }
                else if (y > ceiling && y <= floor)
                {
                    //wall
                    screen[y * Screen.scale.X + x] = gradient;
                }
                else
                {
                    //floor
                    screen[y * Screen.scale.X + x] = ':';
                }
            }
        }
    }
}

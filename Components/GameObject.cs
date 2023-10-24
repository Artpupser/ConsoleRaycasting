namespace ConsoleRaycasting.Components;
public class GameObject
{
    private bool stopRay;
    private bool solid;
    private TypeGameObject type;
    public GameObject(bool stopRay, bool solid, TypeGameObject type)
    {
        StopRay = stopRay;
        Solid = solid;
        Type = type;
    }

    public bool StopRay { get => stopRay; set => stopRay = value; }
    public bool Solid { get => solid; set => solid = value; }
    public TypeGameObject Type { get => type; set => type = value; }
}
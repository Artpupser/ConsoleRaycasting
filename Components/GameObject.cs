namespace ConsoleRaycasting.Components;
public class GameObject
{
    private bool stopRay;
    private TypeGameObject type;
    public GameObject(bool stopRay, TypeGameObject type)
    {
        StopRay = stopRay;
        Type = type;
    }

    public bool StopRay { get => stopRay; set => stopRay = value; }
    public TypeGameObject Type { get => type; set => type = value; }
}
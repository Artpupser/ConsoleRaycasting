using ConsoleRaycasting.Components;
#nullable disable
[Serializable]
public class Ray
{
    private float distance;
    private GameObject stop;

    public Ray(float distance)
    {
        Distance = distance;
    }

    public float Distance { get => distance; set => distance = value; }
    public GameObject Stop { get => stop; set => stop = value; }
}
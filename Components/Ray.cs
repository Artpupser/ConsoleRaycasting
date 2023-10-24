using ConsoleRaycasting.Components;

[Serializable]
public class Ray
{
    private float distance;
    private List<GameObject> solids;

    public Ray(float distance, List<GameObject> solids)
    {
        Distance = distance;
        Solids = solids;
    }

    public float Distance { get => distance; set => distance = value; }
    public List<GameObject> Solids { get => solids; set => solids = value; }
    public GameObject Stop { get; set; }
}
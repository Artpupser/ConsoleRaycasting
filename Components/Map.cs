namespace ConsoleRaycasting.Components;
using ConsoleRaycasting.Properties;
using System.Text;

public class Map
{
    public StringBuilder content { get; private set; }
    public string mapPrefab { get; private set; }
    public Vector2Int scale { get; private set; }

    public Map()
    {
        Random rnd = new();
        var noise = rnd.Next(1, 20);
        var listMap = Resources.ExampleMap.Split('\n').ToList();
        content = new();
        for (var i = 0; i < listMap.Count - 1; i++)
            listMap[i] = listMap[i].Trim();
        scale = new(listMap[0].Length, listMap.Count);
        listMap.ForEach(el => content.Append(el));
        for (var y = 0; y < scale.X; y++)
            //task
            for (var x = 0; x < scale.Y; x++)
                if (y > 0 && x < 19 && y < 19 && x > 0 && rnd.Next(0, 100) < noise)
                    content[y * scale.X + x] = '.';
        mapPrefab = content.ToString();
    }
    public void Update()
    {
        content.Clear();
        content.Append(mapPrefab);
    }
}
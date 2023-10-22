namespace ConsoleRaycasting.Components;
using ConsoleRaycasting.Properties;
using System.Text;

public class Map
{
    public StringBuilder content { get; private set; }
    public int widthMap { get; private set; }
    public int heightMap { get; private set; }

    public Map()
    {
        content = new();
        var listMap = Resources.Map.Split('\n').ToList();
        for (var i = 0; i < listMap.Count - 1; i++)
        {
            listMap[i] = listMap[i].Trim();
        }
        widthMap = listMap[0].Length;
        heightMap = listMap.Count;
        listMap.ForEach(el => content.Append(el));
    }
}
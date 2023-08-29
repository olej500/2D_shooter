using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voronoi : MonoBehaviour
{
    [Range(1,100)]
    public int seedsNumber;

    [Range(0, 20)]
    public int borderSize;

    public int size;
    public bool useRandomSeed;
    public int seed;
    public int candidateNum;
    public int doorSize;

    public enum DiagramType
    {
        Euclid,
        Mixed,
        Manhattan
    };

    public DiagramType diagramtype;

    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = VoronoiDiagram("euclid", borderSize);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Renderer>().material.mainTexture = VoronoiDiagram("euclid", borderSize);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Renderer>().material.mainTexture = VoronoiDiagram("mixed", borderSize);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Renderer>().material.mainTexture = VoronoiDiagram("manhattan", borderSize);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            useRandomSeed = !useRandomSeed;
        }
    }

    //private void OnValidate()
    //{
    //    if (diagramtype == DiagramType.Euclid)
    //        GetComponent<Renderer>().material.mainTexture = VoronoiDiagram("euclid", borderSize);
    //    if (diagramtype == DiagramType.Mixed)
    //        GetComponent<Renderer>().material.mainTexture = VoronoiDiagram("mixed", borderSize);
    //    if (diagramtype == DiagramType.Manhattan)
    //        GetComponent<Renderer>().material.mainTexture = VoronoiDiagram("manhattan", borderSize);
    //}

    Texture2D VoronoiDiagram(string type, int borderSize)
    {

        Vector2Int[] seeds = new Vector2Int[seedsNumber];
        List<Color> colors  = new List<Color>();

        if(!useRandomSeed)
        {
            Random.InitState(seed);
        }
        else
        {
            seed = Random.Range(0, int.MaxValue);
            Random.InitState(seed);
        }

        //sorting by color

        //for (int i = 0; i < seedsNumber; i++)
        //{
        //    seeds[i] = new Vector2(Random.Range(0, size), Random.Range(0, size));
        //}

        //seeds = SortArray(seeds);

        //for (int i = 0; i < seedsNumber; i++)
        //{
        //    colors[i] = GetNextColor(i);
        //}

        seeds = MitchellDistribution(seedsNumber, candidateNum, size);

        for (int i = 0; i < seedsNumber; i++)
        {
            //seeds[i] = new Vector2Int(Random.Range(0, size), Random.Range(0, size));
            //colors.Add(GetNextColor(i));
            colors.Add(GetRandomColor());
        }

        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        //Texture mainTexture = GetComponent<Renderer>().material.mainTexture;
        //Texture2D texture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
        //texture.filterMode = FilterMode.Point;
        //texture.wrapMode = TextureWrapMode.Repeat;

        //RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        //Graphics.Blit(mainTexture, renderTexture);

        //RenderTexture.active = renderTexture;
        //texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        //texture.Apply();

        for (int y = 0; y < texture.height; y++)
        {

            for (int x = 0; x < texture.width; x++) 
            {
                Vector2 pixPos = new Vector2(x, y);
                float distance = 0;
                int seedNumber = 0;
                float minDistance = float.MaxValue;

                if ((new Vector2(pixPos.x, pixPos.y) - new Vector2((size)/2, (size)/2)).magnitude < (size)/2) // circle
                {
                    for (int i = 0; i < seedsNumber; i++)
                    {
                        if (type == "euclid")
                            distance = (seeds[i] - pixPos).sqrMagnitude;
                        else if (type == "manhattan")
                            distance = (Mathf.Abs(seeds[i].x - pixPos.x) + Mathf.Abs(seeds[i].y - pixPos.y));
                        else
                            distance = Mathf.Abs(seeds[i].x - pixPos.x) + Mathf.Abs(seeds[i].y - pixPos.y) + (seeds[i] - pixPos).magnitude;
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            seedNumber = i;
                        }
                    }
                    texture.SetPixel(x, y, colors[seedNumber]);
                }
            }
        }

        //make borders

        List<Vector2Int> borders = new List<Vector2Int>();
        List<Vector2Int> doors = new List<Vector2Int>();

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (texture.GetPixel(x, y) != texture.GetPixel(x + borderSize, y) && colors.Contains(texture.GetPixel(x, y)) && colors.Contains(texture.GetPixel(x + borderSize, y)))// && x + borderSize < texture.width)
                {
                    if (!doors.Contains(new Vector2Int((seeds[colors.IndexOf(texture.GetPixel(x, y))].x + seeds[colors.IndexOf(texture.GetPixel(x + borderSize, y))].x) / 2,
                                                       (seeds[colors.IndexOf(texture.GetPixel(x, y))].y + seeds[colors.IndexOf(texture.GetPixel(x + borderSize, y))].y) / 2)))
                        doors.Add(new Vector2Int((seeds[colors.IndexOf(texture.GetPixel(x, y))].x + seeds[colors.IndexOf(texture.GetPixel(x + borderSize, y))].x) / 2,
                                                 (seeds[colors.IndexOf(texture.GetPixel(x, y))].y + seeds[colors.IndexOf(texture.GetPixel(x + borderSize, y))].y) / 2));
                    borders.Add(new Vector2Int(x, y));
                    borders.Add(new Vector2Int(x + borderSize, y));
                }
                if (texture.GetPixel(x, y) != texture.GetPixel(x, y + borderSize) && colors.Contains(texture.GetPixel(x, y)) && colors.Contains(texture.GetPixel(x, y + borderSize)))// && y + borderSize < texture.height)
                {
                    if (!doors.Contains(new Vector2Int((seeds[colors.IndexOf(texture.GetPixel(x, y))].x + seeds[colors.IndexOf(texture.GetPixel(x, y + borderSize))].x) / 2,
                                                       (seeds[colors.IndexOf(texture.GetPixel(x, y))].y + seeds[colors.IndexOf(texture.GetPixel(x, y + borderSize))].y) / 2)))
                        doors.Add(new Vector2Int((seeds[colors.IndexOf(texture.GetPixel(x, y))].x + seeds[colors.IndexOf(texture.GetPixel(x, y + borderSize))].x) / 2,
                                                 (seeds[colors.IndexOf(texture.GetPixel(x, y))].y + seeds[colors.IndexOf(texture.GetPixel(x, y + borderSize))].y) / 2));
                    borders.Add(new Vector2Int(x, y));
                    borders.Add(new Vector2Int(x, y + borderSize));
                }
            }
        }



        for (int i = 0; i < borders.Count; i++)
        {
            bool isWall = true; ;
            foreach (var door in doors)
            {
                if ((new Vector2(borders[i].x, borders[i].y) - door).magnitude < doorSize)
                    isWall = false;
            }
            if (isWall)
                texture.SetPixel(borders[i].x, borders[i].y, Color.white);
        }

        //foreach (var door in doors)
        //{
        //    Debug.Log(door);
        //    texture.SetPixel(door.x, door.y, Color.magenta);
        //}

        //setting block of pixels the same color

        //Color[] blackColor = new Color[9];
        //for(int i = 0; i < 9; i++)
        //{
        //    blackColor[i] = Color.black;
        //}

        //for (int i = 0; i < seedsNumber; i++)
        //{
        //    texture.SetPixels((int)seeds[i].x, (int)seeds[i].y, 3, 3, blackColor);
        //}

        //set seed color

        for (int i = 0; i < seedsNumber; i++)
        {
            texture.SetPixel(seeds[i].x, seeds[i].y, Color.black);
        }

        texture.Apply();

        return texture;
    }

    static Vector2Int[] MitchellDistribution(int seedNum, int candidateNum, int size)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        points.Add(new Vector2Int(Random.Range(0, size), Random.Range(0, size)));

        for (int i = 1; i < seedNum; i++)
        {
            List<Vector2Int> candidatePoints = new List<Vector2Int>();
            float bestDistance = 0;
            int candidate = 0;
            for (int j = 0; j < candidateNum; j++)
            {
                candidatePoints.Add(new Vector2Int(Random.Range(0, size), Random.Range(0, size)));
                float distance = float.MaxValue;
                foreach(var seed in points)
                {
                    if((seed - candidatePoints[j]).sqrMagnitude < distance)
                    {
                        distance = (seed - candidatePoints[j]).sqrMagnitude;
                    }
                }
                if(distance > bestDistance)
                {
                    bestDistance = distance;
                    candidate = j;
                }
            }
            points.Add(candidatePoints[candidate]);
        }

        Vector2Int[] finalPoints = new Vector2Int[seedNum];
        for (int i = 0; i < points.Count; i++)
        {
            finalPoints[i] = points[i];
        }

        return finalPoints;
    }

    Color GetRandomColor()
    {
        Color color = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);

        return color;
    }

    Color GetNextColor(int i)
    {
        Color color = Color.HSVToRGB(((float)i / (float)seedsNumber), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f));

        return color;
    }

    Vector2[] SortArray(Vector2[] array)
    {
        var itemMoved = false;
        do
        {
            itemMoved = false;
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i].x + array[i].y > array[i + 1].x + array[i + 1].y)
                {
                    var lowerValue = array[i + 1];
                    array[i + 1] = array[i];
                    array[i] = lowerValue;
                    itemMoved = true;
                }
            }
        } while (itemMoved);

        return array;
    }
}

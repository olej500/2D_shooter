using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class LevelGenerator : MonoBehaviour
{
    public int xSizeLevel;    //10
    public int ySizeLevel;    //4
    public int xMaxRoomSize;  //3
    public int ymaxRoomSize;  //4
    public int xRoomTiles;    //40
    public int yRoomTiles;    //20

    public BoundsInt backgroundBounds;
    public Tilemap foregroundTilemap;
    public Tilemap backgroundTilemap;
    public TileBase metalTile;
    public TileBase[] metalTileBackground;

    GameObject player;
    public GameObject entrance;
    public GameObject exit;
    Vector3 playerPosition;

    bool graphToScan;

    public GameObject[] rooms_1x1;
    public GameObject[] rooms_1x2;
    public GameObject[] rooms_1x3;
    public GameObject[] rooms_1x4;
    public GameObject[] rooms_2x1;
    public GameObject[] rooms_2x2;
    public GameObject[] rooms_2x3;
    public GameObject[] rooms_2x4;

    public struct Room
    {
        public int x;
        public int y;
        public int xSize;
        public int ySize;
        public GameObject room;
    }

    public List<Room> rooms;
    public List<Vector2Int> entrances;
    public List<GridGraph> graph;


    void Awake()
    {
        //player = GameObject.Find("Player");
        //rooms = new List<Room>();
        //entrances = new List<Vector2Int>();

        //GenerateLevel();

        //foreach (var room in rooms)
        //{
        //    Debug.Log("X: " + room.x + " Y: " + room.y + " XSIZE: " + room.xSize + " YSIZE: " + room.ySize);
        //}
    }

    public void Update()
    {
        /*if(!graphToScan)
        {
            var graph = AstarPath.active.data.gridGraph;
            AstarPath.active.Scan(graph);
            graphToScan = true;
        }*/
    }

    public void GenerateLevel()
    {
        player = GameObject.Find("Player");
        rooms = new List<Room>();
        entrances = new List<Vector2Int>();

        foregroundTilemap.ClearAllTiles();
        backgroundTilemap.ClearAllTiles();

        int roomCounter = 0;

        entrances.Clear();
        rooms.Clear();
        Room room = new Room();
        room.x = 0;
        //room.xSize = Random.Range(1, xMaxRoomSize + 1);
        //room.ySize = Random.Range(1, ymaxRoomSize + 1);
        //room.y = Random.Range(0, ySizeLevel - room.ySize);
        room.y = ySizeLevel / 2;
        room.xSize = 1;
        room.ySize = 1;
        int rand = Random.Range(0, rooms_1x1.Length);
        room.room = rooms_1x1[rand];

        int levelTop = room.y + room.ySize;
        int levelBottom = room.y;

        rooms.Add(room);
        playerPosition = new Vector3(3, room.y * yRoomTiles + 3, player.transform.position.z);

        while (true)
        {
            rooms.Add(GenerateRoom(roomCounter));
            roomCounter++;
            Room newRoom = rooms[roomCounter];
            if (newRoom.x + newRoom.xSize >= xSizeLevel)
            {
                newRoom.xSize = xSizeLevel - newRoom.x;
                rooms[roomCounter] = newRoom;
                break;
            }

            if (newRoom.y < levelBottom)
                levelBottom = newRoom.y;

            if (newRoom.y + newRoom.ySize > levelTop)
                levelTop = newRoom.y + newRoom.ySize;
        }

        GenerateTiles();

        player.transform.position = playerPosition;

        /*var graph = AstarPath.active.data.gridGraph;

        graph.center = new Vector3(200, (levelTop + levelBottom) / 2 * yRoomTiles, 0);
        graph.SetDimensions(100, (levelTop - levelBottom) * yRoomTiles / 4 , 4);

        AstarPath.active.Scan(graph);*/
    }

    Room GenerateRoom(int counter)
    {
        Room room = new Room();
        room.x = rooms[counter].x + rooms[counter].xSize;
        room.xSize = Random.Range(1, xMaxRoomSize + 1);
        room.ySize = Random.Range(1, ymaxRoomSize + 1);
        room.y = Random.Range(0, ySizeLevel - room.ySize);

        if (room.y + room.ySize - 1 < rooms[counter].y)
        {
            room.y = rooms[counter].y - room.ySize + 1;
        }
        else if(room.y > rooms[counter].y + rooms[counter].ySize - 1)
        {
            room.y = rooms[counter].y + rooms[counter].ySize - 1;
        }

        if(room.x + room.xSize >= xSizeLevel)
        {
            room.xSize = xSizeLevel - room.x;
        }

        if (room.xSize == 1)
        {
            if (room.ySize == 1)
            {
                int rand = Random.Range(0, rooms_1x1.Length);
                room.room = rooms_1x1[rand];
            }
            else if (room.ySize == 2)
            {
                int rand = Random.Range(0, rooms_1x2.Length);
                room.room = rooms_1x2[rand];
            }
            else if (room.ySize == 3)
            {
                int rand = Random.Range(0, rooms_1x3.Length);
                room.room = rooms_1x3[rand];
            }
            else if (room.ySize == 4)
            {
                int rand = Random.Range(0, rooms_1x4.Length);
                room.room = rooms_1x4[rand];
            }
        }
        else
        {
            if (room.ySize == 1)
            {
                int rand = Random.Range(0, rooms_2x1.Length);
                room.room = rooms_2x1[rand];
            }
            else if (room.ySize == 2)
            {
                int rand = Random.Range(0, rooms_2x2.Length);
                room.room = rooms_2x2[rand];
            }
            else if (room.ySize == 3)
            {
                int rand = Random.Range(0, rooms_2x3.Length);
                room.room = rooms_2x3[rand];
            }
            else if (room.ySize == 4)
            {
                int rand = Random.Range(0, rooms_2x4.Length);
                room.room = rooms_2x4[rand];
            }
        }

        return room;
    }

    void GenerateTiles()
    {
        //--TILES BEFORE THE 1ST ROOM--//

        for (int i = 0; i <= xRoomTiles * 2; i++)
        {
            for (int j = -yRoomTiles * 5 + rooms[0].y * yRoomTiles; j < yRoomTiles * 5 + rooms[0].y * yRoomTiles; j++)
            {
                foregroundTilemap.SetTile(new Vector3Int(-i, j, 0), metalTile);
            }
        }

        //--ROOM TILES--//

        foreach (var room in rooms)
        {
            GameObject roomInstance = Instantiate(room.room, new Vector3(room.x * xRoomTiles, room.y * yRoomTiles, 0), Quaternion.identity);
            /*foreach (Transform child in room.room.transform)
            {
                if (child.CompareTag("TileSpawn"))
                {
                    Vector3Int positionInt = new Vector3Int((int)child.position.x, (int)child.position.y, 0);
                    foregroundTilemap.SetTile(new Vector3Int(room.x * xRoomTiles + positionInt.x, room.y * yRoomTiles + positionInt.y, 0), metalTile);
                }
            }*/

            for (int i = 0; i <= xRoomTiles * room.xSize; i++)
            {
                for (int j = 0; j < yRoomTiles * 4; j++)
                {
                    foregroundTilemap.SetTile(new Vector3Int(room.x * xRoomTiles + i, room.y * yRoomTiles - j, 0), metalTile);
                    foregroundTilemap.SetTile(new Vector3Int(room.x * xRoomTiles + i, room.y * yRoomTiles + room.ySize * yRoomTiles + j, 0), metalTile);
                }
            }
            for (int i = -yRoomTiles * 5 + 1; i < yRoomTiles * 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    foregroundTilemap.SetTile(new Vector3Int(room.x * xRoomTiles + room.xSize * xRoomTiles + j, room.y * yRoomTiles + room.ySize * yRoomTiles / 2 + i, 0), metalTile);
                }
            }

            //-------------------TILES INSIDE OF EACH ROOM -----------------------//

            List<Vector3> tileWorldLocations = new List<Vector3>();
            Tilemap tilemap = roomInstance.GetComponentInChildren<Tilemap>();

            //print(tilemap.transform.position);

            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    tilemap.SetTile(pos, null);
                    foregroundTilemap.SetTile(Vector3Int.FloorToInt(tilemap.CellToWorld(pos)), metalTile);
                }
            }
        }

        //--TILES AFTER THE LAST ROOM--//

        for (int i = xRoomTiles * xSizeLevel; i < xRoomTiles * xSizeLevel + xRoomTiles * 2; i++)
        {
            for (int j = -yRoomTiles * 4 + rooms[rooms.Count - 1].y * yRoomTiles + 1; j < yRoomTiles * 4 + rooms[rooms.Count - 1].y * yRoomTiles + rooms[rooms.Count - 1].ySize * yRoomTiles; j++)
            {
                foregroundTilemap.SetTile(new Vector3Int(i, j, 0), metalTile);
            }
        }

        //--DELETING TILES AT THE ENTRANCE AND EXIT--//

        entrances.Add(new Vector2Int(rooms[0].x, rooms[0].y * yRoomTiles + 1));
        entrances.Add(new Vector2Int((rooms[rooms.Count - 1].x + rooms[rooms.Count - 1].xSize) * xRoomTiles, rooms[rooms.Count - 1].y * yRoomTiles + 1));

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                foregroundTilemap.SetTile(new Vector3Int(entrances[0].x - i, entrances[0].y + j, 0), null);
                foregroundTilemap.SetTile(new Vector3Int(entrances[1].x + i, entrances[1].y + j, 0), null);

                if ((entrances[0].x - i) % 2 == 0)
                {
                    if ((entrances[0].y + j) % 2 == 0)
                        backgroundTilemap.SetTile(new Vector3Int(entrances[0].x - i, entrances[0].y + j, 0), metalTileBackground[0]);
                    else
                        backgroundTilemap.SetTile(new Vector3Int(entrances[0].x - i, entrances[0].y + j, 0), metalTileBackground[2]);
                }
                else
                {
                    if ((entrances[0].y + j) % 2 == 0)
                        backgroundTilemap.SetTile(new Vector3Int(entrances[0].x - i, entrances[0].y + j, 0), metalTileBackground[1]);
                    else
                        backgroundTilemap.SetTile(new Vector3Int(entrances[0].x - i, entrances[0].y + j, 0), metalTileBackground[3]);
                }

                if ((entrances[1].x + i) % 2 == 0)
                {
                    if ((entrances[1].y + j) % 2 == 0)
                        backgroundTilemap.SetTile(new Vector3Int(entrances[1].x + i, entrances[1].y + j, 0), metalTileBackground[0]);
                    else
                        backgroundTilemap.SetTile(new Vector3Int(entrances[1].x + i, entrances[1].y + j, 0), metalTileBackground[2]);
                }
                else
                {
                    if ((entrances[1].y + j) % 2 == 0)
                        backgroundTilemap.SetTile(new Vector3Int(entrances[1].x + i, entrances[1].y + j, 0), metalTileBackground[1]);
                    else
                        backgroundTilemap.SetTile(new Vector3Int(entrances[1].x + i, entrances[1].y + j, 0), metalTileBackground[3]);
                }
            }
        }
         
        //--DOORS BETWEEN THE ROOMS--//

        for (int i = 1; i < rooms.Count; i++)
        {
            if (rooms[i].y > rooms[i - 1].y)
                entrances.Add(new Vector2Int(rooms[i].x * xRoomTiles, rooms[i].y * yRoomTiles + 1));
            else
                entrances.Add(new Vector2Int(rooms[i - 1].x * xRoomTiles + rooms[i - 1].xSize * xRoomTiles, rooms[i - 1].y * yRoomTiles + 1));
        }

        foreach (var entrance in entrances)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    foregroundTilemap.SetTile(new Vector3Int(entrance.x + i - 1, entrance.y + j, 0), null);
                }
            }
        }

        //--BACKGROUND--//

        foreach (var room in rooms)
        {
            for (int i = room.x * xRoomTiles; i < xRoomTiles * (room.x + room.xSize); i++)
            {
                for (int j = room.y * yRoomTiles; j < yRoomTiles * (room.y + room.ySize); j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                            backgroundTilemap.SetTile(new Vector3Int(i, j, 0), metalTileBackground[0]);
                        else
                            backgroundTilemap.SetTile(new Vector3Int(i, j, 0), metalTileBackground[2]);
                    }
                    else
                    {
                        if (j % 2 == 0)
                            backgroundTilemap.SetTile(new Vector3Int(i, j, 0), metalTileBackground[1]);
                        else
                            backgroundTilemap.SetTile(new Vector3Int(i, j, 0), metalTileBackground[3]);
                    }
                }
            }
        }

        //--ENTRANCE AND EXIT--//

        Instantiate(entrance, new Vector3(1, rooms[0].y * yRoomTiles + 1, 0), Quaternion.identity);
        Instantiate(exit, new Vector3(xSizeLevel * xRoomTiles, rooms[rooms.Count - 1].y * yRoomTiles + 1, 0), Quaternion.Euler(0, 180, 0));
        exit.GetComponent<ExitScript>().fadeFromBlackCheck = true;
    }
}

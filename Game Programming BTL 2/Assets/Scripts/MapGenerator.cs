using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Tilemap References")]
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private TileBase wallRuleTile;

    [Header("Map Data")]
    private readonly string[] levelMap = {
        "WWWWWWWWWWWWWWWWWW",
        "W.....WWWWW....WWW",
        "W.....WWWWW......W",
        "W..WW.......WW...W",
        "W..WW.......WW...W",
        "W..1..........2..W",
        "W................W",
        "W.......WWWW.....W",
        "WWWWWWWWWWWWWWWWWW"
    };

    public static MapGenerator Instance { get; private set; }

    public Vector2 SpawnPoint1 { get; private set; }
    public Vector2 SpawnPoint2 { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GenerateMap();
    }

    private void GenerateMap()
    {
        wallTilemap.ClearAllTiles();

        int rows = levelMap.Length;
        int cols = levelMap[0].Length;

        int startX = -cols / 2;
        int startY = rows / 2 - 1;

        bool[,] visited = new bool[rows, cols];

        // Step 1: Scan for spawn points and mark empty spaces as visited
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                char c = levelMap[y][x];

                if (c == '1')
                {
                    SpawnPoint1 = wallTilemap.GetCellCenterWorld(new Vector3Int(startX + x, startY - y, 0));
                    visited[y, x] = true;
                }
                else if (c == '2')
                {
                    SpawnPoint2 = wallTilemap.GetCellCenterWorld(new Vector3Int(startX + x, startY - y, 0));
                    visited[y, x] = true;
                }
                else if (c != 'W')
                {
                    visited[y, x] = true; // Ignore empty paths
                }
            }
        }

        // Step 2: Scan for W's and find the optimal chunks
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (!visited[y, x] && levelMap[y][x] == 'W')
                {
                    // Find the best possible 2x2 (or larger) chunk starting from here
                    FindBestChunk(x, y, cols, rows, visited, out int chunkWidth, out int chunkHeight);

                    if (chunkWidth >= 2 && chunkHeight >= 2)
                    {
                        // Mark the entire detected chunk as visited
                        for (int cy = y; cy < y + chunkHeight; cy++)
                        {
                            for (int cx = x; cx < x + chunkWidth; cx++)
                            {
                                visited[cy, cx] = true;
                            }
                        }

                        // Stamp the chunk all at once
                        PlaceWallChunk(startX + x, startY - y, chunkWidth, chunkHeight);
                    }
                }
            }
        }
    }

    private void FindBestChunk(int startX, int startY, int cols, int rows, bool[,] visited, out int bestWidth, out int bestHeight)
    {
        bestWidth = 1;
        bestHeight = 1;
        int maxArea = 1;

        // 1. Find the maximum possible width of the very first row
        int maxWidth = 0;
        for (int x = startX; x < cols; x++)
        {
            if (levelMap[startY][x] == 'W' && !visited[startY, x])
                maxWidth++;
            else
                break;
        }

        // 2. Shrink the width down to 2, testing how deep (height) each width can go
        for (int w = maxWidth; w >= 2; w--)
        {
            int h = 1;

            // Test the rows directly beneath our starting point
            for (int y = startY + 1; y < rows; y++)
            {
                bool isRowValid = true;
                for (int x = startX; x < startX + w; x++)
                {
                    if (levelMap[y][x] != 'W' || visited[y, x])
                    {
                        isRowValid = false;
                        break;
                    }
                }

                if (isRowValid)
                    h++;
                else
                    break; // Stop digging deeper for this specific width
            }

            // 3. If it meets the 2x2 minimum, check if it's the largest area we've found
            if (h >= 2)
            {
                int area = w * h;
                if (area > maxArea)
                {
                    maxArea = area;
                    bestWidth = w;
                    bestHeight = h;
                }
            }
        }
    }

    private void PlaceWallChunk(int unityStartX, int unityStartY, int width, int height)
    {
        int yMin = unityStartY - height + 1;
        BoundsInt bounds = new BoundsInt(unityStartX, yMin, 0, width, height, 1);

        TileBase[] chunkTiles = new TileBase[width * height];
        for (int i = 0; i < chunkTiles.Length; i++)
        {
            chunkTiles[i] = wallRuleTile;
        }

        wallTilemap.SetTilesBlock(bounds, chunkTiles);
    }
}
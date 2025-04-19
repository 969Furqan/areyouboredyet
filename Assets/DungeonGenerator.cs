using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Tile floorTile;
    public Tile topWallTile;
    public Tile bottomWallTile;
    public Tile leftWallTile;
    public Tile rightWallTile;
    public Tile topLeftCornerTile;
    public Tile topRightCornerTile;
    public Tile bottomLeftCornerTile;
    public Tile bottomRightCornerTile;
    public GameObject rockPrefab;
    public int width = 50;
    public int height = 30;
    public int numberOfRocks = 10;

    public GameObject pentagram;
    private GameObject pentagramInstance;

    private int liveEnemies = 0;
    private List<GameObject> enemies = new List<GameObject>(); 

    public GameObject angel;
    public GameObject ghost;
    public GameObject beetle;
    public GameObject giantBeetle;
    public GameObject evilWizard;
    public int totalEnemies = 3;
    private List<Vector3Int> occupiedPositions = new List<Vector3Int>();

    private bool useFirstSetOfEnemies = true;

    void Start()
    {
        GenerateLevel();
        PlaceRocks();
        SpawnEnemies();
        PlacePentagram();
    }

    void GenerateLevel()
    {
        // Clear existing tiles
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                floorTilemap.SetTile(pos, floorTile);

                if (x == 0 && y == 0)
                {
                    wallTilemap.SetTile(pos, bottomLeftCornerTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(-0.05f, -0.05f, 0)));
                }
                else if (x == 0 && y == height - 1)
                {
                    wallTilemap.SetTile(pos, topLeftCornerTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(-0.05f, 0.05f, 0)));
                }
                else if (x == width - 1 && y == 0)
                {
                    wallTilemap.SetTile(pos, bottomRightCornerTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(0.05f, -0.05f, 0)));
                }
                else if (x == width - 1 && y == height - 1)
                {
                    wallTilemap.SetTile(pos, topRightCornerTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(0.05f, 0.05f, 0)));
                }
                else if (y == 0)
                {
                    wallTilemap.SetTile(pos, bottomWallTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(0, -1.3f, 0)));
                }
                else if (y == height - 1)
                {
                    wallTilemap.SetTile(pos, topWallTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(0, 0.05f, 0)));
                }
                else if (x == 0)
                {
                    wallTilemap.SetTile(pos, leftWallTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(-1.3f, 0, 0)));
                }
                else if (x == width - 1)
                {
                    wallTilemap.SetTile(pos, rightWallTile);
                    wallTilemap.SetTransformMatrix(pos, Matrix4x4.Translate(new Vector3(1.3f, 0, 0)));
                }
            }
        }
    }

    void PlaceRocks()
    {
        occupiedPositions.Clear();
        int placedRocks = 0;
        while (placedRocks < numberOfRocks)
        {
            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);
            Vector3Int pos = new Vector3Int(x, y, 0);

            if (floorTilemap.GetTile(pos) == floorTile && !occupiedPositions.Contains(pos))
            {
                Vector3 worldPos = floorTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);
                Instantiate(rockPrefab, worldPos, Quaternion.identity);
                placedRocks++;
                occupiedPositions.Add(pos);
            }
        }
    }

    void SpawnEnemies()
    {
        int spawnedEnemies = 0;
        while (spawnedEnemies < totalEnemies)
        {
            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);
            Vector3Int pos = new Vector3Int(x, y, 0);

            if (floorTilemap.GetTile(pos) == floorTile && !occupiedPositions.Contains(pos))
            {
                GameObject enemyPrefab = useFirstSetOfEnemies ?
                    (Random.Range(0, 2) == 0 ? beetle : beetle) :
                    (Random.Range(0, 2) == 0 ? ghost : evilWizard);
                Vector3 worldPos = floorTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);
                GameObject enemy = Instantiate(enemyPrefab, worldPos, Quaternion.identity);
                var enemyMovement = enemy.GetComponent<BeetleMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.dungeonGenerator = this;
                }
                else
                {
                    var ghostMovement = enemy.GetComponent<ghostMovement>();
                    if (ghostMovement != null)
                    {
                        ghostMovement.dungeonGenerator = this;
                    }
                    else
                    {
                        var evilWizardMovement = enemy.GetComponent<evilWizardMovement>();
                        if (evilWizardMovement != null)
                        {
                            evilWizardMovement.dungeonGenerator = this;
                        }
                    }
                }
                spawnedEnemies++;
                liveEnemies++;
                occupiedPositions.Add(pos);
                enemies.Add(enemy); // Track spawned enemy
            }
        }
    }

    void PlacePentagram()
    {
        int attempts = 0;
        int maxAttempts = 100; 
        while (attempts < maxAttempts)
        {
            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);
            Vector3Int pos = new Vector3Int(x, y, 0);

            if (floorTilemap.GetTile(pos) == floorTile && !occupiedPositions.Contains(pos))
            {
                Vector3 worldPos = floorTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);
                pentagramInstance = Instantiate(pentagram, worldPos, Quaternion.identity);
                pentagramInstance.SetActive(false);
                break;
            }
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogError("Failed to place the pentagram after " + maxAttempts + " attempts.");
        }
    }


    public void EnemyDefeated()
    {
        totalEnemies--;
        Debug.Log("Enemies left: " + totalEnemies);
        if (totalEnemies <= 0 && pentagramInstance != null)
        {
            Debug.Log("Revealing pentagram");
            pentagramInstance.SetActive(true);
        }
    }
}

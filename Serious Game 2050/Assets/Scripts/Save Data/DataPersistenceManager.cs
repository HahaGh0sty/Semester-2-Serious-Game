using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public RandomTilemapGenerator mapgeneratorV2;
    public Tilemap tilemap;  // Assign in Inspector

    private string tilemapSavePath => Application.persistentDataPath + "/tilemapData.json";

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Data Persistence Manager instance found, fix it!");
        }
        instance = this;
    }

    public void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        Debug.Log(Application.persistentDataPath);
        LoadGame();
    }

    // Creates a new game and generates a new tilemap
    public void NewGame()
    {
        this.gameData = new GameData();
        mapgeneratorV2.GenerateMap();
        SaveTilemap();
    }

    // Saves game data and the tilemap
    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Save the game data
        dataHandler.Save(gameData);
        SaveTilemap();
    }

    // Loads game data and the tilemap
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        // If no data exists, start a new game
        if (this.gameData == null)
        {
            Debug.Log("No data found, starting a new game.");
            NewGame();
        }

        // Load other persistent data
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        // Load tilemap
        LoadTilemap();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    // Saves the tilemap's tiles and their positions
    private void SaveTilemap()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference is missing!");
            return;
        }

        TilemapSaveData saveData = new TilemapSaveData();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
            {
                saveData.tiles.Add(new TileData { position = position, tileName = tile.name });
            }
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(tilemapSavePath, json);
        Debug.Log("Tilemap saved to: " + tilemapSavePath);
    }

    // Loads the tilemap from saved data
    private void LoadTilemap()
    {
        if (!File.Exists(tilemapSavePath))
        {
            Debug.LogWarning("No tilemap save file found.");
            return;
        }

        string json = File.ReadAllText(tilemapSavePath);
        TilemapSaveData saveData = JsonUtility.FromJson<TilemapSaveData>(json);

        // Dictionary to map tile names to actual TileBase objects (you need to set this up in Unity)
        Dictionary<string, TileBase> tileLookup = mapgeneratorV2.GetTileLookup();

        tilemap.ClearAllTiles();
        foreach (TileData tileData in saveData.tiles)
        {
            if (tileLookup.TryGetValue(tileData.tileName, out TileBase tile))
            {
                tilemap.SetTile(tileData.position, tile);
            }
            else
            {
                Debug.LogWarning("Tile not found in lookup: " + tileData.tileName);
            }
        }

        Debug.Log("Tilemap loaded successfully.");
    }
}

// Data structure to store tile information
[System.Serializable]
public class TileData
{
    public Vector3Int position;
    public string tileName;
}

[System.Serializable]
public class TilemapSaveData
{
    public List<TileData> tiles = new List<TileData>();
}

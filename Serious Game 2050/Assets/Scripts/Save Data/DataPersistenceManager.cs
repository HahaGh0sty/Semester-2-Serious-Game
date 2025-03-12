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
        int randomSeed = Random.Range(0, 100000); // Generate random seed during runtime
        gameData = new GameData(randomSeed); // Pass it to GameData
        Debug.Log("Generated Map Seed: " + gameData.mapseed);
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
}

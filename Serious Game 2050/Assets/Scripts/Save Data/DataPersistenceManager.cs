using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public NewMapGenerator mapgeneratorV2;

    public int generatedValue;
    

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
        InvokeRepeating("SaveGame", 30f, 300f);
    }

    public void NewGame()
    {
        GenerateValueFromTime();
        gameData = new GameData(generatedValue); // Pass it to GameData
        Debug.Log("Generated Map Seed: " + gameData.generatedvalue);
    }

      public void GenerateValueFromTime()
    {
        DateTime now = DateTime.Now;

        generatedValue = now.Hour * 10000000 +
                         now.Minute * 100000 +
                         now.Second * 1000 +
                         now.Millisecond;
    }

    // Saves game data and updates building positions in the scene
    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        SaveBuildingPositions();
        dataHandler.Save(gameData);
        Debug.Log("Game saved");
    }

    // Loads game data and updates building positions in the scene
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data found, starting a new game.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        LoadBuildingPositions();
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

    private void SaveBuildingPositions()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        gameData.SaveBuildingPositions(buildings);
    }

    private void LoadBuildingPositions()
    {
        gameData.LoadBuildingPositions();
    }
}

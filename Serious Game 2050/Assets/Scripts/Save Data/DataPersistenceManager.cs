using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public NewMapGenerator mapgenerator;

    [Header("Building Prefabs")]
    public List<GameObject> buildingPrefabs; // Assign prefabs in Inspector

    private Dictionary<string, GameObject> prefabDictionary;

    [Header("Camera")]
    public Camera mainCamera;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Data Persistence Manager instance found, fix it!");
        }
        instance = this;

        // Build dictionary
        prefabDictionary = new Dictionary<string, GameObject>();
        foreach (var prefab in buildingPrefabs)
        {
            var id = prefab.GetComponent<BuildingIdentifier>();
            if (id != null)
            {
                prefabDictionary[id.prefabName] = prefab;
            }
            else
            {
                Debug.LogWarning($"Missing BuildingIdentifier on prefab: {prefab.name}");
            }
        }
    }

    public void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        Debug.Log(Application.persistentDataPath);
        LoadGame();
        InvokeRepeating("SaveGame", 0f, 300f);
    }

    public void NewGame()
    {
        this.gameData = new GameData(mapgenerator.generatedValue);
        mapgenerator.NewMapMake();
        SaveGame();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        SaveBuildingData();
        gameData.SaveCameraPosition(mainCamera);

        dataHandler.Save(gameData);
        Debug.Log("Game saved");
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data found, starting a new game.");
            NewGame();
        }
        else
        {
            mapgenerator.GenerateMap();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        LoadBuildingData();
        gameData.LoadCameraPosition(mainCamera);
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

    private void SaveBuildingData()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        gameData.SaveBuildingData(buildings);
    }

    private void LoadBuildingData()
    {
         // Ensure that the prefab dictionary is populated and we have saved building data
    if (gameData.savedBuildings != null)
    {
        foreach (BuildingData data in gameData.savedBuildings)
        {
            if (prefabDictionary.TryGetValue(data.prefabName, out GameObject prefab))
            {
                // Instantiate the building prefab at the saved position
                GameObject.Instantiate(prefab, data.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Prefab not found for: {data.prefabName}. Check the prefab dictionary.");
            }
        }
    }
    else
    {
        Debug.LogError("Saved building data is null. Please check the save process.");
    }
}
    }


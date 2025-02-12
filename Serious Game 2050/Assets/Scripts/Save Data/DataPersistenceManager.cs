using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
   public static DataPersistenceManager instance { get; private set;}

   private GameData gameData;
   private List<IDataPersistence> dataPersistenceObjects;

   private void awake()
   {
     if ( instance != null)
     {
        Debug.LogError("more then one Data Persistence Manager instance found, fix it");
     }
     instance = this;
   }
   public void start()
   {
      this.dataPersistenceObjects = FindAllDataPersistenceObjects();
      LoadGame();
   }

//makes new game data if u press new game
   public void NewGame()
   {
    this.gameData = new GameData();
   }
//saves current game data
   public void SaveGame()
   {
    // pass data to other scripts so they can use it

    //save the data using data handler
    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
         dataPersistenceObj.SaveData(ref gameData);
        }
   }
//loads existing game data
   public void LoadGame()
   {
    // MAKE THE DATA HANDLER PULL THE DATA AND USE IT HERE IAN U IDIOT
    
    // if no data exists... run new game instead
    if(this.gameData == null)
    {
        Debug.Log("no data therefore nothing to load, starting new game");
        NewGame();
        // make it send all loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
         dataPersistenceObj.LoadData(gameData);
        }
    }
   }

   private List<IDataPersistence> FindAllDataPersistenceObjects()
   {
      IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

      return new List<IDataPersistence>(dataPersistenceObjects);
   }
}

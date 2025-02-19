using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class FileDataHandler 
{
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        //uses path.combine to make sure this shit runs on different OS
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                //load data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //turn data from json file back into C# objects
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error when loading data, plz fix at: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //uses path.combine to make sure this shit runs on different OS
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // creates directory if one doesn't already exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // puts the data into a Json file to make it take up less space
            string DataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(DataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("error when saving data, plz fix at: " + fullPath + "\n" + e);
        }
    }
}

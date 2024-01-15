using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<Level> levels = new List<Level>();

    public Level GetTheLevelAtThisIndex(int index)
    {
        return levels[index];
    }

    public List<Level> GetAllLevelSaved() 
    { 
        return levels;
    }

    public void SetDataForGivenLevelIndex(int levelIndex, bool isPlayable, bool isCompleted)
    {
        levels[levelIndex].isPlayable = isPlayable;
        levels[levelIndex].isCompleted = isCompleted;
    }

    #region Save and Load
    public void SaveDataAsJSONFomat()
    {
        string content = JsonHelper.ToJsonFormat(levels.ToArray(), true);
        WriteJSONToFile(content);
    }

    public void LoadDataFromJSONFomat()
    {
        string content = ReadJsonFile();
        if (content != null)
        {
            List<Level> levelsData = new List<Level>(JsonHelper.FromJsonToWrapper<Level>(content).ToList());
            for (int i = 0; i < levelsData.Count; i++)
            {
                levels[i].SetLevel(levelsData[i]);
            }
        }
    }

    private void WriteJSONToFile(string content)
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Create);

        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.Write(content);
        }
    }

    private string ReadJsonFile()
    {
        if (File.Exists(Application.persistentDataPath + "/Levels.json"))
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Open);

            using (StreamReader reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }
        else
        {
            return null;
        }
    }
    #endregion
}

[System.Serializable]
public class Level
{
    public GameObject map;
    public float timeLimit;
    public bool isCompleted;
    public bool isPlayable;

    public void SetLevel(Level levelData)
    {
        this.timeLimit = levelData.timeLimit;
        this.isCompleted = levelData.isCompleted;
        this.isPlayable = levelData.isPlayable;
    }
}

public static class JsonHelper
{
    public static T[] FromJsonToWrapper<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJsonFomat<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJsonFormat<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}



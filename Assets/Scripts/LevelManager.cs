using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject
{
    public int shapeIndex;
    public int rotation;
    public int height;
    public bool isLast;
};

public class LevelData
{
    public int levelNumber;
    public int speed;
    public List<LevelObject> shapesData = new List<LevelObject>();
};

public class LevelManager
{
    Dictionary<string, LevelData> levelsData = new Dictionary<string, LevelData>();
    static LevelManager m_instance = null;

    public static LevelManager getInstance()
    {
        if (m_instance == null)
        {
            m_instance = new LevelManager();
            m_instance.readLevelsFromJSON();
        }

        return m_instance;
    }

    public void readLevelsFromJSON()
    {
        TextAsset asset = Resources.Load("LevelData") as TextAsset;
        string datafileContent = asset.text;
//        Debug.Log(datafileContent);
        JSONObject json = new JSONObject(datafileContent);

        int count = json.Count;

        for (int i = 1; i <= json.Count; i++)
        {
            LevelData levelData = new LevelData();
            JSONObject levelDataJson = json["level" + i];
            levelData.levelNumber = (int)levelDataJson["levelNum"].n;
            levelData.speed = (int)levelDataJson["speed"].n;
            List<JSONObject> shapes = levelDataJson["data"].list;

            for (int j = 0; j < shapes.Count; j++)
            {
                JSONObject shape = shapes[j];

                LevelObject data = new LevelObject();
                data.shapeIndex  = (int)shape["shapeIndex"].n;
                data.rotation    = (int)shape["rotation"].n;
                data.height      = (int)shape["height"].n;
                data.isLast      = shape["isLast"].b;

                levelData.shapesData.Add(data);
            }

            levelsData.Add("level" + i, levelData);
        }
    }

    public LevelData getDataForLevel(int levelNum)
    {
        string key = "level" + levelNum;
        LevelData data = levelsData[key];
        return data;
    }

}

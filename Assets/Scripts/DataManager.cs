using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string savePath;
    
    // Singleton
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.persistentDataPath + "/savefile.json";
        Load();
    }

    // Session Data
    public string username;
    [System.Serializable]
    public class SessionData
    {
        public string username;
        public int score;

        public SessionData(string username, int score)
        {
            this.username = username;
            this.score = score;
        }
    }

    public string bestScoreName;
    public int bestScore = 0;
    public List<SessionData> rankScores = new List<SessionData>();
    public List<SessionData> latestScores = new List<SessionData>();
    [System.Serializable]
    class SaveData
    {
        public string bestScoreName;
        public int bestScore;
        public SessionData[] rankScores;
        public SessionData[] latestScores;
    }

    public void Save(int score)
    {
        Debug.Log(score);
        // Update Instance data
        if (score >= bestScore)
        {
            bestScoreName = username;
            bestScore = score;
        }
        latestScores.Insert(0, new SessionData(username, score));
        latestScores = latestScores.Take(20).ToList();
        InsertInRank(new SessionData(username, score));

        // Prepare save data
        SaveData data = new SaveData();
        data.bestScoreName = bestScoreName;
        data.bestScore = bestScore;
        data.rankScores = rankScores.ToArray();
        data.latestScores = latestScores.ToArray();

        // Save file 
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            // Load file 
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Update Instance data
            bestScoreName = data.bestScoreName;
            bestScore = data.bestScore;
            rankScores = data.rankScores.ToList();
            latestScores = data.latestScores.ToList();
        }
    }

    private void InsertInRank(SessionData newScore)
    {
        int insertIndex = rankScores.FindIndex(entry => newScore.score > entry.score);
        if (insertIndex >= 0)
        {
            rankScores.Insert(insertIndex, newScore);
        }
        else
        {
            rankScores.Add(newScore);
        }
        if (rankScores.Count > 20)
        {
            rankScores = rankScores.Take(20).ToList();
        }
    }
}

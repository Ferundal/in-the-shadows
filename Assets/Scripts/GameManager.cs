using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameProgress gameProgress;

    private string _filePath;
    
    private static GameManager _instance = null;
    private ObjectController _b_CurrentObject;
    private bool _b_IsTestMode = false;
    public bool IsTestMode
    {
        get => _b_IsTestMode;
        set
        {
            LevelSelector levelSelector = FindObjectOfType<LevelSelector>();
            if (value == true)
            {
                levelSelector = FindObjectOfType<LevelSelector>();
                levelSelector.SetActiveLevels(levelSelector.LevelsAmount);
            }
            else
            {
                levelSelector.SetActiveLevels(gameProgress.unlockedLevels);
            }

            _b_IsTestMode = true;
        }
    }

    [Serializable]
    public class GameProgress
    {
        public int unlockedLevels = 1;
    }

    private void Awake()
    {
        _filePath = Application.dataPath + "/Resources/SaveFile.json";
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
            if (!File.Exists(_filePath))
            {
                gameProgress = new GameProgress();
                File.WriteAllBytes(_filePath, System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(gameProgress)));
            }
            else
            {
                gameProgress =
                    JsonUtility.FromJson<GameProgress>(
                        System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(_filePath)));
            }
        }
        else if (_instance.gameObject != gameObject)
        {
            Destroy(gameObject);
        }
    }


    public void LoadLevel(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void UnlockLevel(int sceneBuildIndex)
    {
        if (sceneBuildIndex == gameProgress.unlockedLevels)
        {
            ++gameProgress.unlockedLevels;
        }
        File.WriteAllBytes(_filePath, System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(gameProgress)));
    }
}

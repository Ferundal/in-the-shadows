using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levels;
    private GameManager _gameManager;

    private void Start()
    {
        foreach (GameObject level in _levels)
        {
            level.SetActive(false);
        }
        _gameManager = FindObjectOfType<GameManager>();
        SetActiveLevels(_gameManager.gameProgress.unlockedLevels);
    }

    public int LevelsAmount => _levels.Count;

    public void SetActiveLevels(int unlockedLevels)
    {
        StartCoroutine(ShowLevels(unlockedLevels));
    }
    private IEnumerator ShowLevels(int unlockedLevels)
    {
        foreach (GameObject level in _levels)
        {
            level.SetActive(false);
        }
        if (unlockedLevels > _levels.Count)
        {
            unlockedLevels = _levels.Count;
        }
        for (int counter = 0; counter < unlockedLevels; ++counter)
        {
            yield return new WaitForSeconds(.1f);
                _levels[counter].SetActive(true);
        }
    }

    public void LoadLevel(int sceneBuildIndex)
    {
        _gameManager.LoadLevel(sceneBuildIndex);
    }

    public void SetTestMode(bool isTestMode)
    {
        _gameManager.IsTestMode = isTestMode;
    }
}

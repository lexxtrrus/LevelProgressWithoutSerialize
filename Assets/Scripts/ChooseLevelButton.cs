using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChooseLevelButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Game game;
    [SerializeField] private Button button;
    [SerializeField] private GameObject locked_bg;
    [SerializeField] private List<GameObject> stars;
    [SerializeField] private GameObject selectedIcon;
    
    public delegate void LevelButtonPressing(LevelData levelData);
    public event LevelButtonPressing OnLevelButtonPressed;
    
    private LevelData _levelData;
    private int levelIndex;
    
    private void Awake()
    {
        game.OnLevelComplete += UpdateLevelButtonInfo;
    }

    private void OnDestroy()
    {
        game.OnLevelComplete -= UpdateLevelButtonInfo;
    }

    public void SetLevelButtonInfo(LevelData levelData)
    {
        levelIndex = levelData.Index;
        SetLevelStars(levelData.StarsAmount);
        SetLockedBG(levelData.IsLocked);
        _levelData = levelData;
    }

    private void UpdateLevelButtonInfo(LevelData levelData)
    {
        if (levelIndex == levelData.Index)
        {
            SetLevelButtonInfo(levelData);
        }
    }

    private void SetLevelStars(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            stars[i].SetActive(false);
            stars[i].SetActive(i < amount);
        }
    }

    public void SetLockedBG(bool isLocked)
    {
        if (isLocked)
        {
            button.interactable = false;
            locked_bg.SetActive(true);
        }
        else
        {
            button.interactable = true;
            locked_bg.SetActive(false);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        OnLevelButtonPressed?.Invoke(_levelData);
    }
}

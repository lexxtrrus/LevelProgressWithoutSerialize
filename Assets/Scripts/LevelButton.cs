using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Game game;
    [SerializeField] private Button button;
    [SerializeField] private GameObject locked_bg;
    [SerializeField] private GameObject locker_image;
    [SerializeField] private TextMeshProUGUI levelIndexText;
    [SerializeField] private List<GameObject> emptyStars;
    [SerializeField] private List<GameObject> fullStars;

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
        levelIndexText.text = levelData.Index.ToString();
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
            fullStars[i].SetActive(false);
            emptyStars[i].SetActive(i>= amount);
            fullStars[i].SetActive(i < amount);
        }
    }

    public void SetLockedBG(bool isLocked)
    {
        if (isLocked)
        {
            button.interactable = false;
            locked_bg.SetActive(true);
            locker_image.SetActive(true);
        }
        else
        {
            button.interactable = true;
            locked_bg.SetActive(false);
            locker_image.SetActive(false);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        OnLevelButtonPressed?.Invoke(_levelData);
    }
}

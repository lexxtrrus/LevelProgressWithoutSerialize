using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private List<LevelButton> _levelButtons;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private List<Canvas> canvases;
    [SerializeField] private List<LevelData> _levelDatas;
    [SerializeField] private GameObject[] resultStars;
    
    private int stars;
    private int _currentPage;
    private bool _isAwatingCallback = false;
    private bool _isLevelEnd = false;
    private Coroutine _coroutine;
    private LevelData _currentLevel;
    
    public delegate void LevelComplete(LevelData levelData);
    public event LevelComplete OnLevelComplete;
    
    private void Awake()
    {
        _currentPage = 0;
        previousPageButton.gameObject.SetActive(false);
        SubscribePageButtons();
        LoadLevelsData();
    }

    private void LoadLevelsData()
    {
        _levelDatas = new List<LevelData>();

        for (int i = 0; i < 24; i++)
        {
            var levelIndex = i + _levelButtons.Count * _currentPage;
            _levelDatas.Add(new LevelData(true, 0, levelIndex));

            if(i >= 8) continue;
            
            _levelButtons[i].OnLevelButtonPressed += LaunchLevel;
            _levelButtons[i].SetLevelButtonInfo(_levelDatas[levelIndex]);
        }
        
        _levelDatas[0].SetUnlocked(false);
        _levelButtons[0].SetLevelButtonInfo(_levelDatas[0]);
    }

    private void SubscribePageButtons()
    {
        nextPageButton.onClick.AddListener(OnNextPage);
        previousPageButton.onClick.AddListener(OnPreviousPage);
    }

    private void OnPreviousPage()
    {
        _currentPage--;
        if (_currentPage <= 0)
        {
            nextPageButton.gameObject.SetActive(true);
            previousPageButton.gameObject.SetActive(false);
        }

        if (_currentPage == 1)
        {
            previousPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);
        }

        SetLevelDataToButtons();
    }

    private void SetLevelDataToButtons()
    {
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            var levelIndex = i + _levelButtons.Count * _currentPage;
            _levelButtons[i].SetLevelButtonInfo(_levelDatas[levelIndex]);
        }
    }

    private void OnNextPage()
    {
        _currentPage++;
        if (_currentPage >= 2)
        {
            nextPageButton.gameObject.SetActive(false);
            previousPageButton.gameObject.SetActive(true);
        }
        
        if (_currentPage == 1)
        {
            previousPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);
        }

        SetLevelDataToButtons();
    }

    private void OnDestroy()
    {
        nextPageButton.onClick.RemoveListener(OnNextPage);
        previousPageButton.onClick.RemoveListener(OnPreviousPage);
        foreach (var levelButton in _levelButtons)
        {
            levelButton.OnLevelButtonPressed += LaunchLevel;
        }
    }

    public void LaunchLevel(LevelData levelData)
    {
        if (!levelData.IsLocked)
        {
            TurnOnCanvas(1);
            _currentLevel = levelData;
            _isAwatingCallback = true;
        }
    }

    private void TurnOffCanvases()
    {
        foreach (var canvas in canvases)
        {
            canvas.enabled = false;
        }
    }

    private void Update()
    {
        if (!_isAwatingCallback) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _isAwatingCallback = false;
            stars = 1;
            SetUpResultStars();
            TurnOnCanvas(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _isAwatingCallback = false;
            stars = 2;
            SetUpResultStars();
            TurnOnCanvas(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _isAwatingCallback = false;
            stars = 3;
            SetUpResultStars();
            TurnOnCanvas(2);
        }
    }

    public void CompleteLevel()
    {
        _isLevelEnd = false;
        _levelDatas[_currentLevel.Index].SetStars(stars);
        
        _levelDatas[_currentLevel.Index].SetStars(stars);
        OnLevelComplete?.Invoke(_levelDatas[_currentLevel.Index]);

        if(_currentLevel.Index >= _levelDatas.Count - 1) return;

        var nextLevelIndex = _currentLevel.Index + 1;
        _levelDatas[nextLevelIndex].SetUnlocked(false);
        _levelDatas[nextLevelIndex].SetStars(0);
        OnLevelComplete?.Invoke(_levelDatas[nextLevelIndex]);
        
        TurnOnCanvas(0);
    }

    private void TurnOnCanvas(int index)
    {
        TurnOffCanvases();
        canvases[index].enabled = true;
    }

    private void SetUpResultStars()
    {
        foreach (var star in resultStars)
        {
            star.SetActive(false);
        }

        for (int i = 0; i < stars; i++)
        {
            resultStars[i].SetActive(true);
        }
    }
}

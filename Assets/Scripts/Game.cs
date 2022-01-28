using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class Game : MonoBehaviour
{
    [SerializeField] private List<ChooseLevelButton> _levelButtons;
    [SerializeField] private List<Canvas> canvases;
    [SerializeField] private List<LevelData> _levelDatas;
    [SerializeField] private GameObject[] resultStars;
    [SerializeField] private LevelConfigPanel _levelConfigPanel;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Slider timer;

    private LevelData _currentLevel;
    private int stars;
    private int tapCount;
    public bool isGameLoop = false;
    private bool _isLevelEnd = false;
    private float duration = 0;
    private float endTime;
    public delegate void LevelComplete(LevelData levelData);
    public event LevelComplete OnLevelComplete;
    
    private void Awake()
    {
        TurnOnCanvas(0);
        LoadLevelsData();

        _levelConfigPanel.OnPlayPressed += StartGame;
        _levelConfigPanel.OnClosePressed += CloseLevelConfigPanel;
    }

    private void LoadLevelsData()
    {
        _levelDatas = new List<LevelData>();

        
        
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            _levelDatas.Add(CreateLevelCoinfig(i));
            _levelButtons[i].OnLevelButtonPressed += ShowLevelConfigPanel;
            _levelButtons[i].SetLevelButtonInfo(_levelDatas[i]);
        }
        
        _levelDatas[0].SetUnlocked(false);
        _levelButtons[0].SetLevelButtonInfo(_levelDatas[0]);
    }

    private LevelData CreateLevelCoinfig(int index)
    {
        var tapForPerfect = 10;
        
        return new LevelData(
            true,
            0,
            index,
            GetRandomValuesForConfig(),
            GetRandomValuesForConfig(),
            GetRandomValuesForConfig(),
            _sprites[UnityEngine.Random.Range(0, _sprites.Count)],
            tapForPerfect + index * 3);
    }

    private int GetRandomValuesForConfig()
    {
        return UnityEngine.Random.Range(10, 100);
    }

    private void SetLevelDataToButtons()
    {
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            _levelButtons[i].SetLevelButtonInfo(_levelDatas[i]);
        }
    }

    private void OnDestroy()
    {
        foreach (var levelButton in _levelButtons)
        {
            levelButton.OnLevelButtonPressed += ShowLevelConfigPanel;
        }
        
        _levelConfigPanel.OnPlayPressed -= StartGame;
        _levelConfigPanel.OnClosePressed -= CloseLevelConfigPanel;
    }

    public void ShowLevelConfigPanel(LevelData levelData)
    {
        if (!levelData.IsLocked)
        {
            _currentLevel = levelData;
            StartCoroutine(SelectLevelAnimation(levelData));
        }
    }

    private IEnumerator SelectLevelAnimation(LevelData levelData)
    {
        float startTime = 0f;
        RectTransform rect = _levelButtons[levelData.Index].GetComponent<RectTransform>();
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * 1.5f;

        while (startTime < 1f)
        {
            startTime += Time.deltaTime;
            rect.localScale = Vector3.Lerp(startScale, endScale, startTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        TurnOnCanvas(1);
        _levelConfigPanel.SetLevelConfig(_currentLevel);
    }

    private void StartGame()
    {
        tapCount = 0;
        isGameLoop = true;
        timer.value = 1;
        duration = 0f;
        endTime = Time.time + 3f;
        TurnOnCanvas(2);
        StartCoroutine(EndLevel(3f));
    }

    private IEnumerator EndLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        _levelButtons[_currentLevel.Index].GetComponent<RectTransform>().localScale = Vector3.one;
        isGameLoop = false;
        var starStep =_currentLevel.TapCountForPerfect / 3;

        for (int i = 1; i < 4; i++)
        {
            if (tapCount == 0 || tapCount < starStep)
            {
                LevelFailed();
                yield break;
            }

            if (tapCount > starStep * i)
            {
                stars = i;
            }
        }
        CompleteLevel();
    }

    private void LevelFailed()
    {
        _isLevelEnd = false;
        stars = 0;
        _levelDatas[_currentLevel.Index].SetStars(stars);
        OnLevelComplete?.Invoke(_levelDatas[_currentLevel.Index]);
        TurnOnCanvas(0);
    }
    private void CloseLevelConfigPanel()
    {
        _levelButtons[_currentLevel.Index].GetComponent<RectTransform>().localScale = Vector3.one;
        _currentLevel = null;
        TurnOnCanvas(0);
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
        if (!isGameLoop) return;

        if (Input.GetMouseButtonDown(0))
        {
            tapCount += 1;
        }

        duration += Time.deltaTime;
        timer.value = 1f - (duration / 3);
    }

    public void CompleteLevel()
    {
        _isLevelEnd = false;
        
        _levelDatas[_currentLevel.Index].SetStars(stars);
        OnLevelComplete?.Invoke(_levelDatas[_currentLevel.Index]);

        if (_currentLevel.Index >= _levelDatas.Count - 1)
        {
            TurnOnCanvas(0);
            return;
        }

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

    private void UpdateGameLoopInfo()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelConfigPanel : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Image icon;
    [SerializeField] private Slider mana;
    [SerializeField] private Slider coins;
    [SerializeField] private Slider lootboxes;
    [SerializeField] private TextMeshProUGUI label;

    private float manaAmount;
    private float coinsAmount;
    private float lootboxesAmount;

    public delegate void StartGame();

    public event StartGame OnPlayPressed;
    
    public delegate void ClosePanel();

    public event StartGame OnClosePressed;

    public void SetLevelConfig(LevelData levelData)
    {
        mana.value = levelData.Mana / 100f;
        coins.value = levelData.Coins / 100f;
        lootboxes.value = levelData.Lootboxes / 100f;
        label.text = $"LEVEL {levelData.Index + 1}";
        icon.sprite = levelData.LevelSprite;
    }

    public void OnPlayButtonPressed()
    {
        if(_game.isGameLoop) return;
        OnPlayPressed?.Invoke();
    }
    
    public void OnCloseButtonPressed()
    {
        if(_game.isGameLoop) return;
        OnClosePressed?.Invoke();
    }
    
    
}

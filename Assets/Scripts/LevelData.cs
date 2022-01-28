using UnityEngine;

[System.Serializable]
public class LevelData
{
    private bool _isLocked;
    private int _stars;
    private int _index;
    private int mana;
    private int coins;
    private int lootboxes;
    private Sprite levelSprite;
    private float tapCountForPerfect;

    public bool IsLocked => _isLocked;
    public int StarsAmount => _stars;
    public int Index => _index;
    public int Mana => mana;
    public int Coins => coins;
    public int Lootboxes => lootboxes;
    public Sprite LevelSprite => levelSprite;
    public float TapCountForPerfect => tapCountForPerfect;

    public LevelData(bool isLocked, int stars, int index, int mana, int coins, int lootboxes, Sprite levelSprite, float tapCountForPerfect)
    {
        _isLocked = isLocked;
        _stars = stars;
        _index = index;
        this.mana = mana;
        this.coins = coins;
        this.lootboxes = lootboxes;
        this.levelSprite = levelSprite;
        this.tapCountForPerfect = tapCountForPerfect;
    }

    public void SetUnlocked(bool value)
    {
        _isLocked = value;
    }

    public void SetStars(int amount)
    {
        if (amount > _stars)
        {
            _stars = amount;
        }
    }
}
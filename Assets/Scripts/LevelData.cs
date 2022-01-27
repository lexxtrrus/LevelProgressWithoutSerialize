[System.Serializable]
public class LevelData
{
    private bool _isLocked;
    private int _stars;
    private int _index;
    
    public bool IsLocked => _isLocked;
    public int StarsAmount => _stars;
    public int Index => _index;

    public LevelData(bool isLocked, int stars, int index)
    {
        _isLocked = isLocked;
        _stars = stars;
        _index = index;
    }

    public void SetUnlocked(bool value)
    {
        _isLocked = value;
    }

    public void SetStars(int amount)
    {
        _stars = amount;
    }
}
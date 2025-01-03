public class Score
{
    private readonly int _value;
    public int Value => _value;

    private Score(int value)
    {
        _value = value;
    }

    public Score Incremented()
    {
        return new Score(_value + 1);
    }

    public static Score Zero()
    {
        return new Score(0);
    }

    public bool IsWinningScore(int maxLamps)
    {
        return _value >= maxLamps;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}

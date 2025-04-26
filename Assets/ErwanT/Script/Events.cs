public static class Events
{
    public delegate void void_d_int(int value);

    public static void DoScoreLoaded(int score) => _scoreLoaded?.Invoke(score);
    public static event void_d_int _scoreLoaded;
}

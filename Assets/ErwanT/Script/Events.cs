public static class Events
{
    public delegate void void_d_int(int value);
    public delegate void void_d_bool(bool state);

    public static void DoScoreLoaded(int score) => _scoreLoaded?.Invoke(score);
    public static event void_d_int _scoreLoaded;

    public static void DoDanceState(bool state) => _danceState?.Invoke(state);
    public static event void_d_bool _danceState;
}

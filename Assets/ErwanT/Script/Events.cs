public static class Events
{
    public delegate void void_d_int(int value);
    public delegate void void_d_bool(bool state);
    public delegate void void_d_PowerType_int(PowerType type, int time);

    public static void DoScoreLoaded(int score) => _scoreLoaded?.Invoke(score);
    public static event void_d_int _scoreLoaded;

    public static void DoDanceState(bool state) => _danceState?.Invoke(state);
    public static event void_d_bool _danceState;

    public static void DoSetCooldown(PowerType type, int time) => _onSetCooldown?.Invoke(type, time);
    public static event void_d_PowerType_int _onSetCooldown;
}

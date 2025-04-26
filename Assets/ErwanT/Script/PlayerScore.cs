using System;
using System.Collections.Generic;

[Serializable]
public class PlayerScore
{
    public string _name;
    public int _score;

    public PlayerScore(string name, int score)
    {
        _name = name;
        _score = score;
    }
}

[Serializable]
public class Wrapper<T>
{
    public List<T> target;

    public Wrapper()
    {
        target = new List<T>();
    }

    public Wrapper(List<T> target)
    {
        this.target = target;
    }
}

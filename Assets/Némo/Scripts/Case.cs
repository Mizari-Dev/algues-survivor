using UnityEngine;

public enum Type
{
    Empty, Algae, Sun, Black
}

public class Case
{
    public Sprite _sprite;
    public Type _type;

    public Case(Sprite sprite, Type type)
    {
        _sprite = sprite;
        _type = type;
    }
}

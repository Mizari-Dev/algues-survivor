using UnityEngine;
using UnityEngine.Tilemaps;

public enum Type
{
    Empty, Algae1, Algae2, Sun, Black
}

public class Case
{
    public Tile tile;
    public Type type;

    public Case(Tile tile, Type type)
    {
        this.tile = tile;
        this.type = type;
    }
}

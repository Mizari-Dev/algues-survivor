using UnityEngine;
using UnityEngine.Tilemaps;

public enum Type
{
    Empty, BlueAlgae, YellowAlgae, Sun, Black, Coquillage, Fish, Fish2, Fish3, Fugu, Palourde, Couteau,   
}

public class Case
{
    public TileBase tile;
    public Type type;
    public Vector2Int position;

    public Case(TileBase tile, Type type, Vector2Int position)
    {
        this.tile = tile;
        this.type = type;
        this.position = position;   
    }

    public Case(Type type) { this.type = type; }
}

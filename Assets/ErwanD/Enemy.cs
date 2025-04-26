using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    public string name;
    public int width;
    public int height;
    public Tile tile;
    public Type type;
}

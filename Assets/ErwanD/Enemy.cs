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
    public bool isHighTide;

    public AudioReference _soundOnSpawn;
    public AudioReference _soundOnPlay;
    public RuntimeAnimatorController _runtimeAnimatorController;
    public Sprite image;
}

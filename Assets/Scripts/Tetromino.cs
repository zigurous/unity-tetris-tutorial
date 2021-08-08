using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tetris/Tetromino")]
public class Tetromino : ScriptableObject
{
    public Tile block;
    public Vector2Int[] cells = new Vector2Int[4];

}

using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I, J, L, O, S, T, Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tile tile;
    public Tetromino tetromino;

    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }
    public Vector2Int[,] rotations { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
        this.wallKicks = Data.WallKicks[this.tetromino];

        PrecomputeRotations();
    }

    private void PrecomputeRotations()
    {
        this.rotations = new Vector2Int[4, 4];

        // Set the initial rotation to the spawn state
        for (int i = 0; i < this.cells.Length; i++) {
            this.rotations[0, i] = this.cells[i];
        }

        // Start at index 1 since we already set the initial rotation
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j < this.cells.Length; j++)
            {
                // Get the cell data from the previous rotation
                Vector2 cell = this.rotations[i - 1, j];

                int x, y;

                // Calculate the x,y using a rotation matrix
                switch (this.tetromino)
                {
                    // I, O are rotated from an offset center point
                    case Tetromino.I:
                    case Tetromino.O:
                        cell.x -= 0.5f;
                        cell.y -= 0.5f;
                        x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0]) + (cell.y * Data.RotationMatrix[1]));
                        y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2]) + (cell.y * Data.RotationMatrix[3]));
                        break;

                    default:
                        x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0]) + (cell.y * Data.RotationMatrix[1]));
                        y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2]) + (cell.y * Data.RotationMatrix[3]));
                        break;
                }

                this.rotations[i, j] = new Vector2Int(x, y);
            }
        }
    }

}

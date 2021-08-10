using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.rotationIndex = 0;
        this.data = data;

        this.stepTime = Time.time + this.stepDelay;
        this.moveTime = Time.time + this.moveDelay;
        this.lockTime = 0f;

        UpdateCells();
    }

    private void UpdateCells()
    {
        if (this.cells == null) {
            this.cells = new Vector3Int[this.data.cells.Length];
        }

        for (int i = 0; i < this.cells.Length; i++) {
            this.cells[i] = (Vector3Int)this.data.rotations[this.rotationIndex, i];
        }
    }

    private void Update()
    {
        this.board.Clear(this);

        // We use a timer to allow the player to make adjustments to the piece
        // before it locks in place
        this.lockTime += Time.deltaTime;

        // Handle rotation
        if (Input.GetKeyDown(KeyCode.Q)) {
            Rotate(-1);
        } else if (Input.GetKeyDown(KeyCode.E)) {
            Rotate(1);
        }

        // Handle hard drop
        if (Input.GetKeyDown(KeyCode.Space)) {
            HardDrop();
        }

        // Allow the player to hold movement keys but only after a move delay
        // so it does not move too fast
        if (Time.time > this.moveTime) {
            HandleMoveInputs();
        }

        // Advance the piece to the next row every x seconds
        if (Time.time > this.stepTime) {
            Step();
        }

        this.board.Set(this);
    }

    private void HandleMoveInputs()
    {
        this.moveTime = Time.time + this.moveDelay;

        // Soft drop movement
        if (Input.GetKey(KeyCode.S)) {
            Move(Vector2Int.down);
        }

        // Left/right movement
        if (Input.GetKey(KeyCode.A)) {
            Move(Vector2Int.left);
        } else if (Input.GetKey(KeyCode.D)) {
            Move(Vector2Int.right);
        }
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;

        // Do not move down if the player is already holding down
        // otherwise it can cause a double movement
        if (!Input.GetKey(KeyCode.S)) {
            Move(Vector2Int.down);
        }

        // Once the piece has been inactive for too long it becomes locked
        if (this.lockTime >= this.lockDelay) {
            Lock();
        }
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down)) {
            continue;
        }

        Lock();
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0f; // reset
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        // Store the current rotation in case the rotation fails and we need to revert
        int originalRotation = this.rotationIndex;

        // Update the cell data to the new rotation
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);
        UpdateCells();

        // Revert the rotation if the wall kick tests fail
        if (!TestWallKicks(direction))
        {
            this.rotationIndex = originalRotation;
            UpdateCells();
        }
    }

    private bool TestWallKicks(int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationDirection);

        for (int i = 0; i < 5; i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation)) {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationDirection)
    {
        int wallKickIndex = this.rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }

}

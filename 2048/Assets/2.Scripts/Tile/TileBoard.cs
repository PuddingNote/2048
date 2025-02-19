using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;

    public Tile tilePrefab;                         // 생성한 타일 프리펩
    public TileState[] availableTileStates;         // 타일 상태 배열

    private TileGrid boardGrid;                     // 그리드
    private List<Tile> activeTiles;                 // 현재 존재하는 타일 목록

    private bool isMerging;                         // Merge 애니메이션 확인 bool

    private void Awake()
    {
        boardGrid = GetComponentInChildren<TileGrid>();
        activeTiles = new List<Tile>(16);
    }

    private void Update()
    {
        if (isMerging)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTiles(Vector2Int.down, 0, 1, boardGrid.gridHeight - 2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTiles(Vector2Int.right, boardGrid.gridWidth - 2, -1, 0, 1);
        }
        
    }

    // 보드 초기화
    public void ClearBoard()
    {
        foreach (var cell in boardGrid.gridCells)
        {
            cell.currentTile = null;
        }

        foreach (var tile in activeTiles)
        {
            Destroy(tile.gameObject);
        }

        activeTiles.Clear();
    }

    // 새로운 타일 생성
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, boardGrid.transform);
        tile.SetState(availableTileStates[0], 2);
        tile.SpawnTile(boardGrid.GetRandomEmptyCell());
        activeTiles.Add(tile);
    }

    // 모든 타일 이동
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool isChanged = false;

        for (int x = startX; x >= 0 && x < boardGrid.gridWidth; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < boardGrid.gridHeight; y += incrementY)
            {
                TileCell cell = boardGrid.GetCell(x, y);

                if (!cell.isEmpty)
                {
                    isChanged |= MoveTile(cell.currentTile, direction);
                }
            }
        }

        if (isChanged)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    // 단일 타일 이동
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = boardGrid.GetAdjacentCell(tile.currentCell, direction);

        while (adjacent != null)
        {
            if (!adjacent.isEmpty)
            {
                if (CanMerge(tile, adjacent.currentTile))
                {
                    MergeTile(tile, adjacent.currentTile);
                    return true;
                }
                break;
            }

            newCell = adjacent;
            adjacent = boardGrid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveToCell(newCell);
            return true;
        }

        return false;
    }

    // a,b 타일을 합칠 수 있는지 확인
    private bool CanMerge(Tile a, Tile b)
    {
        return a.tileNumber == b.tileNumber && !b.isMergeLocked;
    }

    private void MergeTile(Tile a, Tile b)
    {
        activeTiles.Remove(a);
        a.MergeWithCell(b.currentCell);

        int index = Mathf.Clamp(IndexOf(b.tileState) + 1, 0, availableTileStates.Length - 1);
        int number = b.tileNumber * 2;

        b.SetState(availableTileStates[index], number);

        gameManager.AddScore(number);
    }

    // 타일 상태의 인덱스 반환
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < availableTileStates.Length; i++)
        {
            if (state == availableTileStates[i])
            {
                return i;
            }
        }

        return -1;
    }

    // 변경 사항 대기
    private IEnumerator WaitForChanges()
    {
        isMerging = true;

        yield return new WaitForSeconds(0.1f);

        isMerging = false;

        foreach (var tile in activeTiles)
        {
            tile.isMergeLocked = false;
        }

        if (activeTiles.Count != boardGrid.totalCells)
        {
            CreateTile();
        }

        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if (activeTiles.Count != boardGrid.totalCells)
        {
            return false;
        }

        foreach (var tile in activeTiles)
        {
            TileCell up = boardGrid.GetAdjacentCell(tile.currentCell, Vector2Int.up);
            TileCell down = boardGrid.GetAdjacentCell(tile.currentCell, Vector2Int.down);
            TileCell left = boardGrid.GetAdjacentCell(tile.currentCell, Vector2Int.left);
            TileCell right = boardGrid.GetAdjacentCell(tile.currentCell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.currentTile))
            {
                return false;
            }

            if (down != null && CanMerge(tile, down.currentTile))
            {
                return false;
            }

            if (left != null && CanMerge(tile, left.currentTile))
            {
                return false;
            }

            if (right != null && CanMerge(tile, right.currentTile))
            {
                return false;
            }
        }

        return true;
    }

}

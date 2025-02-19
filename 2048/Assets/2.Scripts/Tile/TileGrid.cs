using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] gridRows { get; private set; }
    public TileCell[] gridCells { get; private set; }

    public int totalCells => gridCells.Length;              // 전체 셀 개수
    public int gridHeight => gridRows.Length;               // 그리드 높이
    public int gridWidth => totalCells / gridHeight;        // 그리드 너비

    private void Awake()
    {
        gridRows = GetComponentsInChildren<TileRow>();
        gridCells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        for (int y = 0; y < gridRows.Length; y++)
        {
            for (int x = 0; x < gridRows[y].rowCells.Length; x++)
            {
                gridRows[y].rowCells[x].gridPosition = new Vector2Int(x, y);
            }
        }
    }

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return gridRows[y].rowCells[x];
        }
        else
        {
            return null;
        }
    }

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.gridPosition;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, gridCells.Length);
        int startingIndex = index;

        while (!gridCells[index].isEmpty)
        {
            index++;

            if (index >= gridCells.Length)
            {
                index = 0;
            }

            if (index == startingIndex)
            {
                return null;
            }
        }

        return gridCells[index];
    }

}

using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] rowCells { get; private set; }        // 행의 모든 셀

    private void Awake()
    {
        rowCells = GetComponentsInChildren<TileCell>();
    }

}

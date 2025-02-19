using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] rowCells { get; private set; }        // ���� ��� ��

    private void Awake()
    {
        rowCells = GetComponentsInChildren<TileCell>();
    }

}

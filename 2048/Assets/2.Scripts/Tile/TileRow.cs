using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] cells { get; private set; }       // 행의 모든 셀

    private void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();
    }

}

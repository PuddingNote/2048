using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int coordinates { get; set; }     // ���� ��ǥ
    public Tile tile { get; set; }                  // ���� �ִ� Ÿ��

    public bool empty => tile == null;
    public bool occupied => tile != null;

}

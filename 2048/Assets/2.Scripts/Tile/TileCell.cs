using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int gridPosition { get; set; }    // ���� ��ǥ
    public Tile currentTile { get; set; }           // ���� �ִ� Ÿ��

    public bool isEmpty => currentTile == null;

}

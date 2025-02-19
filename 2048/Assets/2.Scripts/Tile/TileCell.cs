using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int gridPosition { get; set; }    // ¼¿ÀÇ ÁÂÇ¥
    public Tile currentTile { get; set; }           // ¼¿¿¡ ÀÖ´Â Å¸ÀÏ

    public bool isEmpty => currentTile == null;

}

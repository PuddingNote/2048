using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int coordinates { get; set; }     // ¼¿ÀÇ ÁÂÇ¥
    public Tile tile { get; set; }                  // ¼¿¿¡ ÀÖ´Â Å¸ÀÏ

    public bool empty => tile == null;
    public bool occupied => tile != null;

}

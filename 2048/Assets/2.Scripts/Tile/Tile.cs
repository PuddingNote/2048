using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public TileState tileState { get; private set; }    // 타일의 시각적 상태
    public TileCell currentCell { get; private set; }   // 타일이 위치한 셀
    public int tileNumber { get; private set; }         // 타일의 숫자
    public bool isMergeLocked { get; set; }             // 한번의 입력에 여러개의 merge가 생기지 않게 확인

    private Image backgroundImage;                      // 배경이미지
    private TextMeshProUGUI tileNumberText;             // 타일 숫자를 표시하는 텍스트

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        tileNumberText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // 타일 상태 설정
    public void SetState(TileState state, int number)
    {
        this.tileState = state;
        this.tileNumber = number;

        backgroundImage.color = state.backgroundColor;
        tileNumberText.color = state.textColor;
        tileNumberText.text = number.ToString();
    }

    // 타일 스폰
    public void SpawnTile(TileCell cell)
    {
        if (this.currentCell != null)
        {
            this.currentCell.currentTile = null;
        }

        this.currentCell = cell;
        this.currentCell.currentTile = this;

        transform.position = cell.transform.position;
    }

    // 타일 이동
    public void MoveToCell(TileCell cell)
    {
        if (this.currentCell != null)
        {
            this.currentCell.currentTile = null;
        }

        this.currentCell = cell;
        this.currentCell.currentTile = this;

        StartCoroutine(AnimateMovement(cell.transform.position, false));
    }

    public void MergeWithCell(TileCell cell)
    {
        if (this.currentCell != null)
        {
            this.currentCell.currentTile = null;
        }

        this.currentCell = null;
        cell.currentTile.isMergeLocked = true;

        StartCoroutine(AnimateMovement(cell.transform.position, true));
    }

    private IEnumerator AnimateMovement(Vector3 to, bool isMerging)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (isMerging)
        {
            Destroy(gameObject);
        }
    }

}

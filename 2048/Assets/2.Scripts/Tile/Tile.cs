using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public TileState tileState { get; private set; }    // Ÿ���� �ð��� ����
    public TileCell currentCell { get; private set; }   // Ÿ���� ��ġ�� ��
    public int tileNumber { get; private set; }         // Ÿ���� ����
    public bool isMergeLocked { get; set; }             // �ѹ��� �Է¿� �������� merge�� ������ �ʰ� Ȯ��

    private Image backgroundImage;                      // ����̹���
    private TextMeshProUGUI tileNumberText;             // Ÿ�� ���ڸ� ǥ���ϴ� �ؽ�Ʈ

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        tileNumberText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Ÿ�� ���� ����
    public void SetState(TileState state, int number)
    {
        this.tileState = state;
        this.tileNumber = number;

        backgroundImage.color = state.backgroundColor;
        tileNumberText.color = state.textColor;
        tileNumberText.text = number.ToString();
    }

    // Ÿ�� ����
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

    // Ÿ�� �̵�
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

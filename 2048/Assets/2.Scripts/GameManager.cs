using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("-----[UI]")]
    public TileBoard gameBoard;                 // ���� ����
    public CanvasGroup gameOverCanvasGroup;     // ���� ������ ���̴� CanvasGroup
    public TextMeshProUGUI scoreText;           // ���� ���� Text UI
    public TextMeshProUGUI highScoreText;       // �ְ� ���� Text UI
    public TextMeshProUGUI scorePopupPrefab;    // ���� �˾� UI ������

    private int currentScore;                   // ���� ����

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        highScoreText.text = LoadHighScore().ToString();

        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = false;

        gameBoard.ClearBoard();
        gameBoard.CreateTile();
        gameBoard.CreateTile();
        gameBoard.enabled = true;
    }

    public void GameOver()
    {
        gameBoard.enabled = false;
        gameOverCanvasGroup.interactable = true;

        StartCoroutine(Fade(gameOverCanvasGroup, 1f, 1f));
    }

    // ���ӿ��� ȭ�� ��ȯ ��� �ڷ�ƾ (Fade ȿ��)
    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void AddScore(int points)
    {
        ShowScorePopup(points);

        SetScore(currentScore + points);
    }

    private void ShowScorePopup(int points)
    {
        TextMeshProUGUI popup = Instantiate(scorePopupPrefab, scoreText.transform.parent);
        popup.text = $"+{points}";

        popup.rectTransform.position = scoreText.rectTransform.position;

        StartCoroutine(AnimateScorePopup(popup));
    }

    private IEnumerator AnimateScorePopup(TextMeshProUGUI popup)
    {
        float duration = 1f;
        float elapsed = 0f;

        Vector3 startPosition = popup.rectTransform.anchoredPosition;
        Vector3 endPosition = startPosition + new Vector3(0, 50, 0);

        Color startColor = popup.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            popup.rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t);
            popup.color = Color.Lerp(startColor, endColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(popup.gameObject);
    }

    private void SetScore(int currentScore)
    {
        this.currentScore = currentScore;
        scoreText.text = currentScore.ToString();

        SaveHighScore();
    }

    private void SaveHighScore()
    {
        int highScore = LoadHighScore();

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("highScore", currentScore);
        }
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }

    public void EndGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}

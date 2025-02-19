using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("-----[UI]")]
    public TileBoard gameBoard;                 // 게임 보드
    public CanvasGroup gameOverCanvasGroup;     // 게임 오버에 쓰이는 CanvasGroup
    public TextMeshProUGUI scoreText;           // 현재 점수 Text UI
    public TextMeshProUGUI highScoreText;       // 최고 점수 Text UI
    public TextMeshProUGUI scorePopupPrefab;    // 점수 팝업 UI 프리팹

    private int currentScore;                   // 현재 점수

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

    // 게임오버 화면 전환 기능 코루틴 (Fade 효과)
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

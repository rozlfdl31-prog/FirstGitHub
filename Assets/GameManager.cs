using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("게임 설정")]
    public int scoreToWin = 10; // 승리하기 위한 점수

    [Header("UI 컴포넌트")]
    public TextMeshProUGUI leftScoreText;  // 왼쪽 점수판
    public TextMeshProUGUI rightScoreText; // 오른쪽 점수판
    public GameObject gameOverPanel;
    public TextMeshProUGUI winMessageText;

    // [수정] 1인용 currentScore 대신 2인용 점수로 복구
    private int leftScore = 0;
    private int rightScore = 0;
    private bool isGameOver = false;

    void Start()
    {
        // [수정] 두 점수 모두 0으로 초기화
        leftScore = 0;
        rightScore = 0;
        isGameOver = false;
        UpdateScoreUI();

        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // [수정] 점수를 추가하는 함수 (어느 쪽 점수인지, 몇 점인지)
    public void AddScore(bool isForLeftPlayer, int pointsToAdd)
    {
        if (isGameOver) return; // 게임 끝나면 점수 안 오름

        // [수정] isForLeftPlayer 값에 따라 올바른 점수를 증가
        if (isForLeftPlayer)
        {
            leftScore += pointsToAdd;
        }
        else
        {
            rightScore += pointsToAdd;
        }

        UpdateScoreUI();

        // [수정] 점수 추가 후, 승리 조건 달성했는지 확인
        CheckForWin();
    }

    // [신규] 패배(Game Over)를 발동하는 함수 (Goal.cs가 호출)
    public void TriggerGameOver()
    {
        if (isGameOver) return;
        EndGame("GAME OVER"); // 패배 메시지
    }

    // [수정] 두 점수판 모두 업데이트
    void UpdateScoreUI()
    {
        if (leftScoreText != null)
            leftScoreText.text = leftScore.ToString();

        if (rightScoreText != null)
            rightScoreText.text = rightScore.ToString();
    }

    // [수정] 승리 조건 확인 (둘 중 하나라도 10점 넘으면)
    void CheckForWin()
    {
        if (leftScore >= scoreToWin)
        {
            EndGame("LEFT PLAYER WINS!");
        }
        else if (rightScore >= scoreToWin)
        {
            EndGame("RIGHT PLAYER WINS!");
        }
    }

    // [수정] 승리/패배 메시지를 모두 처리하는 공용 함수
    void EndGame(string message)
    {
        isGameOver = true;
        Time.timeScale = 0f;
        winMessageText.text = message;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
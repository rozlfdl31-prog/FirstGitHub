using UnityEngine;

public class Goal : MonoBehaviour
{
    // 인스펙터에서 _GameManager 오브젝트를 연결합니다.
    public GameManager gameManager;

    // [중요] 이 스크립트가 붙은 오브젝트(GameOverZone)의
    // Box Collider 2D에서 'Is Trigger'를 꼭 체크해주세요!
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거에 들어온 것이 플레이어(Player)가 *아니라면* (즉, 공(Ball)이라면)
        if (!other.CompareTag("Player"))
        {
            // GameManager에게 "패배"를 알립니다.
            if (gameManager != null)
            {
                // OnGoalScored(X)가 아니라 TriggerGameOver() (O)를 호출해야 합니다.
                gameManager.TriggerGameOver();
            }
        }
    }
}
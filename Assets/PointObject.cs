using UnityEngine;

public class PointObject : MonoBehaviour
{
    // 인스펙터에서 _GameManager 오브젝트를 연결합니다.
    public GameManager gameManager;
    public int points = 1; // 이 물체에 닿으면 얻는 점수

    // [삭제] public bool isLeftScoreBar; 
    // -> 이 헷갈리는 체크박스를 코드에서 아예 삭제했습니다.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 부딪힌 것이 플레이어(Player)가 *아니라면* (즉, 공(Ball)이라면)
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                // [신규] 스크립트가 스스로 자신의 X좌표를 확인합니다.
                // X좌표가 0보다 작으면(왼쪽이면) true, 0보다 크면(오른쪽이면) false가 됩니다.
                bool amIOnTheLeft = transform.position.x < 0;

                // [수정] GameManager에게 '왼쪽인지(true/false)'와 '점수'를 함께 전달합니다.
                gameManager.AddScore(amIOnTheLeft, points);
            }
        }
    }
}
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))] // Rigidbody 2D가 꼭 필요하다고 명시
public class Ball : MonoBehaviour
{
    // 인스펙터 창에서 시작 속도를 조절할 수 있습니다.
    public float startForce = 5f;

    private Rigidbody2D rb;

    // 게임이 시작될 때 (첫 프레임) 딱 한 번 호출됩니다.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall(); // 공을 발사하는 함수 호출
    }

    // 공을 랜덤한 방향으로 발사하는 함수
    void LaunchBall()
    {
        // X 방향을 랜덤으로 정함 (왼쪽 -1, 또는 오른쪽 1)
        float xDirection = Random.Range(0, 2) == 0 ? -1f : 1f;

        // Y 방향을 랜덤으로 정함 (위아래 랜덤)
        float yDirection = Random.Range(-0.5f, 0.5f); // 너무 수직으로 가지 않게 각도 조절

        // 방향 벡터를 정규화(normalized)하고 힘을 곱합니다.
        Vector2 direction = new Vector2(xDirection, yDirection).normalized;

        // 공에 순간적인 힘(Impulse)을 가합니다.
        rb.AddForce(direction * startForce, ForceMode2D.Impulse);
    }

    // 공을 리셋하라는 명령을 받는 공개 함수 (Goal.cs가 호출할 예정)
    public void ResetBall()
    {
        // 리셋 코루틴을 실행합니다.
        StartCoroutine(ResetAndLaunchCoroutine());
    }

    // IEnumerator는 코루틴을 의미하며, '시간차'를 두고 코드를 실행할 수 있게 해줍니다.
    private IEnumerator ResetAndLaunchCoroutine()
    {
        // 1. 공을 즉시 멈추고 중앙으로 이동
        rb.linearVelocity = Vector2.zero;    // <-- velocity 대신 linearVelocity로 변경 (일관성)
        rb.angularVelocity = 0f;             // <-- [추천] 공의 회전(Spin)도 즉시 멈춤
        transform.position = Vector3.zero;   // 위치를 (0, 0, 0) 중앙으로

        // 2. 1초 동안 기다림 (이 시간 동안 플레이어는 준비)
        yield return new WaitForSeconds(1.0f);

        // 3. 1초 뒤, 공을 다시 발사 (기존에 만든 함수 재사용)
        LaunchBall();
    }

}
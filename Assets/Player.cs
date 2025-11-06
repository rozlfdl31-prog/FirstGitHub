using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; // 코루틴을 위해 필요

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("설정")]
    public float speed = 5f;
    public float sprintSpeed = 10f;
    public InputActionAsset inputActions;
    public GameObject skillPrefab;

    [Header("내부 참조")]
    private Rigidbody2D rb;
    private InputActionMap playerMap;
    private InputAction moveAction;
    private float currentSpeed;

    // [수정] Sprint와 Skill 액션을 변수로 저장
    private InputAction sprintAction;
    private InputAction skillAction;

    void Awake() // Start 대신 Awake를 사용
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = speed;

        playerMap = inputActions.FindActionMap("Player");
        if (playerMap != null)
        {
            // [수정] 액션을 찾아 변수에 저장만 해 둡니다.
            moveAction = playerMap.FindAction("Move");
            sprintAction = playerMap.FindAction("Sprint");
            skillAction = playerMap.FindAction("Skill");
        }
        else
        {
            Debug.LogError("Player Input Action Map을 찾을 수 없습니다.");
        }
    }

    // [신규] OnEnable: 오브젝트가 활성화될 때 (Start 대신 사용)
    private void OnEnable()
    {
        if (playerMap != null)
        {
            playerMap.Enable(); // 맵 활성화

            // [수정] 여기서 '구독 신청'을 합니다.
            if (sprintAction != null)
            {
                sprintAction.performed += OnSprintPerformed; // Sprint 시작
                sprintAction.canceled += OnSprintCanceled;   // Sprint 취소
            }
            if (skillAction != null)
            {
                skillAction.performed += OnSkill; // 스킬 사용
            }
        }
    }

    // [신규] OnDisable: 오브젝트가 비활성화되거나 '파괴'될 때
    private void OnDisable()
    {
        if (playerMap != null)
        {
            // [수정] 여기서 '구독 취소'를 해서 유령을 청소합니다.
            if (sprintAction != null)
            {
                sprintAction.performed -= OnSprintPerformed;
                sprintAction.canceled -= OnSprintCanceled;
            }
            if (skillAction != null)
            {
                skillAction.performed -= OnSkill;
            }

            playerMap.Disable(); // 맵 비활성화
        }
    }

    // [신규] Sprint 관련 함수 (기존 _ => ... 코드를 함수로 뺌)
    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        currentSpeed = sprintSpeed;
    }
    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        currentSpeed = speed;
    }

    // [기존 함수] FixedUpdate (변경 없음)
    void FixedUpdate()
    {
        if (moveAction == null) return;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 movement = moveInput.normalized;
        rb.linearVelocity = movement * currentSpeed;
    }

    // [기존 함수] OnSkill (변경 없음)
    private void OnSkill(InputAction.CallbackContext context)
    {
        if (skillPrefab != null)
        {
            // 이펙트를 생성(Instantiate)함과 동시에,
            // 'this.transform'(플레이어 자신)을 부모(parent)로 지정합니다.
            GameObject _particle = GameObject.Instantiate(skillPrefab, transform.position, skillPrefab.transform.rotation, this.transform);

            StartCoroutine(ParticleOnDelay(_particle));
        }
    }

    // [기존 함수] ParticleOnDelay (변경 없음)
    IEnumerator ParticleOnDelay(GameObject p)
    {
        p.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        p.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        p.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        p.SetActive(true);
        Destroy(p, 1f);
        yield break;
    }
}
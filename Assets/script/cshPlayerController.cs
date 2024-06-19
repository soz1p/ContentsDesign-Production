using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가
using Photon.Pun;

public class cshPlayerController : MonoBehaviourPunCallbacks
{
    private Animator m_animator;
    private Vector3 m_velocity;
    private bool m_isGrounded = true;
    private bool m_jumpOn = false;
    private GameObject statueBearInstance; // 생성된 StatueBear의 인스턴스

    public float m_moveSpeed = 2.0f;
    public float m_runSpeed = 4.0f; // Run speed added
    public float m_jumpForce = 5.0f;
    public GameObject statueBearPrefab; // StatueBear 프리팹
    public float spawnRadius = 20.0f; // StatueBear 생성 반경
    public float detectionRadius = 10.0f; // 플레이어와 StatueBear 간의 거리 감지 반경
    private float timer = 0f;
    private bool hasSpawnedStatueBear = false;

    private cshAttackArea m_attackArea = null;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_attackArea = GetComponentInChildren<cshAttackArea>();

        if (!photonView.IsMine)
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        timer += Time.deltaTime;

        if (timer >= 5f && !hasSpawnedStatueBear)
        {
            SpawnStatueBear();
            hasSpawnedStatueBear = true;
        }

        if (statueBearInstance != null)
        {
            float distance = Vector3.Distance(transform.position, statueBearInstance.transform.position);
            Debug.Log("Distance to StatueBear: " + distance); // 거리 로그 출력

            if (distance <= detectionRadius)
            {
                Debug.Log("Player is close to StatueBear. Loading 'tryagain' scene.");
                SceneManager.LoadScene("tryagain");
            }
        }

        PlayerMove();
        m_animator.SetBool("Jump", !m_isGrounded);

        // Keyboard input for jumping
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
        {
            m_jumpOn = true;
        }
    }

    private void PlayerMove()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float gravity = 20.0f;
        float currentSpeed = m_moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = m_runSpeed; // Change speed to run speed when Shift is held
        }

        if (controller.isGrounded)
        {
            float h = Input.GetAxis("Horizontal"); // Get horizontal input
            float v = Input.GetAxis("Vertical"); // Get vertical input

            m_velocity = new Vector3(h, 0, v);
            m_velocity = m_velocity.normalized;

            m_animator.SetFloat("Move", m_velocity.magnitude);

            if (m_jumpOn)
            {
                m_velocity.y = m_jumpForce;
                m_jumpOn = false;
            }
            else if (m_velocity.magnitude > 0.5)
            {
                transform.LookAt(transform.position + m_velocity);
            }
        }

        m_velocity.y -= gravity * Time.deltaTime;
        controller.Move(m_velocity * currentSpeed * Time.deltaTime);

        m_isGrounded = controller.isGrounded;
    }

    private void SpawnStatueBear()
    {
        Vector3 spawnPosition = transform.position + (Random.insideUnitSphere * spawnRadius);
        spawnPosition.y = transform.position.y; // Y 좌표를 플레이어의 Y 좌표와 동일하게 설정
        statueBearInstance = Instantiate(statueBearPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("StatueBear spawned at: " + spawnPosition); // 생성 위치 로그 출력
    }

    public void OnVirtualPadJump()
    {
        if (this == null) { return; }
        const float rayDistance = 0.2f;
        var ray = new Ray(transform.localPosition + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down);
        if (Physics.Raycast(ray, rayDistance))
        {
            m_jumpOn = true;
        }
    }

    public bool CanAttack()
    {
        return 0 < m_attackArea.colliders.Count;
    }
    public void OnVirtualPadAttack()
    {
        if (this == null) { return; }

        m_animator.SetTrigger("Attack");

        Vector3 center = Vector3.zero;
        int cnt = m_attackArea.colliders.Count;
        int cntBreak = 0;

        for (int i = 0; i < m_attackArea.colliders.Count; ++i)
        {
            var collider = m_attackArea.colliders[i];
            center += collider.transform.localPosition;

            var obj = collider.GetComponent<cshBreakableObject>();
            if (obj != null)
            {
                obj.PlayEffect();
                cntBreak++;
            }
            var enemy = collider.GetComponent<cshEnemyController>();
            if (enemy != null)
            {
                enemy.Damage();
                if (enemy.GetHP() <= 0) m_attackArea.colliders.Clear();
            }
            else
            {
                Destroy(collider.gameObject);
            }
        }
        if (cntBreak > 0) m_attackArea.colliders.Clear();

        center /= cnt;
        center.y = transform.localPosition.y;
        transform.LookAt(center);
    }
}

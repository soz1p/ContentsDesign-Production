using UnityEngine;

public class cshPlayerController : MonoBehaviour
{
    private Animator m_animator;
    private Vector3 m_velocity;
    private bool m_isGrounded = true;
    private bool m_jumpOn = false;

    public float m_moveSpeed = 2.0f;
    public float m_runSpeed = 4.0f; // Run speed added
    public float m_jumpForce = 5.0f;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_attackArea = GetComponentInChildren<cshAttackArea>();
    }

    void Update()
    {
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

    private cshAttackArea m_attackArea = null;

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

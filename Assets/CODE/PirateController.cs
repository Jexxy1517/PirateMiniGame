using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PirateController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Punch")]
    public Transform punchOrigin;
    public float punchRadius = 0.8f;
    public float punchDamage = 25f;
    public LayerMask enemyMask;
    public bool blockMovementDuringPunch = false;

    [Header("Audio")]
    public AudioClip punchClip;  // suara saat meninju
    public AudioClip hurtClip;   // suara saat menerima damage

    private CharacterController controller;
    private Animator animator;
    private AudioSource audioSource;
    private Vector3 velocity;
    private bool isPunching = false;
    private bool punchHasHit = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (punchOrigin == null)
            punchOrigin = transform;
    }

    void Update()
    {
        if (!isPunching || !blockMovementDuringPunch)
        {
            HandleMovement();
            HandleJump();
        }
        else
        {
            if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        HandlePunchInput();
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up * turn * rotateSpeed * Time.deltaTime);

        Vector3 moveDir = transform.forward * move;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        bool isMoving = Mathf.Abs(move) > 0.1f;
        animator.SetBool("isWalk", isMoving);
        animator.SetBool("isJump", !controller.isGrounded);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !isPunching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJump", true);
        }
    }

    void HandlePunchInput()
    {
        if (Input.GetMouseButtonDown(0) && !isPunching)
        {
            animator.SetTrigger("Punch");

            // mainkan suara punch saat trigger animasi
            if (punchClip != null)
                audioSource.PlayOneShot(punchClip);
        }
    }

    public void OnPunchStart()
    {
        isPunching = true;
        punchHasHit = false;
        Debug.Log("Punch started");
    }

    public void OnPunchHit()
    {
        if (punchHasHit) return;
        punchHasHit = true;

        Debug.Log("Punch hit event");

        Collider[] hits = Physics.OverlapSphere(punchOrigin.position, punchRadius, enemyMask);
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                HealthSystem hs = col.GetComponentInParent<HealthSystem>();
                if (hs != null)
                {
                    hs.TakeDamage(punchDamage);

                    // mainkan suara hurt pada musuh (jika ada AudioSource)
                    AudioSource enemyAudio = col.GetComponentInParent<AudioSource>();
                    if (enemyAudio != null && hurtClip != null)
                        enemyAudio.PlayOneShot(hurtClip);
                }
            }
        }
    }

    public void OnPunchEnd()
    {
        isPunching = false;
        punchHasHit = false;
        Debug.Log("Punch ended");
    }

    void OnDrawGizmosSelected()
    {
        if (punchOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punchOrigin.position, punchRadius);
        }
    }

    public void OnHurt()
    {
        animator.SetTrigger("Hurt");

        // mainkan suara hurt
        if (audioSource != null && hurtClip != null)
            audioSource.PlayOneShot(hurtClip);
    }

    public void OnDie()
    {
        animator.SetTrigger("Die");
        enabled = false;
    }
}

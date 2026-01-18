using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float rotateSpeed = 3f;
    public float attackRange = 1.8f;
    public float attackCooldown = 1.5f;

    [Header("Audio")]
    public AudioClip punchClip;    // Suara saat enemy memukul
    public AudioClip hurtClip;     // Suara saat player terkena

    private CharacterController controller;
    private Animator animator;
    private AudioSource audioSource;
    private float gravity = -9.81f;
    private Vector3 velocity;
    private float lastAttackTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        if (distance > attackRange)
        {
            Vector3 moveDir = direction.normalized;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
            TryAttack();
        }

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Punch");

            // Mainkan suara punch
            if (punchClip != null)
                audioSource.PlayOneShot(punchClip);

            lastAttackTime = Time.time;

            // Damage player jika dekat
            if (Vector3.Distance(player.position, transform.position) < attackRange + 0.2f)
            {
                var playerHealth = player.GetComponent<HealthSystem>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(25f);

                    // Mainkan suara hurt pada player
                    var playerAudio = player.GetComponent<AudioSource>();
                    if (playerAudio != null && hurtClip != null)
                        playerAudio.PlayOneShot(hurtClip);
                }
            }
        }
    }
}

using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public float attackRange = 1.8f;
    public float attackCooldown = 0.8f;
    private float lastAttackTime;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Punch");
            lastAttackTime = Time.time;

            TryHitEnemy();
        }
    }

    void TryHitEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 1f, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                HealthSystem health = hit.GetComponent<HealthSystem>();
                if (health != null)
                {
                    health.TakeDamage(25f);
                }
            }
        }
    }
}

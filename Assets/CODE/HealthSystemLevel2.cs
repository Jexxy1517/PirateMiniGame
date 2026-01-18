using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HealthSystemLevel2 : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    [HideInInspector] public bool isDead = false;

    [Header("UI References")]
    public Image healthFill;        // Health bar fill
    public Image gameOverSprite;    // Player only
    public Image youWinSprite;      // Enemy only (Dan Level 2 Player)

    private Animator animator;
    private CannonManager cannonManager;

    // HAPUS variabel lokal ini, kita pakai punya HealthSystem (Level 1) saja biar sinkron
    // public static bool globalGameEnded = false; <--- HAPUS INI

    void Start()
    {
        // --- PERBAIKAN UTAMA DI SINI ---
        // Kita paksa reset status "Game Over" milik HealthSystem Level 1
        // Agar CannonManager (Timer) mau berjalan lagi.
        HealthSystem.globalGameEnded = false;
        // -------------------------------

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        cannonManager = FindObjectOfType<CannonManager>();

        // Hide UI
        if (gameOverSprite != null) gameOverSprite.enabled = false;
        if (youWinSprite != null) youWinSprite.enabled = false;

        // Full HP
        if (healthFill != null)
            healthFill.fillAmount = 1f;

        // Reset waktu (Penting agar timer jalan)
        Time.timeScale = 1f;
    }

    public void TakeDamage(float damage)
    {
        // Ganti 'globalGameEnded' jadi 'HealthSystem.globalGameEnded'
        if (isDead || HealthSystem.globalGameEnded) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        var playerController = GetComponent<PirateController>();
        if (playerController) playerController.enabled = false;

        var ai = GetComponent<EnemyAI>();
        if (ai) ai.enabled = false;

        if (HealthSystem.globalGameEnded) return;

        // ------------------- PLAYER DIED -------------------
        if (CompareTag("Player"))
        {
            HealthSystem.globalGameEnded = true; // Set punya Level 1 jadi true
            ShowGameOver();
            return;
        }

        // ------------------- ENEMY DIED -------------------
        if (CompareTag("Enemy"))
        {
            // Karena ini Level 2, kita bisa cek musuh mati di sini
            // Tapi biasanya logika Win Level 2 diatur CannonManager (Waktu habis)
            // Namun jika Anda ingin musuh mati = menang, biarkan logika ini:

            HealthSystem.globalGameEnded = true;
            ShowYouWin();
        }
    }

    // --- Dipanggil CannonManager saat Waktu Habis ---
    public void WinLevel2()
    {
        if (HealthSystem.globalGameEnded) return;

        HealthSystem.globalGameEnded = true; // Matikan timer lewat saklar utama
        ShowYouWin();
    }

    void DisableAllControllers()
    {
        PirateController player = FindObjectOfType<PirateController>();
        if (player != null) player.enabled = false;

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies) enemy.enabled = false;

        if (cannonManager != null) cannonManager.enabled = false;
    }

    void ShowGameOver()
    {
        DisableAllControllers();
        Time.timeScale = 0f;

        if (gameOverSprite != null)
            gameOverSprite.enabled = true;

        StartCoroutine(WaitAndLoadMenu());
    }

    public void ShowYouWin()
    {
        DisableAllControllers();
        Time.timeScale = 0f;

        if (youWinSprite != null)
            youWinSprite.enabled = true;

        StartCoroutine(WaitAndLoadMenu());
    }

    IEnumerator WaitAndLoadMenu()
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // --- LOGIC TABRAKAN ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckAndTakeDamage(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckAndTakeDamage(collision.gameObject);
    }

    private void CheckAndTakeDamage(GameObject attacker)
    {
        // 🌊 WATER = FORCE KILL (bypass TakeDamage lock)
        if (attacker.CompareTag("Water"))
        {
            if (!isDead)
            {
                currentHealth = 0;
                Die();
            }
            return;
        }

        // Cannonball & enemy damage
        if (attacker.CompareTag("CannonBall") || attacker.CompareTag("Enemy"))
        {
            TakeDamage(10f);

            if (attacker.CompareTag("CannonBall"))
            {
                Destroy(attacker);
            }
        }
}

}
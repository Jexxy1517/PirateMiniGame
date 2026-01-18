using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    [HideInInspector] public bool isDead = false;

    [Header("UI References")]
    public Image healthFill;        // Health bar fill
    public Image gameOverSprite;    // Player only
    public Image youWinSprite;      // Enemy only

    private Animator animator;
    private CannonManager cannonManager;

    // ANTI TIE
    public static bool globalGameEnded = false;

    void Start()
    {
        // --- PERBAIKAN PENTING DI SINI ---
        // Kita paksa reset jadi false setiap kali game dimulai
        globalGameEnded = false;
        // ---------------------------------

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        cannonManager = FindObjectOfType<CannonManager>();

        // Hide UI
        if (gameOverSprite != null) gameOverSprite.enabled = false;
        if (youWinSprite != null) youWinSprite.enabled = false;

        // Full HP
        if (healthFill != null)
            healthFill.fillAmount = 1f;
    }

    // -----------------------------------------------------------
    // ------------------------ TAKE DAMAGE -----------------------
    // -----------------------------------------------------------
    public void TakeDamage(float damage)
    {
        if (isDead) return;

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

    // -----------------------------------------------------------
    // --------------------------- DIE ----------------------------
    // -----------------------------------------------------------
    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log(gameObject.name + " has died");

        if (animator != null)
            animator.SetTrigger("Die");

        // Disable movement or AI
        var playerController = GetComponent<PirateController>();
        if (playerController) playerController.enabled = false;

        var ai = GetComponent<EnemyAI>();
        if (ai) ai.enabled = false;

        // GLOBAL ANTI TIE
        // Karena sudah di-reset di Start, logika ini sekarang aman
        if (globalGameEnded)
            return;

        // ------------------- PLAYER DIED FIRST -------------------
        if (CompareTag("Player"))
        {
            globalGameEnded = true;
            ShowGameOver();
            return;
        }

        // ------------------- ENEMY DIED ---------------------------
        if (CompareTag("Enemy"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                HealthSystem pHS = player.GetComponent<HealthSystem>();

                // TIE → Player death wins
                if (pHS != null && pHS.isDead)
                {
                    globalGameEnded = true;
                    ShowGameOver();
                    return;
                }
            }

            // Player masih hidup → You Win
            globalGameEnded = true;
            ShowYouWin();
        }
    }

    // -----------------------------------------------------------
    // ------------------ DISABLE ALL CONTROLLERS ----------------
    // -----------------------------------------------------------
    void DisableAllControllers()
    {
        PirateController player = FindObjectOfType<PirateController>();
        if (player != null)
            player.enabled = false;

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
            enemy.enabled = false;

        if (cannonManager != null)
            cannonManager.enabled = false;

        Debug.Log("All controllers disabled.");
    }

    // -----------------------------------------------------------
    // ------------------------- GAME OVER ------------------------
    // -----------------------------------------------------------
    void ShowGameOver()
    {
        DisableAllControllers();

        if (gameOverSprite != null)
            gameOverSprite.enabled = true;

        Invoke(nameof(BackToMainMenu), 3f);
    }

    // -----------------------------------------------------------
    // ------------------------- YOU WIN --------------------------
    // -----------------------------------------------------------
    void ShowYouWin()
    {
        DisableAllControllers();

        if (youWinSprite != null)
            youWinSprite.enabled = true;

        Invoke(nameof(BackToMainMenu), 3f);
    }

    // -----------------------------------------------------------
    // ---------------------- MAIN MENU ---------------------------
    // -----------------------------------------------------------
    void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
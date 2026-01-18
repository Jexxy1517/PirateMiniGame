using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Cannon Settings")]
    public GameObject cannonballPrefab;
    public Transform firePoint;
    public float cannonballSpeed = 20f;

    [Header("Ship Movement Reference")]
    public EnemyShipMover shipMover;

    [Header("Warning Area Settings")]
    public GameObject warningAreaPrefab;
    public float warningDuration = 1.5f;
    public float warningAreaLength = 10f;
    public float warningAreaOffset = 0.1f;

    [Header("Firing Rate")]
    public float minInitialFireRate = 3f;
    public float maxInitialFireRate = 5f;

    [Header("Sound")]
    public AudioClip cannonShotSFX;
    private AudioSource audioSource;

    private GameObject currentWarningArea;
    private float nextFireTime;
    private CannonManager cannonManager;

    void Start()
    {
        cannonManager = FindObjectOfType<CannonManager>();

        if (shipMover == null)
        {
            shipMover = GetComponentInParent<EnemyShipMover>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        nextFireTime = Time.time + Random.Range(minInitialFireRate, maxInitialFireRate);
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            if (currentWarningArea == null)
            {
                ShowWarningArea();
                nextFireTime = Time.time + warningDuration;
            }
            else
            {
                FireCannonball();

                if (currentWarningArea != null) Destroy(currentWarningArea);
                currentWarningArea = null;

                float difficulty = cannonManager != null ? cannonManager.CurrentDifficulty : 0f;
                float dynamicFireRate = Mathf.Lerp(maxInitialFireRate, minInitialFireRate, difficulty * 1.5f);
                dynamicFireRate = Mathf.Max(0.5f, dynamicFireRate);

                float randomOffset = Random.Range(-0.3f, 0.3f);
                nextFireTime = Time.time + dynamicFireRate + randomOffset;
            }
        }
    }

    void ShowWarningArea()
    {
        if (warningAreaPrefab == null || firePoint == null) return;

        Vector3 warningPosition = firePoint.position + firePoint.forward * (warningAreaLength / 2f);
        warningPosition.y = warningAreaOffset;

        currentWarningArea = Instantiate(warningAreaPrefab, warningPosition, firePoint.rotation);

        Vector3 currentScale = currentWarningArea.transform.localScale;
        currentWarningArea.transform.localScale = new Vector3(currentScale.x, currentScale.y, warningAreaLength);
    }

    void FireCannonball()
    {
        if (cannonballPrefab == null || firePoint == null) return;

        if (cannonShotSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(cannonShotSFX);
        }

        if (shipMover != null)
        {
            shipMover.OnCannonFired();
        }

        GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * cannonballSpeed, ForceMode.VelocityChange);
        }
    }
}
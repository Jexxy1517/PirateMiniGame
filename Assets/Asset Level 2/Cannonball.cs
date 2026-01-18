using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float lifetime = 5f;
    public float damage = 34f;

    void Start()
    {
        // Tidak perlu kode aneh-aneh, Unity Physics Matrix sudah mengurusnya!
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Cek jika kena Player
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystemLevel2 playerHealth = collision.gameObject.GetComponent<HealthSystemLevel2>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Hancur kena pemain
        }
        // Cek jika kena Raft atau Ground (Layer Default/Ground/Raft)
        // Cannonball TIDAK AKAN masuk sini jika kena Pagar, karena sudah di-ignore via Matrix
        else if (collision.gameObject.CompareTag("Raft") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.2f); // Hancur kena lantai/rakit
        }
    }
}
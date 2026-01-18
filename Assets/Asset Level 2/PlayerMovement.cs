using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; 
    public float dodgeStrength = 5f; 
    public float jumpCooldown = 1f;
    private float lastJumpTime;

    private Rigidbody rb;
    private bool isGrounded;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true; 
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Player! Make sure it's attached.");
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (moveDirection != Vector3.zero) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        float currentSpeed = moveDirection.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        if (Input.GetButtonDown("Jump") && isGrounded && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * dodgeStrength, ForceMode.Impulse);
            lastJumpTime = Time.time;
            isGrounded = false;

            animator.SetBool("IsJump", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Raft") || collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (animator.GetBool("IsJump"))
            {
                animator.SetBool("IsJump", false);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Raft") || collision.gameObject.CompareTag("Ground"))
        {
            Invoke("CheckIfStillGrounded", 0.1f);
        }
    }

    void CheckIfStillGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 0.2f;

        Vector3 raycastOrigin = transform.position + Vector3.down * (GetComponent<Collider>().bounds.extents.y - 0.05f);


        if (!Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance, LayerMask.GetMask("Raft", "Ground")))
        {
            isGrounded = false;
        }
        Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, isGrounded ? Color.green : Color.red);
    }
}
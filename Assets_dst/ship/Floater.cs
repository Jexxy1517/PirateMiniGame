using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour
{
    public WaterPhysics water;       // Drag your water plane here in the Inspector
    public float floatHeight = 1f;   // Desired distance above the water
    public float bounceDamp = 0.05f; // Smooth damping of float movement
    public float liftForce = 10f;    // Strength of buoyancy

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (water == null) return;

        // Get water height at this object's position
        float waterHeight = water.GetWaterHeightAt(transform.position);

        // How far below/above water we are
        float difference = (waterHeight + floatHeight) - transform.position.y;

        // Apply buoyancy if below the surface
        if (difference > 0f)
        {
            Vector3 uplift = Vector3.up * difference * liftForce;
            rb.AddForce(uplift - rb.linearVelocity * bounceDamp, ForceMode.Acceleration);
        }
    }
}

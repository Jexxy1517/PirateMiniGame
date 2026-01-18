using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipFloater : MonoBehaviour
{
    public WaterPhysics water;         // Reference to your WaterPhysics script
    public Transform[] floatPoints;    // 4 or more empty GameObjects under the ship

    [Header("Buoyancy Settings")]
    public float buoyancyStrength = 15f;  // Force pushing up
    public float waterDrag = 1f;          // Damping (reduces jitter)
    public float waterAngularDrag = 0.5f; // Rotational damping

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        if (water == null) return;

        // Total number of active float points
        int pointsUnderWater = 0;

        foreach (var point in floatPoints)
        {
            float waterHeight = water.GetWaterHeightAt(point.position);
            float heightDiff = waterHeight - point.position.y;

            if (heightDiff > 0f)
            {
                // Add upward force proportional to how deep the point is submerged
                float force = buoyancyStrength * heightDiff;
                rb.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Acceleration);
                pointsUnderWater++;
            }
        }

        // Add water drag (resistance)
        if (pointsUnderWater > 0)
        {
            rb.AddForce(-rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(-rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}

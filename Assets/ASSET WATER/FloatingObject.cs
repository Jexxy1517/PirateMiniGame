using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Water Reference")]
    public Material waterMaterial;   // assign your BigWaveWater material here in Inspector

    [Header("Floating Settings")]
    public float buoyancyOffset = 0f;   // lift object slightly above water
    public float smoothness = 5f;       // higher = smoother bobbing
    public bool tiltWithWaves = true;   // rocking effect

    private float waveSpeed, waveHeight, waveFrequency;
    private Vector3 targetPos;

    void Update()
    {
        // Get shader values dynamically
        waveSpeed     = waterMaterial.GetFloat("_WaveSpeed");
        waveHeight    = waterMaterial.GetFloat("_WaveHeight");
        waveFrequency = waterMaterial.GetFloat("_WaveFrequency");
        Vector4 rippleOrigin = waterMaterial.GetVector("_RippleOrigin");

        Vector3 pos = transform.position;

        // --- Match shader math exactly ---
        float t = Time.time * waveSpeed;
        float dist = Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(rippleOrigin.x, rippleOrigin.z));
        float wave = Mathf.Sin(dist * waveFrequency - t) * waveHeight;
        // --------------------------------

        float waterHeight = wave;

        // Smooth interpolation so it doesn’t snap
        targetPos = new Vector3(pos.x, waterHeight + buoyancyOffset, pos.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothness);

        // Optional tilt/rocking with slope of wave
        if (tiltWithWaves)
        {
            // Approximate slope by sampling wave height around object
            float sampleOffset = 0.5f;
            float hX = Mathf.Sin((Vector2.Distance(new Vector2(pos.x + sampleOffset, pos.z), new Vector2(rippleOrigin.x, rippleOrigin.z)) * waveFrequency - t)) * waveHeight;
            float hZ = Mathf.Sin((Vector2.Distance(new Vector2(pos.x, pos.z + sampleOffset), new Vector2(rippleOrigin.x, rippleOrigin.z)) * waveFrequency - t)) * waveHeight;

            float tiltX = (hX - waterHeight) * 20f;
            float tiltZ = (hZ - waterHeight) * 20f;

            Quaternion targetRot = Quaternion.Euler(tiltX, transform.rotation.eulerAngles.y, tiltZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smoothness);
        }
    }
}

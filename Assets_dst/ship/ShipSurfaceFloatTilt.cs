using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipSurfaceFloatTiltStable : MonoBehaviour
{
    public WaterPhysics water;
    public float offsetHeight = 0.5f;
    public float followSpeed = 2f;
    public float tiltSmooth = 1.5f;
    public float probeDistance = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (water == null) return;

        Vector3 pos = transform.position;

        // Ambil titik sampling di sekitar kapal
        Vector3 front = pos + transform.forward * probeDistance;
        Vector3 back = pos - transform.forward * probeDistance;
        Vector3 left = pos - transform.right * probeDistance;
        Vector3 right = pos + transform.right * probeDistance;

        // Tinggi permukaan air di titik-titik tersebut
        float centerY = water.GetWaterHeightAt(pos);
        float frontY = water.GetWaterHeightAt(front);
        float backY = water.GetWaterHeightAt(back);
        float leftY = water.GetWaterHeightAt(left);
        float rightY = water.GetWaterHeightAt(right);

        // Hitung rata-rata tinggi air
        float avgY = (centerY + frontY + backY + leftY + rightY) / 5f + offsetHeight;

        // Update posisi (smooth)
        pos.y = Mathf.Lerp(pos.y, avgY, Time.fixedDeltaTime * followSpeed);
        transform.position = pos;

        // Buat vektor normal permukaan berdasarkan perbedaan tinggi
        Vector3 forwardDir = new Vector3(0f, frontY - backY, 2f).normalized;
        Vector3 rightDir = new Vector3(2f, rightY - leftY, 0f).normalized;
        Vector3 surfaceNormal = Vector3.Cross(rightDir, forwardDir).normalized;

        // Pastikan normal tidak terbalik (menghadap ke bawah)
        if (surfaceNormal.y < 0f)
            surfaceNormal = -surfaceNormal;

        // Hitung rotasi target
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;

        // Batasi rotasi hanya sebagian biar gak 180°
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * tiltSmooth);
    }
}

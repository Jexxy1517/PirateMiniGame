using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterPhysics : MonoBehaviour
{
    [Header("Wave Settings")]
    public float waveHeight = 0.5f;
    public float waveFrequency = 0.5f;
    public float waveSpeed = 1f;

    private Mesh mesh;
    private Vector3[] baseVertices;
    private Vector3[] displacedVertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        baseVertices = mesh.vertices;
        displacedVertices = new Vector3[baseVertices.Length];
    }

    void Update()
    {
        AnimateWaves();
    }

    void AnimateWaves()
    {
        float time = Time.time * waveSpeed;

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            vertex.y = Mathf.Sin(vertex.x * waveFrequency + vertex.z * waveFrequency + time) * waveHeight;
            displacedVertices[i] = vertex;
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }

    // Gets the Y height of the wave at a world position
    public float GetWaterHeightAt(Vector3 position)
    {
        float time = Time.time * waveSpeed;
        float wave = Mathf.Sin(position.x * waveFrequency + position.z * waveFrequency + time);
        return transform.position.y + wave * waveHeight;
    }
}

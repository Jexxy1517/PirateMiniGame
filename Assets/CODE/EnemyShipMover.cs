using UnityEngine;
using System.Collections;

public class EnemyShipMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistanceMin = 0.5f;
    public float moveDistanceMax = 1.5f;
    public float moveSpeed = 2.0f;
    public float returnSpeed = 1.0f;

    private Vector3 initialPosition;
    private bool isMoving = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void OnCannonFired()
    {
        if (!isMoving)
        {
            StartCoroutine(SideMoveRoutine());
        }
    }

    IEnumerator SideMoveRoutine()
    {
        isMoving = true;
        float randomDist = Random.Range(moveDistanceMin, moveDistanceMax);
        float directionMultiplier = Random.value > 0.5f ? 1f : -1f;

        Vector3 targetPos = initialPosition + (transform.right * randomDist * directionMultiplier);

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while (Vector3.Distance(transform.position, initialPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.position = initialPosition;
        isMoving = false;
    }
}
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float amplitude = 0.01f; // The height of the floating effect
    public float frequency = 1f; // The speed of the floating effect

    private bool isFloating = false;
    private Vector3 startPos;
    private float randomOffset; // To ensure each object wobbles differently

    void Start()
    {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI); // Random offset for variety
    }

    void Update()
    {
        if (isFloating)
        {
            Floating();
        }
    }

    void Floating()
    {
        // Only float up/down, don't modify x and z
        Vector3 tempPos = transform.position;
        tempPos.y = startPos.y + Mathf.Sin(Time.fixedTime * Mathf.PI * frequency + randomOffset) * amplitude;
        transform.position = tempPos;
    }

    public void SetFloating(bool floating)
    {
        isFloating = floating;
    }
}

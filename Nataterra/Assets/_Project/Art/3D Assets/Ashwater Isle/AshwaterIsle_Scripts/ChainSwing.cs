using UnityEngine;

public class GentleSwing : MonoBehaviour
{
    public float swingAmplitude = 5f; // degrees
    public float swingSpeed = 1f;     // cycles per second
    public float swingOffset = 0f;    // phase offset

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        swingOffset = Random.Range(0f, Mathf.PI * 2); // vary across objects
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed * 2f * Mathf.PI + swingOffset) * swingAmplitude;
        transform.localRotation = initialRotation * Quaternion.Euler(angle, 0f, 0f);
    }
}
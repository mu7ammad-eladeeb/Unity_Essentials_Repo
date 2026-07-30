using UnityEngine;

/// <summary>
/// Rotates a Directional Light to simulate a smooth day and night cycle.
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Tooltip("The duration of a full day/night cycle in real-time seconds.")]
    public float dayDurationInSeconds = 120.0f;

    void Update()
    {
        // Prevent division by zero if dayDurationInSeconds is set to 0 or negative
        if (dayDurationInSeconds <= 0) return;

        // A full rotation is 360 degrees.
        // Degrees to rotate per second = 360 / total day length in seconds
        float rotationSpeed = 360.0f / dayDurationInSeconds;

        // Rotate around the X-axis over time
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
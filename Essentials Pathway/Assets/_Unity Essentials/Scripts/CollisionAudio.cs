using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class CollisionAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("The sound clip to play on impact.")]
    [SerializeField] private AudioClip impactSound;

    [Tooltip("Minimum impact force required to play the sound.")]
    [SerializeField] private float minImpactVelocity = 1.0f;

    [Tooltip("Impact force at which the volume reaches maximum (1.0).")]
    [SerializeField] private float maxImpactVelocity = 10.0f;

    [Header("Pitch Variation (Optional)")]
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private float minPitch = 0.85f;
    [SerializeField] private float maxPitch = 1.15f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Calculate relative collision speed
        float impactForce = collision.relativeVelocity.magnitude;

        // Ignore soft contacts or sliding
        if (impactForce < minImpactVelocity) return;

        // Scale volume relative to impact force
        float volume = Mathf.InverseLerp(minImpactVelocity, maxImpactVelocity, impactForce);

        // Apply pitch variation for natural sound
        if (randomizePitch)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }

        // Play sound clip without cutting off previous overlapping bounces
        audioSource.PlayOneShot(impactSound, volume);
    }
}
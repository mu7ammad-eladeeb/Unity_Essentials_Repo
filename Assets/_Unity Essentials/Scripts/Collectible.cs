using UnityEngine; // Provides access to core Unity features, classes, and components (like MonoBehaviour, GameObject, Vector3)

// Defines the Collectible class, which inherits from MonoBehaviour so it can be attached to Unity GameObjects
public class Collectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Public float exposed in the Unity Inspector to control how fast the object rotates (in degrees per frame)
    public float rotationSpeed;

    // Public reference to a GameObject prefab (like a particle effect) to spawn when collected
    public GameObject onCollectEffect;

    [Header("Audio Settings")]
    // Sound clip to play every time a single coin is collected
    public AudioClip collectSound;

    // Volume slider for collection sound (0.0 = silent, 1.0 = 100% volume)
    [Range(0f, 1f)] public float collectVolume = 1.0f;

    // Sound clip to play when the final coin is collected and the player wins
    public AudioClip winSound;

    // Volume slider for win sound (0.0 = silent, 1.0 = 100% volume)
    [Range(0f, 1f)] public float winVolume = 1.0f;

    [Header("Win Settings")]
    // Public reference to the visual effect prefab to spawn when all collectibles are gathered
    public GameObject winVFXPrefab;

    // Called on the frame when the script is enabled, before any Update methods are called
    void Start()
    {
        // Currently empty, but useful if you need to initialize variables or components when the game starts
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0); // Rotate the collectible around the Y-axis
    }

    // Automatically called by Unity when another Collider enters this object's Trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Checks if the object entering the trigger has the "Player" tag assigned in the Inspector
        if (other.CompareTag("Player"))
        {
            // Instantiate (spawn) the particle effect prefab at this collectible's current position and orientation
            if (onCollectEffect != null)
            {
                Instantiate(onCollectEffect, transform.position, transform.rotation);
            }

            // Play the pickup sound as a 2D sound using the custom collectVolume level
            if (collectSound != null)
            {
                PlayClip2D(collectSound, collectVolume);
            }

            // Find all collectibles currently in the scene
            GameObject[] remainingCoins = GameObject.FindGameObjectsWithTag("Collectible");

            // Subtract 1 because THIS coin is about to be destroyed at the end of the frame!
            int coinsLeftAfterThisOne = remainingCoins.Length - 1;

            // Log remaining count to Console to track coin detection in real-time
            Debug.Log($"Coin picked up! Remaining collectibles in scene: {coinsLeftAfterThisOne}");

            // If zero coins remain after this one, spawn the grand victory effect
            if (coinsLeftAfterThisOne <= 0)
            {
                Debug.Log("🎉 LAST COIN COLLECTED! Spawning Win VFX now!");

                if (winVFXPrefab != null)
                {
                    // Spawn the grand win effect 1.5 units above the player's position
                    Instantiate(winVFXPrefab, other.transform.position + Vector3.up * 1.5f, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("❌ winVFXPrefab field is EMPTY on this specific coin!");
                }

                // Play victory fanfares / win sound effect as a 2D sound using the custom winVolume level
                if (winSound != null)
                {
                    PlayClip2D(winSound, winVolume);
                }
            }

            // Destroy the collectible when the player collides with it
            Destroy(gameObject); // gameObject (with lowercase 'g') refers to the GameObject this script is attached to (the collectible) not the player
        }
    }

    // Helper method to play audio clips as 2D (non-spatial) sounds with controllable volume
    private void PlayClip2D(AudioClip clip, float volume)
    {
        GameObject audioGO = new GameObject("Temp2DAudio");
        AudioSource source = audioGO.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0.0f; // 0.0f forces 2D sound (ignores distance / camera position)
        source.Play();

        // Automatically clean up the temporary GameObject after the audio finishes playing
        Destroy(audioGO, clip.length);
    }
}

/*
A Quick Tip for Smooth Rotation (By Gemini AI):

In your Update() method, consider multiplying rotationSpeed by Time.deltaTime (e.g., transform.Rotate(0, rotationSpeed * Time.deltaTime, 0)).
This makes the rotation frame-rate independent, ensuring the item spins at the exact same speed on both slow and fast devices!
*/
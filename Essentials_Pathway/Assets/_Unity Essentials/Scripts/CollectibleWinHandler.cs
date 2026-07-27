using UnityEngine;
using TMPro; // Needed if using TextMeshPro for UI

public class CollectibleWinHandler : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Drag your UI Text (TMP) component here to display remaining count.")]
    [SerializeField] private TMP_Text remainingText;
    [SerializeField] private string uiPrefix = "Remaining: ";

    [Header("Winning Effects")]
    [Tooltip("The Particle System to play when all items are collected.")]
    [SerializeField] private ParticleSystem winParticleSystem;

    [Tooltip("Set this to 'true' if the Win Particle System is already placed in the scene and you just need it to play.")]
    [SerializeField] private bool useSceneVFX = true;

    [Tooltip("If 'useSceneVFX' is false, instantiate the VFX prefab at this specific transform position (like above the player or center screen). If null, plays at this manager's position.")]
    [SerializeField] private Transform vfxSpawnPoint;

    private int totalCollectibles;
    private int collectedCount = 0;
    private bool hasWon = false;

    private void Start()
    {
        // Automatically find all objects tagged as "Collectible" in the scene
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        totalCollectibles = collectibles.Length;

        // Ensure the VFX starts off
        if (winParticleSystem != null)
        {
            if (winParticleSystem.isPlaying)
            {
                winParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        UpdateUI();
    }

    public void CollectItem()
    {
        if (hasWon) return;

        collectedCount++;
        UpdateUI();

        // Check Win Condition
        if (collectedCount >= totalCollectibles)
        {
            WinGame();
        }
    }

    private void UpdateUI()
    {
        int remaining = totalCollectibles - collectedCount;
        if (remainingText != null)
        {
            remainingText.text = $"{uiPrefix}{remaining}";
        }
    }

    private void WinGame()
    {
        hasWon = true;

        TriggerWinVFX();

        Debug.Log("All collectibles gathered! You win!");
    }

    private void TriggerWinVFX()
    {
        if (winParticleSystem == null) return;

        if (useSceneVFX)
        {
            // The VFX is already positioned in the room, just start emitting
            winParticleSystem.Play();
        }
        else
        {
            // Treat winParticleSystem as a PREFAB and spawn it at the spawn point
            Transform spawnTrans = (vfxSpawnPoint != null) ? vfxSpawnPoint : this.transform;
            ParticleSystem spawnedVFX = Instantiate(winParticleSystem, spawnTrans.position, spawnTrans.rotation);
            spawnedVFX.Play();

            // Optional: Destroy the spawned VFX object after its duration is done
            Destroy(spawnedVFX.gameObject, spawnedVFX.main.duration + 1.0f);
        }
    }
}
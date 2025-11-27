using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelGeneratorPath generator;
    public Transform player;
    public SreanFadder fadeController;

    [Header("UI")]
    public GameObject deathPanel;

    public void ResetPlayer()
    {
        // fade-out -> reset -> fade-in
        if (fadeController != null)
        {
            fadeController.FadeOut(() =>
            {
                ResetPlayerInternal();
                fadeController.FadeIn();
            });
        }
        else
        {
            ResetPlayerInternal();
        }
    }

    public void ResetLevel()
    {
        if (fadeController != null)
        {
            Debug.Log("cos");
            fadeController.FadeOut(() =>
            {
                generator.GeneratePath();
                ResetPlayerInternal();
                fadeController.FadeIn();
            });
        }
        else
        {
            Debug.Log("c222os");
            generator.GeneratePath();
            ResetPlayerInternal();
        }
    }

    private void ResetPlayerInternal()
    {
        // 1. Ukrycie DeathPanel
        if (deathPanel != null)
            deathPanel.SetActive(false);

        // 2. Ustawienie gracza dokładnie na środku startowego bloku
        Vector3 startBlockPos = generator.startPos;
        player.position = startBlockPos + Vector3.up * 1f;
        player.rotation = Quaternion.identity;

        // 3. Reset Rigidbody
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
        Time.timeScale = 1f;
        // 4. Reset stanu gracza
        var playerScript = player.GetComponent<PlayerJump>();
        if (playerScript != null)
            playerScript.ResetPlayerState();

        // 5. Włączenie strzałki od razu
        var arrow = FindObjectOfType<arrow>();
        
    }
}



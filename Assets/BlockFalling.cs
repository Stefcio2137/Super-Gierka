using UnityEngine;
using System.Collections;

public class BlockFalling : MonoBehaviour
{
    public float timeBeforeFall = 1.2f;
    public float destroyAfter = 3f;

    private bool started = false;
    private bool playerOnBlock = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"BlockFalling on '{gameObject.name}' wymaga Rigidbody!");
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    // wywoływane przez FallingBlockTrigger
    public void PlayerEntered()
    {
        playerOnBlock = true;
        if (!started)
            StartCoroutine(FallRoutine());
    }

    public void PlayerExited()
    {
        playerOnBlock = false;
    }

    IEnumerator FallRoutine()
    {
        started = true;
        yield return new WaitForSeconds(timeBeforeFall);

        if (playerOnBlock)
        {
            var player = FindObjectOfType<PlayerJump>();
            if (player != null) player.KillPlayer();
        }

        rb.isKinematic = false;
        rb.useGravity = true;

        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }
}

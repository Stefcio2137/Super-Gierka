using UnityEngine;

public class FallingBlockTrigger : MonoBehaviour
{
    [Tooltip("Przypisz BlockFalling rêcznie lub zostaw puste, wtedy spróbuje znaleŸæ w rodzicu.")]
    public BlockFalling block;

    void Start()
    {
        if (block == null)
            block = GetComponentInParent<BlockFalling>();

        if (block == null)
            Debug.LogError($"FallingBlockTrigger on '{gameObject.name}' nie ma przypisanego BlockFalling i nie znalaz³ go w rodzicu!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (block == null) return; // bez crasha
        if (other.CompareTag("player"))
            block.PlayerEntered();
    }

    void OnTriggerExit(Collider other)
    {
        if (block == null) return;
        if (other.CompareTag("player"))
            block.PlayerExited();
    }
}

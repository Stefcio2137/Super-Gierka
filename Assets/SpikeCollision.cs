using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("player"))
        {
            var p = FindObjectOfType<PlayerJump>();
            if (p != null) p.KillPlayer();
        }
    }

    // alternatywnie — dla trigger colliderów:
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            var p = FindObjectOfType<PlayerJump>();
            if (p != null) p.KillPlayer();
        }
    }
}

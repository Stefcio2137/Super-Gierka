using UnityEngine;
using System.Collections;

public class SpikeBlock : MonoBehaviour
{
    [Tooltip("Transform kolca (dziecko).")]
    public Transform spike;

    [Tooltip("Wysokość kolca względem lokalnej pozycji platformy gdy jest wysunięty.")]
    public float upHeight = 0.6f;

    [Tooltip("Czas między zmianami stanu (sekundy).")]
    public float cycleTime = 2f;

    [Tooltip("Prędkość poruszania kolca.")]
    public float speed = 6f;

    // opcjonalne: mała korekta Y pozycji dla pozycji schowanej (żeby być pewnym, że nie koliduje)
    public float hiddenYOffset = -0.12f;

    private bool spikeOut = false;
    private bool playerOnBlock = false;
    private Vector3 spikeHiddenPos;
    private Vector3 spikeOutPos;
    private Collider spikeCollider;

    void Start()
    {
        if (spike == null)
        {
            Debug.LogError("SpikeBlock: przypisz Transform kolca (spike).");
            enabled = false;
            return;
        }

        // ustalamy pozycje lokalne: schowana i wystawiona
        spikeHiddenPos = new Vector3(spike.localPosition.x, hiddenYOffset, spike.localPosition.z);
        spikeOutPos = new Vector3(spike.localPosition.x, upHeight, spike.localPosition.z);

        // ustaw początkowo schowaną pozycję i pobierz collider
        spike.localPosition = spikeHiddenPos;

        spikeCollider = spike.GetComponent<Collider>();
        if (spikeCollider == null)
        {
            // dodajemy BoxCollider jeśli brak
            spikeCollider = spike.gameObject.AddComponent<BoxCollider>();
        }

        // Na start collider powinien być wyłączony (kolec schowany)
        spikeCollider.enabled = false;

        // upewnij się, że spike ma kinematic Rigidbody, żeby MoveTowards działało bez konfliktów fizyki
        Rigidbody rb = spike.GetComponent<Rigidbody>();
        if (rb == null) rb = spike.gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        StartCoroutine(Cycle());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("player")) return;

        playerOnBlock = true;

        // jeśli kolec już jest wysunięty i jego collider aktywny — gracz ginie
        if (spikeOut && spikeCollider != null && spikeCollider.enabled)
        {
            Debug.Log("SpikeBlock: gracz wszedł gdy kolce wysunięte -> KillPlayer");
            var p = FindObjectOfType<PlayerJump>();
            if (p != null) p.KillPlayer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("player")) return;
        playerOnBlock = false;
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(cycleTime);

            spikeOut = !spikeOut;
            Vector3 target = spikeOut ? spikeOutPos : spikeHiddenPos;

            // jeśli wysuwamy, najpierw włączamy collider dopiero gdy kolec już wystaje na pewną część
            bool enablingCollider = spikeOut;

            while (Vector3.Distance(spike.localPosition, target) > 0.01f)
            {
                spike.localPosition = Vector3.MoveTowards(spike.localPosition, target, speed * Time.deltaTime);

                // gdy wysuwamy: włącz collider dopiero gdy kolec osiągnie połowę drogi (opcjonalnie)
                if (enablingCollider && spikeCollider != null)
                {
                    float progress = Mathf.InverseLerp(spikeHiddenPos.y, spikeOutPos.y, spike.localPosition.y);
                    if (progress >= 0.5f && !spikeCollider.enabled)
                        spikeCollider.enabled = true;
                }

                yield return null;
            }

            spike.localPosition = target;

            // Upewnij się stan collidera odpowiada stanu spikeOut (jeśli chowamy -> wyłączamy collider)
            if (spikeCollider != null)
                spikeCollider.enabled = spikeOut;

            // jeśli kolec wysunął się pod graczem (playerOnBlock) — zabijamy
            if (spikeOut && playerOnBlock)
            {
                Debug.Log("SpikeBlock: kolec pojawił się pod graczem -> KillPlayer");
                var p = FindObjectOfType<PlayerJump>();
                if (p != null) p.KillPlayer();
            }
        }
    }
}

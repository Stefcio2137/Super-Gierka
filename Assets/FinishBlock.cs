using UnityEngine;
using System.Collections;

public class FinishBlock : MonoBehaviour
{
    [Tooltip("Panel UI wygranej")]
    public GameObject winPanel;

    [Tooltip("G³ówna kamera do shakera")]
    public Camera mainCamera;

    [Tooltip("Czas trwania shake")]
    public float shakeDuration = 0.5f;

    [Tooltip("Intensywnoœæ shake")]
    public float shakeMagnitude = 0.3f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("player")) return;

        triggered = true;

        // poka¿ panel wygranej
        if (winPanel != null)
            winPanel.SetActive(true);
        Time.timeScale = 0f;

        // uruchom shaker
        if (mainCamera != null)
            StartCoroutine(CameraShake());
    }

    private IEnumerator CameraShake()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }
}

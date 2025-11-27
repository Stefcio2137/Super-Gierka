using UnityEngine;

public class BoostBlock : MonoBehaviour
{
    public float boostMultiplier = 2f;   // x2 szybciej
    public float boostDuration = 2f;     // ile czasu trwa turbo

    private bool triggered = false;      // aby dzia³a³o tylko raz

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("player"))
        {
            arrow arrow = FindObjectOfType<arrow>();
            if (arrow != null)
            {
                
                triggered = true;
                StartCoroutine(BoostRoutine(arrow));
            }
        }
    }

    private System.Collections.IEnumerator BoostRoutine(arrow arrow)
    {
        float originalSpeed = arrow.rotateSpeed;
        arrow.rotateSpeed *= boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        arrow.rotateSpeed = originalSpeed;
        triggered = false; // pozwala u¿yæ bloku ponownie
    }
}

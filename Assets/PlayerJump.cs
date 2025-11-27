using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJump : MonoBehaviour
{
    [Header("movement")]
    public Transform arrow;
    public float jumpHeight = 2f;
    public float jumpTime = 0.3f;
    public GameObject gameOverUI;  // przypnij panel
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    private bool isJumping = false;
    private bool falling = false;
    [Header("Arrow")]
    public GameObject arrowObject;
    private bool onBlock = true;
    void Update()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
    }

    void TryJump()
    {
        
        Vector3 dir = Vector3.zero;
        float y = arrow.eulerAngles.y;

        if (Mathf.Abs(y - 90) < 10f) dir = Vector3.forward;
        else if (Mathf.Abs(y - 180) < 10f) dir = Vector3.right;
        else if (Mathf.Abs(y - 270) < 10f) dir = Vector3.back;
        else if (Mathf.Abs(y - 0) < 10f) dir = Vector3.left;

        Vector3 targetPos = transform.position + dir * 2f;

        bool block = BlockExists(targetPos);

        if (!block)
        {
            // skaczemy w przepaœæ
            audioSource.PlayOneShot(deathSound);
            Vector3 fallTarget = transform.position + dir * 4f; // dalej, ¿eby wygl¹da³o jak upadek
            arrowObject.SetActive(false);
            onBlock = false;

            StartCoroutine(JumpIntoVoid(fallTarget));
            return;
        }

        // normalny skok na blok
        audioSource.PlayOneShot(jumpSound);
        arrowObject.SetActive(false);
        onBlock = false;

        StartCoroutine(JumpAnimation(targetPos));
    }

    bool BlockExists(Vector3 pos)
    {
        Collider[] hits = Physics.OverlapSphere(pos, 0.4f);

        foreach (var h in hits)
        {
            if (h.CompareTag("block"))
                return true;
        }
        return false;
    }

    System.Collections.IEnumerator JumpAnimation(Vector3 target)
    {
        isJumping = true;
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / jumpTime;
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            transform.position = Vector3.Lerp(start, target, t) + Vector3.up * height;

            yield return null;
        }

        transform.position = target;

        onBlock = true;
        arrowObject.SetActive(true);

        isJumping = false;
    }
    public void KillPlayer()
    {
        // ukryj strza³kê
        arrowObject.SetActive(false);

        // œmieræ w dó³
        StartCoroutine(JumpIntoVoid(transform.position + Vector3.down * 3f));
    }
    public void ResetPlayerState()
    {
        
        isJumping = false; 
                           
    }
    System.Collections.IEnumerator JumpIntoVoid(Vector3 target)
    {
        isJumping = true;
        falling = true;

        Vector3 start = transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / (jumpTime * 1.2f);

            // mniejszy ³uk — wygl¹da bardziej jak upadek
            float height = Mathf.Sin(t * Mathf.PI) * (jumpHeight * 0.5f);

            transform.position = Vector3.Lerp(start, target, t) + Vector3.up * height;

            yield return null;
        }

        // po animacji — wyœwietlamy Game Over
        gameOverUI.SetActive(true);

        // mo¿na zablokowaæ sterowanie
        Time.timeScale = 0.3f; // delikatne zwolnienie (opcjonalne)
    }
}

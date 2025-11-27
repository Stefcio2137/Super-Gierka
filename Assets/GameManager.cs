using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f; // przywrócenie czasu jeœli by³ spowolniony
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

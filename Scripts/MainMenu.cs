using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Muat scene berikutnya
    }

    public void Quit()
    {
        Application.Quit(); // Keluar dari aplikasi
    }
}

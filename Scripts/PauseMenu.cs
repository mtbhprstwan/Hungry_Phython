using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // UI untuk menu pause
    [SerializeField] private AudioSource[] music; // Array AudioSource untuk musik

    private bool isPaused = false; // Status pause
    private bool isMusicOn = true; // Status musik aktif

    void Start()
    {
        // Baca status musik terakhir dari PlayerPrefs saat game dimuat
        isMusicOn = PlayerPrefs.GetInt("IsMusicOn", 1) == 1; // Default: musik aktif
        UpdateMusicStatus(); // Terapkan status musik terakhir
    }

    void Update()
    {
        // Tambahkan input untuk mem-pause/unpause musik jika tombol tertentu ditekan
        if (Input.GetKeyDown(KeyCode.M)) // Contoh: tekan M untuk mem-pause/unpause musik
        {
            if (isPaused)
            {
                UnpauseMusic(); // Unpause musik
            }
            else
            {
                PauseMusic(); // Pause musik
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Tampilkan menu pause
        Time.timeScale = 0f; // Berhenti berjalan
        isPaused = true; // Set status pause
    }

    public void Continue()
    {
        pauseMenuUI.SetActive(false); // Sembunyikan menu pause
        Time.timeScale = 1f; // Kembalikan ke waktu normal
        isPaused = false; // Set status tidak pause
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Kembalikan ke waktu normal sebelum merestart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Muat ulang scene saat ini
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Kembalikan ke waktu normal sebelum ke menu utama
        SceneManager.LoadScene("MainMenu"); // Muat scene menu utama
    }

    public void PauseMusic()
    {
        isMusicOn = false; // Matikan musik
        UpdateMusicStatus(); // Terapkan status musik terbaru
    }

    public void UnpauseMusic()
    {
        isMusicOn = true; // Hidupkan musik
        UpdateMusicStatus(); // Terapkan status musik terbaru
    }

    private void UpdateMusicStatus()
    {
        int musicOnInt = isMusicOn ? 1 : 0; // Konversi status musik ke integer
        PlayerPrefs.SetInt("IsMusicOn", musicOnInt); // Simpan status musik ke PlayerPrefs
        PlayerPrefs.Save();

        // Terapkan status musik ke semua AudioSource dalam array
        foreach (var audioSource in music)
        {
            audioSource.volume = isMusicOn ? 1f : 0f; // Atur volume berdasarkan status musik
        }
    }
}

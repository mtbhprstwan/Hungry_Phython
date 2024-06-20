using System;
using System.Collections.Generic;
using UnityEngine;


using System;
// Menggunakan System untuk event Action
using UnityEngine.SceneManagement;
// Menggunakan UnityEngine.SceneManagement untuk mengakses manajemen scene
using UnityEngine.UI;
// Menggunakan UnityEngine.UI untuk mengakses UI elements seperti Text

[RequireComponent(typeof(BoxCollider2D))]
// Menyatakan bahwa komponen ini membutuhkan BoxCollider2D agar dapat berfungsi
public class Snake : GameEntity
{
    public Transform segmentPrefab;
    // Prefab untuk segmen tubuh ular
    public float speed = 20f;
    // Kecepatan pergerakan ular
    public float speedMultiplier = 1f;
    // Pengganda kecepatan ular
    public int initialSize = 4;
    // Ukuran awal ular
    public GameObject gameOverPanel;
    // Panel yang muncul saat permainan berakhir
    public List<GameObject> gameElementsToHide = new List<GameObject>();
    // Daftar elemen permainan yang harus disembunyikan saat permainan berakhir

    public AudioSource eatSFX;
    // SFX saat ular memakan makanan
    public AudioSource dieSFX;
    // SFX saat ular mati
    public AudioSource bgm;
    // Background music saat permainan berjalan
    public AudioSource dieBGM;
    // Background music saat permainan berakhir

    private List<Transform> segments = new List<Transform>();
    // List untuk menyimpan segmen-segmen tubuh ular
    private List<Vector2Int> directions = new List<Vector2Int>();
    // List untuk menyimpan arah dari setiap segmen tubuh ular
    private Vector2Int direction = Vector2Int.right;
    // Arah awal dari ular
    private Vector2Int input;
    // Input arah dari pemain
    private float nextUpdate;
    // Waktu kapan update selanjutnya akan dilakukan
    private bool isGameOver = false;
    // Status apakah permainan sudah berakhir atau belum

    public int score = 0;
    // Skor yang didapat saat bermain
    public int highScore = 0;
    // Skor tertinggi yang pernah dicapai
    private Text scoreText;
    // Text UI untuk menampilkan skor saat bermain
    public Text gameOverScoreText;
    // Text UI untuk menampilkan skor saat permainan berakhir

    public event Action OnResetState;
    // Event yang dipanggil saat mengatur ulang status permainan

    private void Start()
    {
        Initialize();
        // Memanggil fungsi Initialize saat permainan dimulai
    }

    public override void Initialize()
    {
        ResetState();
        // Mengatur ulang status permainan saat inisialisasi
        gameOverPanel.SetActive(false);
        // Menyembunyikan panel game over saat inisialisasi
        bgm.Play();
        // Memainkan background music saat inisialisasi
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // Mendapatkan komponen Text untuk menampilkan skor
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        // Mendapatkan skor tertinggi dari PlayerPrefs
    }

    private void Update()
    {
        if (isGameOver)
        {
            scoreText.text = score.ToString();
            return;
        }

        // Memeriksa input dari pemain dan mengubah arah ular sesuai dengan input
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                input = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                input = Vector2Int.down;
            }
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                input = Vector2Int.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = Vector2Int.left;
            }
        }

        scoreText.text = score.ToString();
        // Memperbarui UI text untuk menampilkan skor saat bermain
    }

    private void FixedUpdate()
    {
        UpdateEntity();
        // Memanggil fungsi UpdateEntity pada setiap fixed frame
    }

    public override void UpdateEntity()
    {
        if (isGameOver || Time.time < nextUpdate)
        {
            return;
        }

        if (input != Vector2Int.zero)
        {
            direction = input;
        }

        // Memperbarui posisi dari setiap segmen tubuh ular
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
            directions[i] = directions[i - 1];
        }

        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;
        transform.position = new Vector2(x, y);

        directions[0] = direction;

        // Memutar setiap segmen tubuh ular sesuai dengan arahnya
        for (int i = 0; i < segments.Count; i++)
        {
            RotateSegment(segments[i], directions[i]);
        }

        // Memeriksa jika ular bertabrakan dengan dirinya sendiri
        for (int i = 1; i < segments.Count; i++)
        {
            if (segments[i].position == transform.position)
            {
                GameOver();
                return;
            }
        }

        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
        // Menjadwalkan update selanjutnya berdasarkan kecepatan ular
    }

    private void RotateSegment(Transform segment, Vector2Int direction)
    {
        // Memutar segmen tubuh ular sesuai dengan arahnya
        if (direction == Vector2Int.right)
        {
            segment.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2Int.left)
        {
            segment.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2Int.up)
        {
            segment.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2Int.down)
        {
            segment.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    public void Grow()
    {
        // Menambahkan segmen tubuh baru pada ular
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
        directions.Add(directions[segments.Count - 2]);

        gameElementsToHide.Add(segment.gameObject);
        eatSFX.Play();

        score++;
        // Menambahkan skor saat ular makan makanan
    }

    public void ResetState()
    {
        // Mengatur ulang status permainan dan segala atribut terkait
        direction = Vector2Int.right;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // Menghapus segmen tubuh ular yang sudah ada
        for (int i = segments.Count - 1; i > 0; i--)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        directions.Clear();
        directions.Add(direction);

        // Menambahkan segmen tubuh awal untuk ular
        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }

        isGameOver = false;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;

        // Menampilkan kembali elemen-elemen permainan yang disembunyikan saat permainan berakhir
        foreach (GameObject element in gameElementsToHide)
        {
            if (element != null)
            {
                element.SetActive(true);
            }
        }

        bgm.Play();
        // Memainkan kembali background music setelah mengatur ulang permainan
        OnResetState?.Invoke();
        // Memanggil event OnResetState jika tidak null
    }

    public override void OnCollision()
    {
        // Dipanggil saat terjadi tabrakan antara ular dengan objek lain
        GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver)
        {
            return;
        }

        // Memeriksa tag dari objek yang ditabrak ular
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Wall"))
        {
            OnCollision();
        }
    }

    private void GameOver()
    {
        // Menangani kondisi saat permainan berakhir
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        dieSFX.Play();
        dieBGM.Play();
        bgm.Stop();

        // Menyembunyikan elemen-elemen permainan saat permainan berakhir
        foreach (GameObject element in gameElementsToHide)
        {
            if (element != null)
            {
                element.SetActive(false);
            }
        }
        // Menampilkan skor akhir saat permainan berakhir
        gameOverScoreText.text = "Your Score: " + score + "\nHigh Score: " + highScore;

        // Memperbarui skor tertinggi jika skor saat ini lebih tinggi
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Menghapus segmen tubuh ular yang masih tersisa
        for (int i = segments.Count - 1; i > 0; i--)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        // Mengosongkan list segmen tubuh ular

        // Reset skor saat permainan berakhir
        score = 0;
    }

    public bool Occupies(int x, int y)
    {
        // Memeriksa apakah ular menduduki posisi (x, y)
        foreach (Transform segment in segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x && Mathf.RoundToInt(segment.position.y) == y)
            {
                return true;
            }
        }
        return false;
    }
}




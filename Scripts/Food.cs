using UnityEngine;

// Menggunakan UnityEngine untuk mengakses kelas-kelas dari Unity
[RequireComponent(typeof(BoxCollider2D))]
// Menyatakan bahwa komponen ini membutuhkan BoxCollider2D agar dapat berfungsi
public class Food : GameEntity
{
    public Collider2D gridArea;
    // Variabel untuk menyimpan area kolider yang menjadi batasan penempatan makanan
    private Snake snake;
    // Variabel untuk menyimpan referensi ke objek Snake

    private void Awake()
    {
        // Dipanggil saat objek pertama kali diinisialisasi
        snake = FindObjectOfType<Snake>();
        // Mencari objek Snake yang ada di dalam scene
        snake.OnResetState += RandomizePosition;
        // Menghubungkan fungsi RandomizePosition dengan event OnResetState dari objek Snake
    }

    private void Start()
    {
        // Dipanggil sebelum Update pertama kali
        Initialize();
        // Memanggil fungsi Initialize untuk menginisialisasi posisi awal makanan
    }

    public override void Initialize()
    {
        // Fungsi untuk menginisialisasi objek (override dari kelas GameEntity)
        RandomizePosition();
        // Memanggil fungsi RandomizePosition untuk menentukan posisi awal makanan
    }

    public override void UpdateEntity()
    {
        // UpdateEntity tidak melakukan apa-apa karena makanan tidak perlu diupdate setiap frame
        // Implementasi kosong
    }

    public override void OnCollision()
    {
        // Dipanggil saat terjadi tabrakan dengan objek lain
        RandomizePosition();
        // Memanggil fungsi RandomizePosition untuk mengubah posisi makanan setelah terjadi tabrakan
    }

    public void RandomizePosition()
    {
        // Fungsi untuk menentukan posisi acak baru untuk makanan di dalam area yang ditentukan
        Bounds bounds = gridArea.bounds;
        // Mengambil batas area dari gridArea

        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
        // Mengambil koordinat x dan y secara acak di dalam batas area

        while (snake.Occupies(x, y))
        {
            // Selama posisi yang dihasilkan sudah ditempati oleh Snake, mencari posisi baru
            x++;

            if (x > bounds.max.x)
            {
                x = Mathf.RoundToInt(bounds.min.x);
                y++;

                if (y > bounds.max.y)
                {
                    y = Mathf.RoundToInt(bounds.min.y);
                }
            }
        }

        transform.position = new Vector2(x, y);
        // Menetapkan posisi baru makanan sesuai dengan koordinat yang sudah dihitung
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Dipanggil saat makanan bersentuhan dengan objek lain (trigger)
        OnCollision();
        // Memanggil fungsi OnCollision untuk menentukan posisi baru makanan setelah terjadi trigger
    }
}

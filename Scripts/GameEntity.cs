using UnityEngine;

// Menggunakan UnityEngine untuk mengakses kelas-kelas dari Unity

public abstract class GameEntity : MonoBehaviour
{
    // Kelas abstrak GameEntity yang merupakan turunan dari MonoBehaviour

    public abstract void Initialize();
    // Metode abstrak untuk menginisialisasi objek turunan

    public abstract void UpdateEntity();
    // Metode abstrak untuk memperbarui perilaku objek turunan

    public abstract void OnCollision();
    // Metode abstrak yang dipanggil saat terjadi tabrakan dengan objek lain
}

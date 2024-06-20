using System.Collections.Generic;
using UnityEngine;

// Menggunakan System.Collections.Generic untuk menggunakan List
// Menggunakan UnityEngine untuk mengakses kelas-kelas dari Unity

public class GameManager : MonoBehaviour
{
    private List<GameEntity> gameEntities;
    // List untuk menyimpan semua objek yang merupakan turunan dari GameEntity

    private void Start()
    {
        // Dipanggil saat permainan dimulai
        gameEntities = new List<GameEntity>(FindObjectsOfType<GameEntity>());
        // Mengisi gameEntities dengan semua objek GameEntity yang ada di dalam scene

        foreach (var entity in gameEntities)
        {
            entity.Initialize();
            // Memanggil fungsi Initialize untuk setiap objek GameEntity dalam gameEntities
        }
    }

    private void Update()
    {
        // Dipanggil setiap frame
        foreach (var entity in gameEntities)
        {
            entity.UpdateEntity();
            // Memanggil fungsi UpdateEntity untuk setiap objek GameEntity dalam gameEntities
        }
    }
}

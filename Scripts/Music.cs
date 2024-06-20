using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource music; // Komponen AudioSource untuk musik

    public void OnMusic()
    {
        music.Play(); // Mainkan musik
    }

    public void OffMusic()
    {
        music.Stop(); // Hentikan musik
    }
}

using System.Collections.Generic; // Używane dla list
using UnityEngine;

public class PlaylistManager : MonoBehaviour
{
    public List<AudioClip> playlist; // Lista utworów
    public AudioSource audioSource; // Komponent AudioSource
    public bool loopPlaylist = true; // Czy zapętlać całą playlistę

    private int currentTrackIndex = 0; // Indeks obecnego utworu

    void Start()
    {
        if (playlist.Count > 0 && audioSource != null)
        {
            PlayTrack(currentTrackIndex); // Zacznij od pierwszego utworu
        }
    }

    void Update()
    {
        // Sprawdź, czy utwór się skończył
        if (!audioSource.isPlaying && playlist.Count > 0)
        {
            NextTrack(); // Przełącz na kolejny utwór
        }
    }

    void PlayTrack(int index)
    {
        if (index < 0 || index >= playlist.Count) return; // Ochrona przed błędami indeksu

        audioSource.clip = playlist[index]; // Ustaw utwór
        audioSource.Play(); // Odtwarzaj
    }

    void NextTrack()
    {
        currentTrackIndex++;

        // Jeśli dotarliśmy do końca listy
        if (currentTrackIndex >= playlist.Count)
        {
            if (loopPlaylist)
            {
                currentTrackIndex = 0; // Wróć do pierwszego utworu
            }
            else
            {
                return; // Zatrzymaj odtwarzanie, jeśli nie zapętlamy
            }
        }

        PlayTrack(currentTrackIndex); // Odtwórz kolejny utwór
    }
}
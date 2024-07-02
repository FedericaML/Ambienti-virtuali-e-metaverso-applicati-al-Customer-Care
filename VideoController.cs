using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        // Ottieni il componente VideoPlayer dalla lavagna
        videoPlayer = GetComponent<VideoPlayer>();
        
        // Assicurati che il video non parta automaticamente all'inizio
        videoPlayer.playOnAwake = false;
    }

    void Update()
    {
        // Controlla se il tasto Invio (Return) viene premuto
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Se il video è in riproduzione, mettilo in pausa
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            // Altrimenti, avvia il video
            else
            {
                videoPlayer.Play();
            }
        }
    }
}

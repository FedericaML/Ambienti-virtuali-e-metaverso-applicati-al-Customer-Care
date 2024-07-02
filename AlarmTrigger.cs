using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    private bool isPlayerNear = false;
    public AudioClip alarmSound;
    private AudioSource audioSource;

    void Start()
    {
        // Aggiungi un AudioSource se non esiste già
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = alarmSound;

        // Debug: verifica se l'audio è impostato correttamente
        if (audioSource.clip != null)
        {
            Debug.Log("Audio source is set up correctly");
        }
        else
        {
            Debug.Log("Audio source setup failed");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is near the button");
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the button");
            isPlayerNear = false;
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed, playing alarm sound");
            audioSource.Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giocatoreDinamica: MonoBehaviour
{
    public float speed = 5f; // Velocità di movimento del giocatore
    public float rotationSpeed = 100f; // Velocità di rotazione del giocatore
    public float stepHeight = 1.0f; // Altezza degli scalini
    private Transform faccia; // Riferimento alla telecamera figlia

    private float currentHeight = 0.0f; // Altezza attuale del giocatore

    void Start()
    {
        // Trova il riferimento alla telecamera figlia
        faccia = transform.Find("faccia");
        if (faccia == null)
        {
            Debug.LogError("La telecamera 'faccia' non è stata trovata come figlia del giocatore.");
        }
    }

    void Update()
    {
        if (faccia == null) return; // Evita ulteriori errori se la telecamera non è stata trovata

        HandleMovement();
        HandleRotation();
        HandleStairs();
    }

    void HandleMovement()
    {
        // Ottieni l'input dell'utente per il movimento orizzontale e verticale
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D o frecce sinistra/destra
        float moveVertical = Input.GetAxis("Vertical"); // W/S o frecce su/giù

        // Crea il vettore di direzione per il movimento
        Vector3 direction = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Muovi il giocatore
        transform.Translate(direction * speed * Time.deltaTime, Space.Self);
    }

    void HandleRotation()
    {
        // Ottieni l'input del mouse per la rotazione
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        // Ruota il giocatore sull'asse Y (orizzontale)
        transform.Rotate(0, mouseX, 0);
    }

    void HandleStairs()
    {
        // Controllo per salire le scale con il tasto Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Aumenta l'altezza attuale
            currentHeight += stepHeight;
        }

        // Controllo per scendere le scale con il tasto R
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Diminuisci l'altezza attuale
            currentHeight -= stepHeight;

            // Assicurati che l'altezza non sia negativa
            if (currentHeight < 0.0f)
            {
                currentHeight = 0.0f;
            }
        }

        // Aggiorna la posizione del giocatore in base all'altezza attuale
        Vector3 position = transform.position;
        position.y = currentHeight;
        transform.position = position;
    }
}

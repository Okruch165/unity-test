using UnityEngine;
using TMPro; // Importuj przestrzeń nazw TextMeshPro

public class ItemHighlight : MonoBehaviour
{
    public float interactionRange = 3f; // Zasięg interakcji
    private GameObject currentHighlightedObject;
    public TMP_Text interactionText; // Referencja do komponentu TextMeshPro

    void Start()
    {
        // Ukryj tekst na początku
        if (interactionText != null)
        {
            interactionText.text = ""; // Pusty tekst
        }
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Sprawdzanie, czy raycast trafił w obiekt
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Sprawdzenie, czy obiekt ma komponent Interaction
            Interaction newInteractionScript = hitObject.GetComponent<Interaction>();

            if (newInteractionScript != null)
            {
                // Wyświetlenie komunikatu
                if (interactionText != null)
                {
                    interactionText.text = "Kliknij E, aby wejść w interakcję"; 
                }

                // Dodaj opcję interakcji, np. naciśnięcie "E"
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Wykonaj interakcję
                    newInteractionScript.Interact();
                }
            }
        }
        else
        {
            // Ukryj tekst, gdy nie ma obiektu
            if (interactionText != null)
            {
                interactionText.text = ""; 
            }
            currentHighlightedObject = null;
        }
    }
}

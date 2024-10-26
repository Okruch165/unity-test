using UnityEngine;
using TMPro; // Importuj przestrzeń nazw TextMeshPro
using UnityEngine.UI; // Importuj przestrzeń nazw dla UI

public class ItemHighlight : MonoBehaviour
{
    public float interactionRange = 3f; // Zasięg interakcji
    public TMP_Text interactionText; // Referencja do komponentu TextMeshPro
    public GameObject optionsPanel; // Referencja do panelu z opcjami

    void Start()
    {
        // Ukryj tekst na początku
        if (interactionText != null)
        {
            interactionText.text = ""; // Pusty tekst
        }

        // Ukryj panel z opcjami
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false); // Ukryj panel na początku
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
                    interactionText.text = "Kliknij E, aby wybrać opcję"; 
                }

                // Dodaj opcję interakcji, np. naciśnięcie "E"
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Wykonaj interakcję
                    newInteractionScript.Interact();
                    // Pokaż panel z opcjami
                    if (optionsPanel != null)
                    {
                        optionsPanel.SetActive(true);
                    }
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
        }
    }

    // Funkcja do zamykania panelu opcji (możesz przypisać ją do przycisku)
    public void CloseOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }
}

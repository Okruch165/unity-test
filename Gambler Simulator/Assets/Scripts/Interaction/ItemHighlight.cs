using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    public float interactionRange = 3f; // Zasięg interakcji
    private GameObject currentHighlightedObject;
    private Interaction interactionScript;

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
                // Jeśli nowy obiekt jest różny od obecnie podświetlonego
                if (currentHighlightedObject != hitObject)
                {
                    // Wyłączenie podświetlenia poprzedniego obiektu
                    if (currentHighlightedObject != null)
                    {
                        interactionScript.ToggleOutline(false);
                    }

                    // Przełączenie na nowy obiekt
                    currentHighlightedObject = hitObject;
                    interactionScript = newInteractionScript;
                    interactionScript.ToggleOutline(true);
                }
            }
        }
        else if (currentHighlightedObject != null)
        {
            // Wyłączenie podświetlenia, jeśli nie trafiono w obiekt
            interactionScript.ToggleOutline(false);
            currentHighlightedObject = null;
            interactionScript = null;
        }
    }
}

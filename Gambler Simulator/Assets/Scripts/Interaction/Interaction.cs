using UnityEngine;

public class Interaction : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow; // Kolor podświetlenia

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    public void ToggleOutline(bool shouldHighlight)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = shouldHighlight ? highlightColor : originalColor;
        }
    }
}

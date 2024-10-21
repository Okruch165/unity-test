using UnityEngine;

public class NumberGenerator : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Random number: " + Random.Range(0, 100));
    }
}

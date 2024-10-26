using UnityEngine;

public class Interaction : MonoBehaviour
{
    public void Interact()
    {
        // Here you can define what happens when the object is interacted with.
        Debug.Log("Interacted with " + gameObject.name);
    }
}

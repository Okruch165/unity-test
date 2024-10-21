using UnityEngine;
using UnityEngine.UI;

    interface IInteractable {
        public void Interact();
    }


public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractorRange;

    private void Start()
    {
        
    }
    
void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractorRange))
            {
                Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name); 
                Debug.DrawRay(InteractorSource.position, InteractorSource.forward * InteractorRange, Color.red); 
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
            else
            {
                Debug.Log("No hit within range."); 
            }
        }
    }
}

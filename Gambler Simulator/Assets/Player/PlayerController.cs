using UnityEngine;
public class PlayerController : MonoBehaviour


{
    public Camera playerCamera;
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float verticalLookLimit = 80f;

    private float rotationX = 0f;

    void Start()
    {
        // Zablokowanie kursora w oknie gry
        Cursor.lockState = CursorLockMode.Locked;  // Blokuje kursor w oknie gry
        Cursor.visible = false;  // Ukrywa kursor
    }

    void Update()
    {
        // Ruch gracza (kuli)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = transform.TransformDirection(movement);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Obrót kamery (rozglądanie)
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void OnApplicationFocus(bool focus)
    {
        // Jeśli aplikacja straci fokus, odblokuj kursor
        if (!focus)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
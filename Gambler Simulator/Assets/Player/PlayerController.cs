using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera; // Kamera przypisana do gracza
    public float moveSpeed = 5f; // Prędkość poruszania się
    public float lookSpeed = 2f; // Prędkość obrotu kamery
    public float verticalLookLimit = 80f; // Limit patrzenia w górę/dół

    private float rotationX = 0f; // Aktualny kąt patrzenia w górę/dół

    void Start()
    {
        // Zablokowanie kursora w oknie gry
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement(); // Obsługa ruchu gracza
        HandleCamera();   // Obsługa obrotu kamery
    }

    void HandleMovement()
    {
        // Odczyt wejścia z klawiatury
        float horizontal = Input.GetAxis("Horizontal"); // A/D - lewo/prawo
        float vertical = Input.GetAxis("Vertical");     // W/S - przód/tył

        // Tworzenie wektora ruchu z poprawnymi osiami
        Vector3 movement = new Vector3(horizontal, 0, vertical); // Ruch w lokalnej osi XZ
        movement = transform.TransformDirection(movement);      // Konwersja na globalne osie

        // Przesunięcie gracza
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleCamera()
    {
        // Odczyt ruchu myszą
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed; // Obrót w poziomie (oś Y/Z)
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed; // Obrót w pionie (oś X)

        // Obrót w osi X (góra/dół) dla kamery
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // Obrót tylko kamery
        }

        // Obrót gracza w osi Y (oś zielona - Z dla modelu)
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

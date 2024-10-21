using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Użyj playerBody, aby zachować konwencję

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Zablokowanie kursora
    }

    void Update()
    {
        // Odczytanie ruchu myszy
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Obrót w pionie
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Ograniczenie ruchu w pionie

        // Obrót w poziomie
        playerBody.Rotate(Vector3.up * mouseX); // Obrót obiektu gracza
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Ustawienie lokalnej rotacji kamery
    }
}
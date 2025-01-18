using UnityEngine;

public class InteractableObjectCoinFlip : MonoBehaviour
{
    public GameObject player;  // Gracz
    public float interactDistance = 3f;  // Odległość, w jakiej gracz może wchodzić w interakcję
    private bool isNear = false;  // Czy gracz jest blisko obiektu?
    private bool isLookingAt = false;  // Czy gracz patrzy na obiekt?
    private bool isGameActive = false; // Flaga, czy gra jest aktywna

    public CoinFlip CoinFlipGame; // Referencja do gry Rock-Paper-Scissors

    private void Update()
    {
        // Sprawdź, czy gracz jest w zasięgu
        float distance = Vector3.Distance(transform.position, player.transform.position);
        isNear = distance <= interactDistance;

        // Wykonaj Raycast z pozycji kamery gracza
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Sprawdź, czy Raycast trafia na ten obiekt
            isLookingAt = hit.collider != null && hit.collider.gameObject == gameObject;
        }
        else
        {
            isLookingAt = false;
        }

        // Jeśli gracz jest blisko, patrzy na obiekt i gra nie jest aktywna, umożliwiamy interakcję
        if (isNear && isLookingAt && Input.GetKeyDown(KeyCode.E) && !isGameActive)
        {
            StartGame(); // Start the Rock-Paper-Scissors game
        }
    }

    void OnGUI()
    {
        // Wyświetl tekst tylko, gdy gracz jest blisko i patrzy na obiekt
        if (isNear && isLookingAt)
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Press 'E' to start Coin Flip");
        }
    }

    // Metoda uruchamiająca wybraną grę
    void StartGame()
    {
        if (isGameActive)
        {
            Debug.Log("A game is already active. Please finish the current game first.");
            return;
        }

        if (CoinFlipGame != null && !CoinFlipGame.isGameActive)
        {
            isGameActive = true; // Set the game as active
            CoinFlipGame.StartGame(); // Start the Rock-Paper-Scissors game
        }
    }

    // Call this method to reset the game state when a game ends
    public void EndGame()
    {
        isGameActive = false; // Reset the game active flag
    }
}

using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject player;  // Gracz
    public float interactDistance = 3f;  // Odległość, w jakiej gracz może wchodzić w interakcję
    private bool isNear = false;  // Czy gracz jest blisko obiektu?
    private bool isLookingAt = false;  // Czy gracz patrzy na obiekt?

    public RockPaperScissors rockPaperScissorsGame;  // Referencja do gry Papier, Kamień, Nożyce

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

        // Jeśli gracz jest blisko i patrzy na obiekt, umożliwiamy interakcję
        if (isNear && isLookingAt && Input.GetKeyDown(KeyCode.E))
        {
            StartRockPaperScissorsGame();
        }
    }

    void OnGUI()
    {
        // Wyświetl tekst tylko, gdy gracz jest blisko i patrzy na obiekt
        if (isNear && isLookingAt)
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Press 'E' to play Rock-Paper-Scissors");
        }
    }

    // Metoda uruchamiająca grę
    void StartRockPaperScissorsGame()
    {
        if (rockPaperScissorsGame != null)
        {
            // Uruchomienie gry i pokazanie przycisków
            rockPaperScissorsGame.StartGame();
        }
    }
}
xd
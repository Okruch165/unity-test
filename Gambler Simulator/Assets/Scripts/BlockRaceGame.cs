using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockRaceGame : MonoBehaviour
{
    public Canvas Canvas;
    public GameObject[] rats; // Tablica szczurów
    public GameObject finishLine; // Obiekt mety
    public float minSpeed = 1f; // Minimalna prędkość szczurów
    public float maxSpeed = 5f; // Maksymalna prędkość szczurów
    public Text resultText; // Tekst do wyświetlenia wyniku
    public MoneyManager moneyManager; // Referencja do MoneyManager
    private int playerBet; // Zakład gracza na szczura
    public int betCost = 50; // Koszt zakładu
    public int winPayout = 200; // Wygrana za trafienie
    public Text instructionText; // Tekst instrukcji
    private bool hasWinningRatReachedFinishLine = false; // Flaga, czy szczur gracza dotarł do mety
    public Vector3[] startPositions; // Tablica pozycji startowych dla szczurów

    private bool isRaceActive = false; // Flaga, czy wyścig jest aktywny

    private void Start()
    {
        Canvas.enabled = false; // Disable the canvas at the start
        instructionText.text = "Press E to start betting!"; // Wyświetl instrukcję na starcie
        moneyManager = Object.FindFirstObjectByType<MoneyManager>(); // Znajdź MoneyManager w scenie

    }

    public void StartBettingProcess() // Changed to public
    { // Wyświetl instrukcję w konsoli
        if (Canvas != null)
        {
            Canvas.gameObject.SetActive(true);
            resultText.gameObject.SetActive(false);
        }
        instructionText.text = "Press 1, 2, 3, or 4 to bet on a rat!"; // Wyświetl instrukcję na ekranie
        StartCoroutine(WaitForBetInput()); // Rozpocznij oczekiwanie na wybór gracza

    }

    public void StartRace(int bet)
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (isRaceActive) // Sprawdź, czy wyścig już trwa
        {
            Debug.Log("Race is already active.");
            return; // Jeśli wyścig jest aktywny, zakończ funkcję
        }

        Debug.Log("Current money: " + moneyManager.money); // Debugging statement to check current money
        if (moneyManager.money < betCost)
        {
            resultText.text = "Not enough money to place a bet!"; // Brak wystarczających środków
            resultText.gameObject.SetActive(true); // Show the result text
            return;
        }

        playerBet = bet; // Ustaw zakład gracza
        hasWinningRatReachedFinishLine = false; // Zresetuj flagę
        moneyManager.SubtractMoney(betCost); // Odejmij koszt zakładu
        instructionText.text = ""; // Wyczyść tekst instrukcji
        resultText.gameObject.SetActive(false); // Hide the result text at the start of the race
        isRaceActive = true; // Zaktualizuj flagę aktywności wyścigu
        StartCoroutine(Race()); // Rozpocznij wyścig
    }

    private IEnumerator Race()
    {
        // Ustaw pozycje startowe dla szczurów
        for (int i = 0; i < rats.Length; i++)
        {
            rats[i].transform.position = startPositions[i]; // Ustaw pozycję startową z tablicy
        }

        // Poruszaj szczurami do przodu z losową prędkością
        while (true)
        {
            for (int i = 0; i < rats.Length; i++)
            {
                float speed = Random.Range(minSpeed, maxSpeed); // Losowa prędkość
                rats[i].transform.Translate(Vector3.forward * speed * Time.deltaTime); // Porusz szczura

                // Sprawdź, czy szczur gracza dotarł do mety
                if (i == playerBet - 1 && rats[i].transform.position.z >= finishLine.transform.position.z)
                {
                    hasWinningRatReachedFinishLine = true;
                }
            }

            // Sprawdź, czy któryś szczur dotarł do mety
            for (int i = 0; i < rats.Length; i++)
            {
                if (rats[i].transform.position.z >= finishLine.transform.position.z) // Sprawdź, czy szczur dotarł do mety
                {
                    DetermineWinner(i); // Określ zwycięzcę
                    yield break; // Zakończ wyścig
                }
            }

            yield return null; // Poczekaj na następną klatkę
        }
    }

    private void DetermineWinner(int winningRatIndex)
    {
        string resultMessage = "Rat " + (winningRatIndex + 1) + " wins!"; // Wyświetl, który szczur wygrał

        if (winningRatIndex == playerBet - 1 && hasWinningRatReachedFinishLine)
        {
            moneyManager.AddMoney(winPayout); // Dodaj wygraną
            resultMessage += " You win!"; // Wyświetl komunikat o wygranej
        }
        else
        {
            resultMessage += " You lose!"; // Wyświetl komunikat o przegranej
        }

        resultText.text = resultMessage; // Update the result text
        resultText.gameObject.SetActive(true); // Show the result text

        // Delay the end of the game to allow the player to see the result
        StartCoroutine(DelayedEndGame());
    }

    private IEnumerator DelayedEndGame()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds before ending the game
        EndGame(); // Zakończ grę po wyłonieniu zwycięzcy
    }

    public void EndGame()
    {
        isRaceActive = false; // Zaktualizuj flagę, by wyścig nie był aktywny
        Canvas.enabled = false; // Ukryj canvas
        hasWinningRatReachedFinishLine = false; // Reset the winning rat flag
        Debug.Log("Block Race Game has ended. Flags reset."); // Debug log for game reset
        

        // Notify the InteractableObjectBlockRace to reset its state
        InteractableObjectBlockRace interactableObject = Object.FindFirstObjectByType<InteractableObjectBlockRace>();
        if (interactableObject != null)
        {
            interactableObject.EndGame();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator WaitForBetInput()
    {
        bool betPlaced = false;

        while (!betPlaced)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // Gracz wybrał szczura 1
            {
                StartRace(1); // Rozpocznij wyścig z wybranym szczurem
                betPlaced = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // Gracz wybrał szczura 2
            {
                StartRace(2);
                betPlaced = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) // Gracz wybrał szczura 3
            {
                StartRace(3);
                betPlaced = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) // Gracz wybrał szczura 4
            {
                StartRace(4);
                betPlaced = true;
            }

            yield return null; // Poczekaj na następną klatkę
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockRaceGame : MonoBehaviour
{
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

    private void Start()
    {
        instructionText.text = "Press E to start betting!"; // Wyświetl instrukcję na starcie
        moneyManager = FindObjectOfType<MoneyManager>(); // Znajdź MoneyManager w scenie
    }

    private void StartBettingProcess()
    {
        Debug.Log("Press 1, 2, 3, or 4 to bet on a rat!"); // Wyświetl instrukcję w konsoli
        instructionText.text = "Press 1, 2, 3, or 4 to bet on a rat!"; // Wyświetl instrukcję na ekranie
        StartCoroutine(WaitForBetInput()); // Rozpocznij oczekiwanie na wybór gracza
    }

    public void StartRace(int bet)
    {
        if (moneyManager.money < betCost)
        {
            resultText.text = "Not enough money to place a bet!"; // Brak wystarczających środków
            return;
        }

        playerBet = bet; // Ustaw zakład gracza
        hasWinningRatReachedFinishLine = false; // Zresetuj flagę
        moneyManager.SubtractMoney(betCost); // Odejmij koszt zakładu
        instructionText.text = ""; // Wyczyść tekst instrukcji
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
        resultText.text = "Rat " + (winningRatIndex + 1) + " wins!"; // Wyświetl, który szczur wygrał

        if (winningRatIndex == playerBet - 1 && hasWinningRatReachedFinishLine)
        {
            moneyManager.AddMoney(winPayout); // Dodaj wygraną resultText.text += " You win!"; // Wyświetl komunikat o wygranej
        }
        else
        {
            resultText.text += " You lose!"; // Wyświetl komunikat o przegranej
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Sprawdź, czy gracz nacisnął klawisz E
        {
            StartBettingProcess(); // Rozpocznij proces obstawiania
        }
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
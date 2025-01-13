using System;
using UnityEngine;
using TMPro;  // Dodajemy przestrzeń nazw dla TextMeshPro
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    public enum Choice { Rock, Paper, Scissors }  // Zdefiniowanie wyborów
    public Choice playerChoice;

    [Header("UI Elements")]
    public Canvas rockPaperScissorsCanvas;  // Canvas do gry
    public Text npcChoiceText;  // Tekst wyświetlający wybór NPC
    public Text resultText;     // Tekst wyświetlający wynik gry
    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;
    public TMP_InputField betInputField;  // Zmieniamy na TMP_InputField
    public Text minMaxBetText;  // Tekst pokazujący minimalną i maksymalną kwotę

    [Header("Game Settings")]
    public int minBet = 5;      // Minimalna kwota do obstawienia
    public int maxBet = 50;     // Maksymalna kwota do obstawienia
    public int rewardAmount = 30;  // Nagroda za wygraną
    public int drawAmount = 20;   // Kwota zwrotu w przypadku remisu

    private int entryFee;  // Koszt gry - zależny od wpisanej kwoty
    private MoneyManager moneyManager; // Referencja do systemu pieniędzy
    private bool isGameActive = false; // Flaga sprawdzająca, czy gra jest aktywna
    private bool hasPlayerChosen = false; // Flaga sprawdzająca, czy gracz dokonał wyboru

    private void Start()
    {
        // Znajdź MoneyManager w scenie
        moneyManager = FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager not found in the scene!");
            return;
        }

        // Ukryj Canvas na początku
        if (rockPaperScissorsCanvas != null)
        {
            rockPaperScissorsCanvas.gameObject.SetActive(false);
        }

        // Ustaw minimalną i maksymalną kwotę w tekście UI
        minMaxBetText.text = $"Min: {minBet} Max: {maxBet}";

        // Dodaj nasłuchiwanie przycisków
        rockButton.onClick.AddListener(() => SetPlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => SetPlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => SetPlayerChoice(Choice.Scissors));

        // Ustaw nasłuchiwanie na zmiany w polu tekstowym
        betInputField.onValueChanged.AddListener(OnBetInputChanged);

        // Ustawienie wartości betu na minimalną kwotę na początku
        entryFee = minBet;
    }

    private void Update()
    {
        // Debugowy klawisz - dodaj 100 pieniędzy po naciśnięciu klawisza K
        if (Input.GetKeyDown(KeyCode.K))
        {
            moneyManager.AddMoney(100);
            Debug.Log("Added 100 money for debugging. Current money: " + moneyManager.money);
        }

        // Resetowanie kasy po naciśnięciu "R"
        if (Input.GetKeyDown(KeyCode.R))
        {
            moneyManager.ResetMoney();
            moneyManager.SaveMoney();
            Debug.Log("Money reset to 100.");
        }
    }

    public void StartGame()
    {
        if (moneyManager == null || isGameActive) return;

        // Sprawdź, czy gracz ma wystarczającą ilość pieniędzy na wpisanie kwoty
        if (moneyManager.money < minBet)
        {
            Debug.Log("Not enough money to start the game!");
            ShowMessage("Not enough money to play!");
            return;
        }

        // Pokaż Canvas i odblokuj kursor
        if (rockPaperScissorsCanvas != null)
        {
            rockPaperScissorsCanvas.gameObject.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Ukryj teksty wyników na początku
        npcChoiceText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);

        // Zablokuj przyciski na razie, aby nie można było grać, dopóki nie wybierzesz stawki
        SetButtonsInteractable(false);

        // Wstrzymaj na razie wybranie opcji
        hasPlayerChosen = false;

        // Ustaw, że gra jest aktywna
        isGameActive = true;
    }

    public void SetPlayerChoice(Choice choice)
    {
        if (!isGameActive || hasPlayerChosen) return;  // Sprawdzamy, czy gra jest aktywna i czy gracz już wybrał

        playerChoice = choice;
        Debug.Log("Player chose: " + playerChoice);
        hasPlayerChosen = true;  // Gracz dokonał wyboru
        PlayGame();

        // Zablokuj przyciski po dokonaniu wyboru
        SetButtonsInteractable(false);
    }

    public void PlayGame()
    {
        if (moneyManager == null || !isGameActive) return;

        // Sprawdź, czy gracz ma wystarczającą ilość pieniędzy, aby obstawić
        if (moneyManager.money < entryFee)
        {
            Debug.Log("Not enough money to continue the game!");
            ShowMessage("Not enough money to continue the game!");
            return;
        }

        // Odejmij obstawioną kwotę od pieniędzy gracza
        moneyManager.SubtractMoney(entryFee);

        // Losowanie wyboru NPC
        Choice npcChoice = GetNpcChoice();
        npcChoiceText.text = "NPC chose: " + npcChoice;
        npcChoiceText.gameObject.SetActive(true);

        // Porównanie wyborów
        if (playerChoice == npcChoice)
        {
            ShowMessage("It's a draw!");
            moneyManager.AddMoney(drawAmount); // Zwrot części wpisowego
        }
        else if ((playerChoice == Choice.Rock && npcChoice == Choice.Scissors) ||
                 (playerChoice == Choice.Paper && npcChoice == Choice.Rock) ||
                 (playerChoice == Choice.Scissors && npcChoice == Choice.Paper))
        {
            ShowMessage("Player wins!");
            moneyManager.AddMoney(rewardAmount); // Dodaj nagrodę
        }
        else
        {
            ShowMessage("NPC wins!");
        }

        // Ukryj Canvas po zakończeniu gry
        CancelInvoke("EndGame"); // Na wszelki wypadek anuluj poprzednie wywołanie
        Invoke("EndGame", 2f);
    }

    public Choice GetNpcChoice()
    {
        Array choices = Enum.GetValues(typeof(Choice));
        System.Random rand = new System.Random();
        return (Choice)choices.GetValue(rand.Next(choices.Length));
    }

    private void ShowMessage(string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
            resultText.gameObject.SetActive(true);
        }
    }

    private void EndGame()
    {
        // Ukryj Canvas
        if (rockPaperScissorsCanvas != null)
        {
            rockPaperScissorsCanvas.gameObject.SetActive(false);
        }

        // Zablokuj kursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ukryj teksty wyników
        if (resultText != null) resultText.gameObject.SetActive(false);
        if (npcChoiceText != null) npcChoiceText.gameObject.SetActive(false);

        // Ustaw, że gra się zakończyła
        isGameActive = false;

        // Odblokuj przyciski, żeby można było wybrać ponownie
        SetButtonsInteractable(true);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        rockButton.interactable = interactable;
        paperButton.interactable = interactable;
        scissorsButton.interactable = interactable;
    }

    private void OnBetInputChanged(string betValue)
    {
        // Obsługuje zmianę wartości kwoty obstawiania
        if (int.TryParse(betValue, out int newBet))
        {
            if (newBet >= minBet && newBet <= maxBet)
            {
                entryFee = newBet;  // Zaktualizuj kwotę obstawiania
                Debug.Log($"Bet value set to {entryFee}");
                SetButtonsInteractable(true);  // Po ustawieniu betu, aktywuj przyciski
            }
            else
            {
                Debug.Log($"Bet value out of range. Valid bet is between {minBet} and {maxBet}");
                SetButtonsInteractable(false);  // Jeśli kwota jest poza zakresem, blokujemy przyciski
            }
        }
        else
        {
            Debug.Log("Invalid input.");
            SetButtonsInteractable(false);  // Jeśli wprowadzono coś, co nie jest liczbą, blokujemy przyciski
        }
    }
}

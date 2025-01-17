using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinFlip : MonoBehaviour
{
    public enum Choice {
     Heads = 1, 
     Tails 
     }
    public bool isGameActive = false;
    TextMeshProUGUI textMeshPro;
    public MoneyManager MoneyManager;
    public Canvas CoinFlipCanvas;
    public TMP_Text minMaxBetText;
    public TMP_Text resultText;
    public GameObject notEnoughMoney;
    public Button orzelButton;
    public Button reszkaButton;
    public TMP_InputField betInputField;
    public bool hasPlayerChosen = false;
    private Choice playerChoice;
    private Choice npcChoice;
    private int entryFee = 5;

    private int minBet;
    private int maxBet;
    

        [System.Obsolete]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        MoneyManager = FindObjectOfType<MoneyManager>();

                if (CoinFlipCanvas != null)
        {
            CoinFlipCanvas.gameObject.SetActive(false);
        }
        minMaxBetText.text = $"Min: {minBet} Max: {maxBet}";

        orzelButton.onClick.AddListener(() => SetPlayerChoice(Choice.Heads));
        reszkaButton.onClick.AddListener(() => SetPlayerChoice(Choice.Tails));

        betInputField.onValueChanged.AddListener(OnBetInputChanged);

        minBet = entryFee;


    }

    public void SetPlayerChoice(Choice choice)
    {
        playerChoice = choice;
        hasPlayerChosen = true;
    }
    public void SetNpcChoice(Choice choice)
    {
        npcChoice = (Random.Range(1, 2) == 1) ? Choice.Heads : Choice.Tails;
    }
    public void OnBetInputChanged(string value)
    {
        if (int.TryParse(value, out int bet))
        {
            entryFee = Mathf.Clamp(bet, minBet, maxBet);
        }
    }

    public void StartGame()
    {
        if (isGameActive)
        {
            Debug.Log("A game is already active. Please finish the current game first.");
            return;
        }
    }
    public void PlayGame()
    {
        if(MoneyManager.money < minBet)
        {
            notEnoughMoney.SetActive(true);
            return;
        }
        MoneyManager.AddMoney(-entryFee);

        if ( playerChoice == Choice.Heads && npcChoice == Choice.Heads)
        {
            resultText.text = "Heads! You win!";
            MoneyManager.AddMoney(entryFee * 2);
        }
        else if (playerChoice == Choice.Tails && npcChoice == Choice.Tails)
        {
            resultText.text = "Tails! You win!";
            MoneyManager.AddMoney(entryFee * 2);
        }
        else
        {
            resultText.text = "You lose!";
        }
    }
    
    public void EndGame()
    {
        isGameActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

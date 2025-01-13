using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockRaceGame : MonoBehaviour
{
    public GameObject[] rats; // Array of rats
    public float minSpeed = 1f; // Minimum speed of the rats
    public float maxSpeed = 5f; // Maximum speed of the rats
    public Text resultText; // UI Text to display the result
    public MoneyManager moneyManager; // Reference to MoneyManager
    private int playerBet; // Player's bet on which rat will win
    public int betCost = 50; // Cost to place a bet
    public int winPayout = 200; // Payout for winning

    private void Start()
    {
        // Initialize rats and money manager
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    public void StartRace(int bet)
    {
        if (moneyManager.money < betCost)
        {
            resultText.text = "Not enough money to place a bet!";
            return;
        }

        playerBet = bet;
        moneyManager.SubtractMoney(betCost); // Deduct the bet cost
        StartCoroutine(Race());
    }

    private IEnumerator Race()
    {
        // Reset positions of rats
        foreach (var rat in rats)
        {
            rat.transform.position = Vector3.zero; // Start at the origin
        }

        // Move rats forward at random speeds
        while (true)
        {
            for (int i = 0; i < rats.Length; i++)
            {
                float speed = Random.Range(minSpeed, maxSpeed);
                rats[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            // Check for a winner
            for (int i = 0; i < rats.Length; i++)
            {
                if (rats[i].transform.position.z >= 10f) // Assuming 10f is the finish line
                {
                    DetermineWinner(i);
                    yield break;
                }
            }

            yield return null; // Wait for the next frame
        }
    }

    private void DetermineWinner(int winningRatIndex)
    {
        resultText.text = "Rat " + (winningRatIndex + 1) + " wins!";
        if (playerBet == winningRatIndex + 1)
        {
            moneyManager.AddMoney(winPayout); // Add payout for winning
            resultText.text += " You win!";
        }
        else
        {
            resultText.text += " You lose!";
        }
    }
}

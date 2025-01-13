using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockRaceGame : MonoBehaviour
{
    public GameObject[] blocks; // Array of blocks
    public float speed = 5f; // Speed of the blocks
    public Text resultText; // UI Text to display the result
    public MoneyManager moneyManager; // Reference to MoneyManager
    private int playerBet; // Player's bet on which block will win

    private void Start()
    {
        // Initialize blocks and money manager
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    public void StartRace(int bet)
    {
        playerBet = bet;
        StartCoroutine(Race());
    }

    private IEnumerator Race()
    {
        // Move blocks forward randomly
        while (true)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            // Check for a winner
            if (blocks[0].transform.position.z >= 10f) // Assuming 10f is the finish line
            {
                DetermineWinner(0);
                yield break;
            }
            else if (blocks[1].transform.position.z >= 10f)
            {
                DetermineWinner(1);
                yield break;
            }
            else if (blocks[2].transform.position.z >= 10f)
            {
                DetermineWinner(2);
                yield break;
            }
            else if (blocks[3].transform.position.z >= 10f)
            {
                DetermineWinner(3);
                yield break;
            }

            yield return null; // Wait for the next frame
        }
    }

    private void DetermineWinner(int winningBlockIndex)
    {
        resultText.text = "Block " + (winningBlockIndex + 1) + " wins!";
        if (playerBet == winningBlockIndex + 1)
        {
            moneyManager.AddMoney(20); // Example payout
            resultText.text += " You win!";
        }
        else
        {
            moneyManager.SubtractMoney(10); // Example loss
            resultText.text += " You lose!";
        }
    }
}

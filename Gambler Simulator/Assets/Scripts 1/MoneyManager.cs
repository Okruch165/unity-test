using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int money = 100; // Initial amount of money
    public UnityEngine.UI.Text moneyText; // Reference to UI text displaying the money

    private void Start()
    {
        UpdateMoneyText();
    }

    // Function to add money
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
        Debug.Log("Current money: " + money);
    }

    // Function to subtract money
    public void SubtractMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyText();
            Debug.Log("Current money: " + money);
        }
        else
        {
            Debug.Log("Not enough money to subtract!");
        }
    }

    // Function to reset money
    public void ResetMoney()
    {
        #if UNITY_EDITOR
        money = 100; // Reset money only in Unity Editor
        UpdateMoneyText();
        Debug.Log("Money reset to 100 in Unity Editor.");
        #else
        Debug.Log("Money reset not allowed in build version.");
        #endif
    }

    // Update the UI text displaying the money
    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + money.ToString();
        }
    }

    // Optional function to save money
    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        Debug.Log("Money saved: " + money);
    }

    // Optional function to load money
    public void LoadMoney()
    {
        money = PlayerPrefs.GetInt("Money", 100); // Default to 100 if no save exists
        UpdateMoneyText();
        Debug.Log("Money loaded: " + money);
    }
}

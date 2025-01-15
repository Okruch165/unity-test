using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int money = 100; // Początkowa ilość pieniędzy
    public UnityEngine.UI.Text moneyText;  // Odwołanie do UI tekstu wyświetlającego ilość pieniędzy

    private void Start()
    {
        UpdateMoneyText(); // Update money display immediately on start
    }

    // Funkcja dodająca pieniądze
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
        Debug.Log("Current money: " + money);
    }

    // Funkcja odejmująca pieniądze
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

    // Funkcja do resetowania pieniędzy
    public void ResetMoney()
    {
        #if UNITY_EDITOR
        money = 100; // Resetowanie pieniędzy tylko w edytorze Unity
        UpdateMoneyText();
        Debug.Log("Money reset to 100 in Unity Editor.");
        #else
        Debug.Log("Money reset not allowed in build version.");
        #endif
    }

    // Aktualizacja tekstu wyświetlającego ilość pieniędzy
    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + money.ToString();
        }
    }

    // Opcjonalna funkcja do zapisu pieniędzy
    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        Debug.Log("Money saved: " + money);
    }

    // Opcjonalna funkcja do ładowania pieniędzy
    public void LoadMoney()
    {
        money = PlayerPrefs.GetInt("Money", 100); // Domyślnie 100, jeśli brak zapisu
        UpdateMoneyText();
        Debug.Log("Money loaded: " + money);
    }
}

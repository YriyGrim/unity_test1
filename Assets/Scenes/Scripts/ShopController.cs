using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public GameObject shopCanvas;
    public TextMeshProUGUI increaseDamageCostText;
    public TextMeshProUGUI increaseHealthCostText;
    public TextMeshProUGUI healFullHealthCostText;
    private int increaseDamageCost;
    private int increaseHealthCost;
    private int healFullHealthCost;
    private bool isPlayerNearby = false;
    private PlayerController playerController;

    private float increaseDamageMultiplier = 1.0f;
    private float increaseHealthMultiplier = 1.0f;
    private float healFullHealthMultiplier = 1.0f;

    void Start()
    {
        CloseShop();
        UpdateCostText(); // Исправленное имя вызываемого метода
    }


    private void UpdateCostText()
    {
        int cost = Random.Range(8, 16);
        increaseDamageCostText.text =  cost + " Coins";
        increaseHealthCostText.text =  cost + " Coins";
        healFullHealthCostText.text =  cost + " Coins";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the shop!"); // Отладочное сообщение
            playerController = other.GetComponent<PlayerController>();
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the shop!"); // Отладочное сообщение
            isPlayerNearby = false;
            CloseShop();
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            shopCanvas.SetActive(!shopCanvas.activeInHierarchy);
            UpdateCosts();
        }
    }

    public void IncreaseDamage()
    {
        if (playerController.SpendCoins(increaseDamageCost))
        {
            playerController.IncreaseDamage();
            increaseDamageMultiplier += 0.2f; // Увеличиваем множитель
            increaseDamageCost = Mathf.RoundToInt(10 * increaseDamageMultiplier); // Обновляем цену
            UpdateCostText();
        }
    }

    public void IncreaseHealth()
    {
        if (playerController.SpendCoins(increaseHealthCost))
        {
            playerController.IncreaseMaxHealth();
            increaseHealthMultiplier += 0.2f;
            increaseHealthCost = Mathf.RoundToInt(10 * increaseHealthMultiplier);
            UpdateCostText();
        }
    }

    public void HealFullHealth()
    {
        if (playerController.SpendCoins(healFullHealthCost))
        {
            playerController.HealToFullHealth();
            healFullHealthMultiplier += 0.2f;
            healFullHealthCost = Mathf.RoundToInt(10 * healFullHealthMultiplier);
            UpdateCostText();
        }
    }


    private void CloseShop()
    {
        shopCanvas.SetActive(false);
    }

    private void UpdateCosts()
    {
        increaseDamageCost = Random.Range(8, 16);
        increaseHealthCost = Random.Range(8, 16);
        healFullHealthCost = Random.Range(8, 16);

        increaseDamageCostText.text = increaseDamageCost.ToString();
        increaseHealthCostText.text = increaseHealthCost.ToString();
        healFullHealthCostText.text = healFullHealthCost.ToString();
    }
}




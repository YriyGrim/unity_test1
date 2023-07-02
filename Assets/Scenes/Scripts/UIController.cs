using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Make sure you're using this namespace for TextMeshPro
using UnityEngine.UI; // Add this namespace for Slider

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI coinText; // Use TextMeshProUGUI instead of Text
    public Slider healthSlider; // Replace TextMeshProUGUI with Slider
    public PlayerController playerController;

    void Start()
    {
        UpdateCoinDisplay();
        UpdateHealthDisplay();
    }

    void Update()
    {
        UpdateCoinDisplay();
        UpdateHealthDisplay();
    }

    public void UpdateCoinDisplay()
    {
        coinText.text = "Coins: " + playerController.GetCoinCount();
    }

    public void UpdateHealthDisplay()
    {
        healthSlider.value = playerController.GetHealth() / playerController.GetMaxHealth();
    }
}







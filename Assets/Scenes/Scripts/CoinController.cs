using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public int coinValue = 1;
    public PlayerController playerController; // Assign the PlayerController object in the Unity Editor

    void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player"))
    {
        Debug.Log("Coin collected!"); // Отладочное сообщение
        // Add the coin value to the player's coin count
        other.GetComponent<PlayerController>().AddCoin(coinValue);
        Destroy(gameObject); // Уничтожаем монетку
    }
    }

}




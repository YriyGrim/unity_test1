using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with ground!"); // Отладочное сообщение
            // Здесь можно добавить логику при столкновении игрока с землей
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collided with ground!"); // Отладочное сообщение
            // Здесь можно добавить логику при столкновении противника с землей
        }
    }
}

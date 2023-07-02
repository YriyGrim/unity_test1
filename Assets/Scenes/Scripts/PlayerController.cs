using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject head;
    public GameObject feet;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashSpeed = 20f;
    public float attackRadius = 1f;
    public int attackDamage = 25; // Изменено значение на 25
    public float maxHealth = 100f; // Maximum health value for the player
    public float currentHealth; // Current health value for the player
    public UIController uiController;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDashing = false;
    private Vector2 dashDirection;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(move, rb.velocity.y);

        // Flip the sprite based on the movement direction
        FlipSprite(move);

        // Determine if the player is running
        animator.SetBool("isRunning", Mathf.Abs(move) > 0 && isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
        else if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
            animator.SetTrigger("Dash");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            Attack();
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        dashDirection = new Vector2(transform.localScale.x, 0).normalized;
        rb.velocity = new Vector2(dashSpeed * dashDirection.x, rb.velocity.y);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        yield return new WaitForSeconds(0.1f);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        isDashing = false;
    }

    public int coinCount = 0; // The number of coins collected by the player

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void AddCoin(int value)
    {
        coinCount += value;
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        Debug.Log("Attack called. Found " + hitEnemies.Length + " colliders in attack range.");

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Debug.Log("Collider found with tag: " + enemyCollider.tag); // Добавьте эту строку для отладки

            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyController enemy = enemyCollider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
                else
                {
                    Debug.LogError("EnemyController component not found on enemy with tag 'Enemy'."); // Добавьте эту строку для отладки
                }
            }
        }
    }




    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    private void FlipSprite(float move)
    {
        if (move > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("Player took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(PlayDamageAnimation());
        }
    }

    private IEnumerator PlayDamageAnimation()
    {
        animator.SetBool("TookDamage", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("TookDamage", false);
    }



    private void Die()
    {
        Debug.Log("Player died!");
        animator.SetBool("IsDead", true);
        
        // Отключите управление игроком и другие компоненты, которые должны быть отключены при смерти игрока
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        
        

        // Остановить движение Rigidbody2D
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        SceneManager.LoadScene("MainMenu");
        // Здесь вы можете добавить код для обработки смерти игрока, такой как перезапуск уровня или показ экрана "Game Over"
    }



    public bool SpendCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough coins!");
            return false;
        }
    }

    public void IncreaseDamage()
    {
        attackDamage += 1; // Увеличиваем урон на 1
        Debug.Log("Damage increased!");
    }

    public void IncreaseMaxHealth()
    {
        maxHealth += 10; // Увеличиваем максимальное здоровье на 10
        currentHealth = maxHealth;
        UpdateHealthSlider();
        Debug.Log("Max health increased!");
    }

    public void HealToFullHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
        Debug.Log("Health fully restored!");
    }

   
    public void UpdateHealthSlider()
    {
        uiController.UpdateHealthDisplay();
    }


}








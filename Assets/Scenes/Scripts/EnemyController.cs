using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float maxHealth = 100f;
    protected float currentHealth;
    protected Rigidbody2D rb;
    protected Transform playerTransform;

    [SerializeField] private GameObject healthCanvasPrefab;
    private Slider healthSlider;

    public float detectionRadius = 5f;
    public float viewAngle = 60f;
    private bool isChasingPlayer = false;

    public float patrolSpeed = 1f;
    public float patrolDistance = 2f;
    public float timeToWaitAtEdge = 2f;
    private float patrolDirection = 1f;
    private float edgeWaitTime;
    private Vector2 patrolStart;
    private bool waitingAtEdge = false;

    public GameObject coinPrefab; // Перетащите ваш префаб монетки сюда в инспекторе
    public int minCoins = 1;
    public int maxCoins = 5;
    

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        currentHealth = maxHealth;

        rb.mass = 1f;
        rb.drag = 5f;

        GameObject healthCanvasInstance = Instantiate(healthCanvasPrefab, transform.position, Quaternion.identity, transform);
        healthSlider = healthCanvasInstance.GetComponentInChildren<Slider>();
        
        Canvas canvas = healthCanvasInstance.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        float offsetY = 1f;
        healthCanvasInstance.transform.localPosition = new Vector3(0, offsetY, 0);

        UpdateHealthSlider();

        patrolStart = transform.position;
    }

    protected virtual void FixedUpdate()
    {
        if (IsPlayerInView())
        {
            isChasingPlayer = true;
        }

        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage. Current health: " + currentHealth);

        UpdateHealthSlider();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Enemy died!");
        SpawnCoins();
        Destroy(gameObject);
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth / maxHealth;
    }

    private void ChasePlayer()
    {
        float directionToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
        rb.velocity = new Vector2(directionToPlayer * moveSpeed, rb.velocity.y);
    }

    private void Patrol()
    {
        if (waitingAtEdge)
        {
            if (Time.time >= edgeWaitTime)
            {
                waitingAtEdge = false;
                patrolDirection *= -1f;
            }
        }
        else
        {
            float distanceFromStart = Vector2.Distance(transform.position, patrolStart);
            if (distanceFromStart >= patrolDistance)
            {
                waitingAtEdge = true;
                edgeWaitTime = Time.time + timeToWaitAtEdge;
            }
            else
            {
                rb.velocity = new Vector2(patrolDirection * patrolSpeed, rb.velocity.y);
            }
        }
    }

    private bool IsPlayerInView()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= detectionRadius)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRadius);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }





    private void SpawnCoins()
    {
        int coinCount = Random.Range(minCoins, maxCoins + 1);

        for (int i = 0; i < coinCount; i++)
        {
            GameObject coinInstance = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coinInstance.GetComponent<CoinController>().coinValue = 1; // Вы можете установить значение монетки здесь, если хотите

            // Добавьте небольшую случайную силу, чтобы монетки разлетались в разные стороны
            Rigidbody2D coinRb = coinInstance.GetComponent<Rigidbody2D>();
            float randomForceX = Random.Range(-2f, 2f);
            float randomForceY = Random.Range(1f, 3f);
            coinRb.AddForce(new Vector2(randomForceX, randomForceY), ForceMode2D.Impulse);
        }
    }
}




















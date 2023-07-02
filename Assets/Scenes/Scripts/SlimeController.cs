using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : EnemyController
{
    public float jumpForce = 5f;
    public int damage = 15;
    public Animator animator;

    public SlimeController()
    {
        maxHealth = 50;
    }

    private bool isGrounded = false;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        FlipSprite();

        if (isGrounded)
        {
            StartCoroutine(PrepareForJump());
        }
    }

    private void FlipSprite()
    {
        if (rb.velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator PrepareForJump()
    {
        isGrounded = false;
        animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, jumpForce);
        animator.SetBool("isJumping", false);
    }

    public virtual void TakeDamage(float damage) // Исправлено на 'float' вместо 'int'
    {
        base.TakeDamage(damage);
        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
        }
        else
        {
            animator.SetTrigger("die");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}









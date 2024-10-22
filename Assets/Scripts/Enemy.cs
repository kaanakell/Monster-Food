using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    public DropItem itemDrop;
    public Animator animator;
    public Rigidbody2D rb;
    public float knockBackForce = 10f; // Single knockback force for top-down
    public float knockTime = 0.2f; // Time to apply knockback force
    public float knockbackSmoothTime = 0.2f; // Time for the knockback to smoothly reduce
    public int dropItemQuantity = 1;

    // Declare the OnDestroyed event
    public delegate void EnemyDestroyed();
    public event EnemyDestroyed OnDestroyed;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Give Damage");
        currentHealth -= damage;

        ApplyKnockback(); // Apply smoother knockback

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to apply smoother knockback
    public void ApplyKnockback()
    {
        Transform attacker = GetClosestDamageSource();
        Vector2 knockbackDirection = (transform.position - attacker.position).normalized;
        StartCoroutine(SmoothKnockback(knockbackDirection * knockBackForce));
    }

    // Coroutine for smoother knockback effect
    private IEnumerator SmoothKnockback(Vector2 force)
    {
        float elapsedTime = 0;
        Vector2 originalVelocity = rb.velocity;

        while (elapsedTime < knockbackSmoothTime)
        {
            rb.velocity = Vector2.Lerp(force, Vector2.zero, elapsedTime / knockbackSmoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero; // Ensure the velocity is zero at the end
    }

    public Transform GetClosestDamageSource()
    {
        GameObject[] damageSources = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform currentClosestDamageSource = null;

        foreach (GameObject go in damageSources)
        {
            float currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                currentClosestDamageSource = go.transform;
            }
        }

        return currentClosestDamageSource;
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Play death animation, sound, etc.
        GetComponent<Collider2D>().enabled = false;
        Invoke("Destroy", 0.2f);
        this.enabled = false;
    }

    public void Destroy()
    {
        // Trigger OnDestroyed event
        if (OnDestroyed != null)
        {
            OnDestroyed.Invoke();
        }

        Destroy(gameObject);

        if (itemDrop != null)
        {
            var droppedItem = Instantiate(itemDrop, transform.position, Quaternion.identity);
            droppedItem.Quantity = dropItemQuantity;
        }
    }
}

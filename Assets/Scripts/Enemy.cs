using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider enemyHealthBar;  // Reference to the health bar slider
    public Canvas healthBarCanvas;  // Reference to the health bar canvas (UI component)
    
    public DropItem itemDrop;
    public Animator animator;
    public Rigidbody2D rb;
    public float knockBackForce = 10f;
    public float knockTime = 0.2f;
    public float knockbackSmoothTime = 0.2f;
    public int dropItemQuantity = 1;

    public delegate void EnemyDestroyed();
    public event EnemyDestroyed OnDestroyed;

    public AudioSource audioSource;
    public AudioClip hitSound; // Add the hit sound

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // Set the max value of the health bar and initialize current value
        if (enemyHealthBar != null)
        {
            enemyHealthBar.maxValue = maxHealth;
            enemyHealthBar.value = currentHealth;
        }

        // Initialize the AudioSource if not already set
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Update the health bar
        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = currentHealth;
        }

        // Play hit sound
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        ApplyKnockback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback()
    {
        Transform attacker = GetClosestDamageSource();
        Vector2 knockbackDirection = (transform.position - attacker.position).normalized;
        StartCoroutine(SmoothKnockback(knockbackDirection * knockBackForce));
    }

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

        rb.velocity = Vector2.zero;
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
        GetComponent<Collider2D>().enabled = false;
        Invoke("Destroy", 0.2f);
        this.enabled = false;
    }

    public void Destroy()
    {
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

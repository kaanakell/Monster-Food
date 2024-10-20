using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float damageFromEnemy = 10f;
    public float damageInterval = 0.5f; // Time between damage applications when in contact
    public HealthBar healthBar;
    public float gameOverDelay = 2f;

    private Animator animator;
    private bool isDead = false;
    private float nextDamageTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        // Check for continuous collision with enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && Time.time >= nextDamageTime)
            {
                TakeDamage(damageFromEnemy);
                nextDamageTime = Time.time + damageInterval;
                break; // Only take damage from one enemy at a time
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Play hurt animation
            animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player died!");
        animator.SetTrigger("IsDead");
        // Disable player controls here
        GetComponent<Collider2D>().enabled = false;
        
        // Start game over sequence
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        DisplayGameOverUI();
    }

    void DisplayGameOverUI()
    {
        GameObject gameOverUI = GameObject.Find("GameOverUI");
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameOverUI not found in the scene!");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float maxHealth;
    float currentHealth;


    public DropItem itemDrop;

    public Animator animator;

    public Rigidbody2D rb;
    public float knockBackForce = 10f;
    public float knockBackForceUp = 2f;

    public float knockTime;

    public int dropItemQuantity = 1;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

    }

    void Update()
    {


    }



    public void TakeDamage(float damage)
    {
        Debug.Log("Give Damage");
        currentHealth -= damage;
        //FindObjectOfType<AudioManager>().Play("SwordHit");

        knockBack();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void knockBack()
    {
        Transform attacker = getClosestDamageSource();
        Vector3 knockbackDirection = (transform.position - attacker.position).normalized;
        knockbackDirection.y = 0; // Ensure no vertical knockback
        rb.AddForce(knockbackDirection * knockBackForce + Vector3.up * knockBackForceUp, ForceMode2D.Impulse);
        StartCoroutine(KnockCo(rb));
    }

    public Transform getClosestDamageSource()
    {
        GameObject[] DamageSources = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform currentClosestDamageSource = null;

        foreach (GameObject go in DamageSources)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
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
        //FindObjectOfType<AudioManager>().Play("DeathSound");
        //Die anim
        //animator.SetTrigger("isDead");
        //Disable enemy
        GetComponent<Collider2D>().enabled = false;
        Invoke("Destroy", 0.2f);
        this.enabled = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
        if (itemDrop != null)
        {
            var droppedItem = Instantiate(itemDrop, transform.position, Quaternion.identity);
            droppedItem.Quantity = dropItemQuantity;
        }
    }

    private IEnumerator KnockCo(Rigidbody2D rb)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector3.zero;
        }
    }

}

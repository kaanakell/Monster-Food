using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    public float maxHealth = 200f;
    float currentHealth;

    public Animator animator;


    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
    }




    public void TakeDamageTwo(float damage)
    {
        Debug.Log("Damage");
        currentHealth -= damage;

        //Play hurt anim
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        //Die anim
        animator.SetBool("IsDead", true);

        //Disable enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}

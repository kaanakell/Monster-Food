using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : MonoBehaviour
{
    public float maxHealth = 250f;
    float currentHealth;

    public Animator animator;


    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
    }




    public void TakeDamageThree(float damage)
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

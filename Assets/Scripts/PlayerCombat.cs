using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;

    public bool isAttacking = false;

    public LayerMask enemyLayers;

    public float attackDamage = 40f;

    public float attackRate = 1.5f;
    float nextAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))  // Changed from GetKey to GetKeyDown
            {
                if (!isAttacking)
                {
                    Debug.Log("Attack initiated");
                    isAttacking = true;
                    animator.SetTrigger("Attack");
                    animator.SetBool("isAttacking", true);

                    // Detect enemies in range
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                    // Damage them
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        if (enemy.GetComponent<Enemy>() != null)
                        {
                            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                        }
                    }

                    nextAttackTime = Time.time + 1f / attackRate;

                    // Reset isAttacking after a short delay
                    //StartCoroutine(ResetAttackState());
                }

            }
            else
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
        }
    }

    IEnumerator ResetAttackState()
    {
        Debug.Log("ResetAttackState started");
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        Debug.Log("isAttacking reset to false");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}
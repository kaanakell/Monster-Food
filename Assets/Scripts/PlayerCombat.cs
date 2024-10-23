using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public bool isAttacking;
    public LayerMask enemyLayers;

    public const string PLAYER_ATTACK_RIGHT = "PlayerAttackRight";
    public const string PLAYER_ATTACK_LEFT = "PlayerAttackLeft";
    public const string PLAYER_ATTACK_UP = "PlayerAttackUp";
    public const string PLAYER_ATTACK_DOWN = "PlayerAttackDown";

    public float attackDamage = 40f;
    public float attackRate;
    private float nextAttackTime = 0f;

    private Vector2 lastDirection = Vector2.zero;
    private PlayerControl playerControl; // Reference to PlayerControl for movement

    private Vector3 defaultAttackPointPosition; // To store the default attack point position

    // Audio-related fields
    public AudioSource audioSource;
    public AudioClip attackSound; // Add the attack sound

    void Start()
    {
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>(); // Get the movement script

        // Store the default position of the attackPoint
        defaultAttackPointPosition = attackPoint.localPosition;
        isAttacking = false;

        // Initialize the AudioSource if not already set
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        DetectMovementDirection();
        if (!isAttacking) // Only allow attacking if not already attacking
        {
            Attack();
        }
        else
        {
            isAttacking = false;
        }
    }

    void DetectMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0)
            lastDirection = Vector2.right;
        else if (horizontal < 0)
            lastDirection = Vector2.left;
        else if (vertical > 0)
            lastDirection = Vector2.up;
        else if (vertical < 0)
            lastDirection = Vector2.down;
    }

    void Attack()
    {
        if (Time.time >= nextAttackTime && Input.GetKey(KeyCode.Space))
        {
            isAttacking = true;

            // Adjust the attackPoint based on the player's direction
            if (lastDirection == Vector2.right)
            {
                
                animator.Play(PLAYER_ATTACK_RIGHT);
                attackPoint.localPosition = new Vector2(1f, 0f); // Move attackPoint to the right
            }
            else if (lastDirection == Vector2.left)
            {
                
                animator.Play(PLAYER_ATTACK_LEFT, 0);
                attackPoint.localPosition = new Vector2(-1f, 0f); // Move attackPoint to the left
            }
            else if (lastDirection == Vector2.up)
            {
               
                animator.Play(PLAYER_ATTACK_UP);
                attackPoint.localPosition = new Vector2(0f, 1f); // Move attackPoint upwards
            }
            else if (lastDirection == Vector2.down)
            {
                
                animator.Play(PLAYER_ATTACK_DOWN);
                attackPoint.localPosition = new Vector2(0f, -1f); // Move attackPoint downwards
            }

            // Play attack sound
            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }

            // Detect and damage enemies
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<Enemy>() != null)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                }
            }

            // Reset isAttacking after short delay
            //StartCoroutine(EndAttackAfterAnimation());
        }
    }

    IEnumerator EndAttackAfterAnimation()
    {
        // Wait for attack animation to complete
        yield return new WaitForSeconds(1f);
    }

    public void FinishAttack()
    {
        // Reset attack state and allow running again
        isAttacking = false;

        // Reset attackPoint to its default position
        attackPoint.localPosition = defaultAttackPointPosition;

        playerControl.ChangeAnimationStateAccordingToMovement(); // Transition back to run or idle
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

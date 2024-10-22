using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    public float speed = 2f;
    public float distanceBetween = 5f;
    public float stopFollowDistance = 10f;

    [Header("Patrol Settings")]
    public float patrolRadius = 5f;
    public float waitTime = 2f;
    private Vector2 moveSpot;
    private float waitCounter;

    private bool isPatrolling = true;
    private Vector2 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
        SetNewMoveSpot();
        waitCounter = waitTime;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("Player not found. Make sure it has the 'Player' tag.");
                return;
            }
        }

        EnemyAILogic();
    }

    void EnemyPatrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
        {
            if (waitCounter <= 0)
            {
                SetNewMoveSpot();
                waitCounter = waitTime;
            }
            else
            {
                waitCounter -= Time.deltaTime;
            }
        }
    }

    void SetNewMoveSpot()
    {
        moveSpot = startingPosition + Random.insideUnitCircle * patrolRadius;
    }

    void EnemyFollow()
    {
        // Move towards the player but keep the original rotation
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        isPatrolling = false;
    }

    void EnemyAILogic()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceBetween)
        {
            EnemyFollow();
        }
        else if (distance > stopFollowDistance)
        {
            isPatrolling = true;
        }

        if (isPatrolling)
        {
            EnemyPatrol();
        }
    }

    // Optional: Visualize patrol area in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startingPosition, patrolRadius);
    }
}

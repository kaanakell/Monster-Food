using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;
    public float stopFollowDistance;
    public Transform moveSpot;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float startWaitTime;
    private float distance;
    private float waitTime;

    private bool isPatrolling = true;

    void Start()
    {
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    void Update()
    {
        EnemyAILogic();
    }

    void EnemyPatrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < .2f)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void EnemyFollow()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        isPatrolling = false;
    }

    void EnemyAILogic()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceBetween)
        {
            EnemyFollow();
        }
        else if (distance > stopFollowDistance)
        {
            isPatrolling = true; // Resume patrolling when player is far enough
        }

        if (isPatrolling)
        {
            EnemyPatrol();
        }
    }
}

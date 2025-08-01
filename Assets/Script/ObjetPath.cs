using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetPath : MonoBehaviour
{

    [Header("Design")]
    public bool Startpatrol;
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float TimeDecrease = 0.2f;

    private void Start()
    {
        targetPoint = 0;
    }

    private void Update()
    {
        if (Startpatrol)
        {
            if (transform.position == patrolPoints[targetPoint].position)
            {
                increaseTargetInt();
            }

            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);
        }
    }

    public void increaseTargetInt()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }
}

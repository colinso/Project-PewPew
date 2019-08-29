using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector2 movement;
    private NavMeshAgent2D agent;
    private Vector2 lastMove;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = agent.velocity.x;
        movement.y = agent.velocity.y;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (agent.velocity.x > 0.5f || agent.velocity.x < -0.5f)
        {
            lastMove = new Vector2(agent.velocity.x, 0f);
        }
        if (agent.velocity.y > 0.5f || agent.velocity.y < -0.5f)
        {
            lastMove = new Vector2(0f, agent.velocity.y);
        }

        animator.SetFloat("LastMoveHorizontal", lastMove.x);
        animator.SetFloat("LastMoveVertical", lastMove.y);
    }
}

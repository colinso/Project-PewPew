using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    private Vector2 stopPosition;
    public bool stopMovement;
    public Animator animator;
    private Vector2 lastMove;

    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        animator.SetFloat("LastMoveHorizontal", lastMove.x);
        animator.SetFloat("LastMoveVertical", lastMove.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    protected virtual void Move()
    {
        if (stopMovement == true)
        {
            transform.position = stopPosition;
        }
        else
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public float distance = 5f;
    public float timeout = 1f;
    public LineRenderer lineSprite;

    Vector3 targetPos;
    RaycastHit2D hit;
    Rigidbody2D rb;
    bool grappling;
    bool spriteReady;
    Vector3 grapplingPos;
    Vector3 originalPos;
    private float timeGrappling;

    private void Start()
    {
        lineSprite = GetComponent<LineRenderer>();
        lineSprite.enabled = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
            moveDirection.z = 0;
            hit = Physics2D.Raycast(transform.position, moveDirection, distance);
            if (hit.collider != null)
            {
                grappling = true;
                timeGrappling = timeout;
                grapplingPos = hit.point;
                lineSprite.enabled = true;
                lineSprite.SetPosition(0, transform.position);
                lineSprite.SetPosition(1, transform.position);
            }
        }
        if (grappling && hit)
        {
            shootSprite();
            if (spriteReady)
            {
                timeGrappling -= Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, grapplingPos, 15f * Time.deltaTime);
                lineSprite.SetPosition(0, transform.position);
                if (timeGrappling <= 0 || (Vector3.Distance(transform.position, hit.transform.position)) < 1f)
                {
                    resetLine();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        resetLine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        resetLine();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>() != null)
        {
            resetLine();
        }
    }

    private void shootSprite()
    {
        lineSprite.SetPosition(0, transform.position);
        lineSprite.SetPosition(1, Vector3.MoveTowards(originalPos, grapplingPos, Vector3.Distance(grapplingPos, originalPos)));
        if(lineSprite.GetPosition(1) == grapplingPos)
        {
            spriteReady = true;
        }
    }
    private void resetLine()
    {
        grappling = false;
        lineSprite.enabled = false;
    }
}

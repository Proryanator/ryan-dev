using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovements : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float JumpForce = 10f;
    public bool Grounded;
    public bool FacingRight { get; set; } = true;
    public Rigidbody2D Rigidbody { get; set; }
    public float Horizontal { get; set; }

    public LayerMask whatIsGround;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Update()
    {
        MovingPlayer();

        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            Grounded = false;
            Rigidbody.velocity = new Vector2(0, JumpForce);
        }
    }

    private void GroundCheck()
    {
        Grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.2f, transform.position.y - 0.15f),
        new Vector2(transform.position.x + 0.2f, transform.position.y - 0.3f), whatIsGround);
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void MovingPlayer()
    {
        Horizontal = Input.GetAxis("Horizontal");

        float signX = Mathf.Sign(Horizontal);

        if (signX == 1 && Mathf.Abs(Horizontal) > 0f && !FacingRight) //if (Horizontal > 0 && !FacingRight)
            Flip();
        else if (signX == -1 && Mathf.Abs(Horizontal) > 0f && FacingRight) //else if (Horizontal < 0 && FacingRight)
            Flip();


        if (Mathf.Abs(Horizontal) > 0.2f)
            Rigidbody.velocity = new Vector2(Mathf.Sign(Horizontal) * MoveSpeed, Rigidbody.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 0.15f),
            new Vector2(0.4f, 0.3f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
    //movement variables
    public float maxSpeed;

    //jumping variables
    bool grounded = false;
    float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;

    //Unity variables
    Rigidbody2D playerRB;
    Animator playerAnim;
    bool facingRight;
    //change player's graphical direction
    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	// Use this for initialization
	void Start () {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        facingRight = true;
	}

    void Update()
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            grounded = false;
            //playerAnim.SetBool("isGrounded",grounded);
            playerRB.AddForce(new Vector2(0, jumpHeight));
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        //jump
        
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //playerAnim.SetBool("isGrounded", grounded);
        playerAnim.SetFloat("verticalSpeed", playerRB.velocity.y);

        //walk
        float move = Input.GetAxis("Horizontal");
        playerRB.velocity = new Vector2(move * maxSpeed, playerRB.velocity.y);
        if(move>0 && !facingRight)
        {
            flip();
        }else if(move<0 && facingRight)
        {
            flip();
        }
    }
}

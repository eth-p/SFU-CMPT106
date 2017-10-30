using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy01Controller: MonoBehaviour {

    public float enemyVelocity;
    Animator enemyAnim;
    public GameObject enemyGraphic;
    bool ableChangeDirection = true;
    bool facingRight = false;
    float sightChangeTime = 2f;
    float nextChange = 0f;

    //for attack
    public float runningTime;
    float startRunningTime;
    bool isRunning;
    Rigidbody2D enemyRB;




	// Use this for initialization
	void Start () {
        enemyAnim = GetComponentInChildren<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > nextChange)
        {
            if(Random.Range(0,5) >= 2) changeDirection();
            nextChange = Time.time + sightChangeTime;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if(facingRight && other.transform.position.x < transform.position.x)
            {
                changeDirection();
            }else if(!facingRight && other.transform.position.x > transform.position.x)
            {
                changeDirection();
            }
            ableChangeDirection = false;
            isRunning = true;
            startRunningTime = Time.time + runningTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(startRunningTime < Time.time)
            {
                if (!facingRight) enemyRB.AddForce(new Vector2(-1, 0) * enemyVelocity);
                else enemyRB.AddForce(new Vector2(1, 0) * enemyVelocity);
                enemyAnim.SetBool("foundPlayer", isRunning);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ableChangeDirection = true;
            isRunning = false;
            enemyRB.velocity = new Vector2(0f, 0f);
            enemyAnim.SetBool("foundPlayer", isRunning);
        }
    }

    void changeDirection()
    {
        if (!ableChangeDirection)
        {
            return;
        }
        float facingX = enemyGraphic.transform.localScale.x;
        facingX *= -1f;
        enemyGraphic.transform.localScale = new Vector3(facingX, enemyGraphic.transform.localScale.y, enemyGraphic.transform.localScale.z);
        facingRight = !facingRight;
    }
}

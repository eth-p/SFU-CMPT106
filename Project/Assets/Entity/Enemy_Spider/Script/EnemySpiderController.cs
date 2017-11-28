using UnityEngine;

/// <summary>
/// The main controller for the zombie researcher enemy.
/// </summary>
public class EnemySpiderController : MonoBehaviour, DeathBehaviour, HurtBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // Constants:

    protected const float SPEED_WALK = 1.5f;
    protected const float SPEED_ATTACK = 2f;

    protected float SEE_DISTANCE = 8f;
    protected float WALK_DISTANCE = 2f;


    // -----------------------------------------------------------------------------------------------------------------
    // Configurable:

    /// <summary>
    /// The layers which obstruct the enemy line of sight.
    /// </summary>
    public LayerMask GroundLayers;

    /// <summary>
    /// The toxin which the spider would spit
    /// </summary>
    public GameObject spider_spit;

    /// <summary>
    /// The number of ticks before a state change when the entity is idle. 
    /// </summary>
    public int TicksIdle = 60;

    /// <summary>
    /// The number of ticks before a state change when the entity is moving around. 
    /// </summary>
    public int TicksMove = 60;

    /// <summary>
    /// The number of ticks before a state change when the entity is attacking. 
    /// </summary>
    public int TicksAttacking = 180;


    // -----------------------------------------------------------------------------------------------------------------
    // Enum:

    /// <summary>
    /// The entity's state.
    /// </summary>
    public enum State
    {
        IDLE,
        WALK,
        ATTACK
    }


    // -----------------------------------------------------------------------------------------------------------------
    // Internal:

    private int updateTicks;
    private State state;

    private Rigidbody2D body;
    private Animator animator;

    private GameObject player;
    private Rigidbody2D player_body;

    private Vector2 moveDirection;
    private Vector2 anchor;

    private Transform spit_from;


    // -----------------------------------------------------------------------------------------------------------------
    // Unity:

    /// <summary>
    /// [UNITY] Called when the object is instantiated.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");
        player_body = player.GetComponent<Rigidbody2D>();

        state = State.WALK;
        anchor = transform.position;

        spit_from = GameObject.Find("Spit From").transform;
    }

    /// <summary>
    /// [UNITY] Called every tick.
    /// </summary>
    void FixedUpdate()
    {
        if (--updateTicks < 0)
        {
            UpdateState();
        }

        switch (state)
        {
            case State.WALK:
                ActWalk();
                break;

            case State.ATTACK:
                ActAttack();
                break;

            case State.IDLE:
                ActIdle();
                break;
        }
    }


    // -----------------------------------------------------------------------------------------------------------------
    // Implement: DeathBehaviour, HurtBehaviour

    /// <inheritdoc cref="DeathBehaviour.OnDeath"/>
    public void OnDeath()
    {
        animator.SetTrigger("Death");
        gameObject.SetActive(false);
        Destroy(this);
    }

    /// <inheritdoc cref="HurtBehaviour.OnHurt"/>
    public void OnHurt(float amount)
    {
        animator.SetBool("Hurt", true);
    }

    /// <inheritdoc cref="HurtBehaviour.OnVulnerable"/>
    public void OnVulnerable()
    {
        animator.SetBool("Hurt", false);
    }


    // -----------------------------------------------------------------------------------------------------------------
    // Methods:

    /// <summary>
    /// Update the entity's state.
    /// </summary>
    void UpdateState()
    {
        bool los = gameObject.CanSee(player_body, SEE_DISTANCE, GroundLayers);
        animator.SetBool("Attacking", los);
        if (los)
        {
            state = State.ATTACK;
            updateTicks = TicksAttacking;
        }
        else
        {
            if (state == State.ATTACK)
            {
                anchor = transform.position;
            }

            if (Random.Range(0, 2) == 1)
            {
                state = State.WALK;
                updateTicks = TicksMove;
                RngDirection();
            }
            else
            {
                state = State.IDLE;
                updateTicks = TicksIdle;
            }
        }
    }

    /// <summary>
    /// Spider will spit toxin once within the range
    /// </summary>
    void ActAttack()
    {
        if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 1)
        {
            body.velocity = new Vector2(0f, 0f);
            Instantiate(spider_spit, spit_from.position, Quaternion.identity);
        }
        else
        {
            body.velocity = new Vector2(Mathf.Sign(player_body.position.x - body.position.x) * SPEED_WALK, 0f);
            if (body.velocity.x < 0)
            {
                FaceLeft();
            }
            else
            {
                FaceRight();
            }
        }
    }

    /// <summary>
    /// Do the actions for when the entity is idle.
    /// </summary>
    void ActIdle()
    {
    }

    /// <summary>
    /// Do the actions for when the entity is walking.
    /// </summary>
    void ActWalk()
    {
        Vector2 distance = anchor - new Vector2(transform.position.x, transform.position.y);
        if (Mathf.Abs(distance.x) < WALK_DISTANCE || Mathf.Sign(moveDirection.x) == Mathf.Sign(distance.x))
        {
            animator.SetTrigger("Walk");
            body.velocity = moveDirection;
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    /// <summary>
    /// Pick the direction which the entity moves during its walking state.
    /// </summary>
    void RngDirection()
    {
        if (Random.Range(0, 2) == 0)
        {
            FaceLeft();
            moveDirection = new Vector2(-SPEED_WALK, 0);
        }
        else
        {
            FaceRight();
            moveDirection = new Vector2(SPEED_WALK, 0);
        }
    }

    /// <summary>
    /// Make the entity face left.
    /// </summary>
    void FaceLeft()
    {
        Vector3 ls = transform.localScale;
        if (ls.x < 0)
        {
            transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
        }
    }

    /// <summary>
    /// Make the entity face right.
    /// </summary>
    void FaceRight()
    {
        Vector3 ls = transform.localScale;
        if (ls.x > 0)
        {
            transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
        }
    }
}
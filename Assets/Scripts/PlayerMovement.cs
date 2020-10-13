using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*********************
     * GENERAL VARIABLES *
     *********************/
    // The rigidbody of the gameobject
    private Rigidbody2D rigidbody2d = default;

    // Is the gameobject on the ground
    private bool grounded = false;

    // ScoreCounter
    private ScoreCounter scoreCounter = default;

    [SerializeField] private int scoreBonus = default;

    private bool addedPointsForGrounded = false;

    private Animator animator = default;

    private bool animationPlayed = false;

    /************************
     * HORIZONTAL VARIABLES *
     ************************/

    // The maximum horizontal velocity
    [SerializeField] private float maximumHorizontalVelocity = 5f;

    // The speed at which velocity increases in the air
    [SerializeField] private float airVelocityMultiplier = 2.5f;

    // The direction the player is moving in
    private int direction = 0;

    // Last direction the player moved in
    private int lastMovedDirection = 1;

    /**********************
     * VERTICAL VARIABLES *
     **********************/

    // Velocity of a jump of the gameobject
    [SerializeField] private float jumpVelocity = 20f;

    // Amount that the total velocity decreases with during a jump
    [SerializeField] private float decrease = 1f;

    // The default gravity the gameobject is set to
    private float defaultGravity = 0;

    // Is the player trying to jump
    private bool jumpRequest = false;

    // Is gravity being applied
    private bool gravityActive = true;

    // Has the player released the jump key
    private bool jumpKeyReleased = true;

    // Layermask for checking if grounded
    private LayerMask groundedMask = 1 << 8;

    /***********
     * GENERAL *
     ***********/

    // Awake is called before start
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreCounter = FindObjectOfType<ScoreCounter>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        defaultGravity = rigidbody2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        ResetVariables();

        HorizontalInputs();
        VerticalInputs();

        HorizontalMovement();
        VerticalMovement();
    }

    // FixedUpdate is called at equal intervals
    void FixedUpdate()
    {
        FixedHorizontalMovement();
        FixedVerticalMovement();
    }

    // Reset variables for the next frame
    void ResetVariables()
    {
        jumpRequest = false;
        grounded = IsGrounded();
    }

    // Check if the player is on the ground
    bool IsGrounded()
    {
        bool grounded = (Physics2D.BoxCast(transform.position, new Vector2(transform.localScale.x - 0.01f, 0.1f), 0, Vector2.down, 0.8f + transform.localScale.y * 0.5f, groundedMask) && rigidbody2d.velocity.y <= 0);

        if (!animationPlayed && grounded)
        {
            animationPlayed = true;
            animator.SetBool("Landed", grounded);
        }
        else
        {
            animator.SetBool("Landed", false);
        }

        if (!grounded)
        {
            animationPlayed = false;
        }

        return grounded;
    }

    /**************
     * HORIZONTAL *
     **************/

    // Inputs related to horizontal movement
    void HorizontalInputs()
    {
        direction = 0;

        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            direction = -1;
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            direction = 1;

        if (direction == 0)
            return;

        lastMovedDirection = direction;
    }

    // Horizontal movement of the player
    void HorizontalMovement()
    {
    }

    // Fixed horizontal movement of the player
    void FixedHorizontalMovement()
    {
        if (direction == 0)
            return;

        if (Mathf.Abs(rigidbody2d.velocity.x) < maximumHorizontalVelocity * direction && !grounded)
            rigidbody2d.velocity += new Vector2(direction * airVelocityMultiplier, 0);
        else
            rigidbody2d.velocity = new Vector2(direction * maximumHorizontalVelocity, rigidbody2d.velocity.y);

        if (direction == -1)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        if (direction == 1)
            GetComponentInChildren<SpriteRenderer>().flipX = false;
    }

    /************
     * VERTICAL *
     ************/

    // Inputs related to vertical movement
    void VerticalInputs()
    {
        if (!Input.GetKey(KeyCode.UpArrow))
        {
            jumpKeyReleased = true;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!jumpKeyReleased)
                return;

            if (grounded)
            {
                jumpKeyReleased = false;
                jumpRequest = true;
            }
        }
        else if (!gravityActive)
            GravityOn();
    }

    // Vertical movement of the player
    void VerticalMovement()
    {
        if (jumpRequest)
            Jump();
    }

    // Fixed vertical movement of the player
    void FixedVerticalMovement()
    {
        float velocity = rigidbody2d.velocity.y;

        if (velocity > jumpVelocity)
        {
            velocity -= decrease;
            velocity = (velocity < jumpVelocity) ? jumpVelocity : velocity;
        }

        if (rigidbody2d.velocity.y > jumpVelocity)
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, velocity);
    }

    // Jumping
    void Jump()
    {
        if (!grounded)
            return;

        animationPlayed = false;

        transform.SetParent(null);
        StartCoroutine(DelayGravity());

        rigidbody2d.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
    }

    // Turn gravity off
    void GravityOff()
    {
        gravityActive = false;
        rigidbody2d.gravityScale = 0;
    }

    // Turn gravity on
    void GravityOn()
    {
        gravityActive = true;
        rigidbody2d.gravityScale = defaultGravity;
    }

    // Coroutine to delay the appliance of gravity
    private IEnumerator DelayGravity()
    {
        GravityOff();
        yield return new WaitForSeconds(0.2f);

        if (!gravityActive)
            GravityOn();
    }

    // Return the last direction the player moved in
    public int GetLastMovedDirection()
    {
        return lastMovedDirection;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && grounded)
        {
            if (!addedPointsForGrounded)
            {
                scoreCounter.AddToScore(scoreBonus);
                addedPointsForGrounded = true;
            }

            collision.gameObject.GetComponent<Platform>().crumbling = true;
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        addedPointsForGrounded = false;
    }
}

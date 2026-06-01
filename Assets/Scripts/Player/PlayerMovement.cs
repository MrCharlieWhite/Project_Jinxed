using System;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    [Header("Audio Manager")]
    AudioManager_Levels audioManager;
    
    [Header("Components")]
    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMovement;
    private float verticalMovement;
    
    [Header("Animation")]
    Animator anim;
    
    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    int jumpsRemaining;
    private bool isOnGround = true;
    
    [Header("Gravity")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18;
    public float fallSpeedMultiplier = 2;
    
    [Header("Jump GroundCheck")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.02f);
    public LayerMask groundLayer;
    private bool isGrounded;
    
    [Header("Jump Wallcheck")]
    public Transform wallCheckPosition;
    public Vector2 wallCheckSize = new Vector2(0.4f, 0.02f);
    public LayerMask wallLayer;
    
    [Header("Wall Movement")]
    public float wallSlideSpeed = 2;
    [SerializeField] private bool isWallSliding;
    [SerializeField] private bool isWallClimbing;

    
    [Header("Wall Jumping")]
    [SerializeField] private bool isWallJumping;
    [SerializeField] private float wallJumpDirection;
    [SerializeField] private float wallJumpTime = 0.5f;
    [SerializeField] private float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);
    
    [Header("Death Checks")]
    public Transform trapCheckPosition;
    public Vector2 trapCheckSize = new Vector2(0.4f, 0.02f);
    public LayerMask trapLayer;
    BoxCollider2D boxCollider;
    bool isDead = false;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager_Levels>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        GroundCheck();
        Gravity();
        Flip();
        ProcessWallSLide();
        ProcessWallJump();
        PlayerDeathAnim();
        OnPlayerDeath();
        IsOnGround();
        
        
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            Flip();
        }
        
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        
    }

    private void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPosition.position, wallCheckSize, 0, wallLayer);
    }
    
    private void ProcessWallSLide()
    {
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        if (horizontalMovement > 0.1 || horizontalMovement < -0.1)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    public void WallMove(InputAction.CallbackContext context)
    {
        verticalMovement = context.ReadValue<Vector2>().y;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        { 
            if (context.performed)
            {
                audioManager.PlaySFX(audioManager.jump);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("isJumping");
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("isJumping");

            }
        }
        // Wall Jump 
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); // Jump away from the wall
            wallJumpTimer = 0;
            
            // Force Flip
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
            
            // Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }


    // Load scenes from player movement, calls the load next scene from level manager. 
    // Checks if the gameObject trying to load the next scene is valid and isn't an artifact from the previous scene
    // A GameManager game object must exist inside the scene for this to work 
    

    /*public void LoadNextScene(InputAction.CallbackContext context)
    {
        if (context.performed && gameObject.scene.IsValid())
        {
            GameManager.Instance.LevelManager.LoadNextScene();
        }
    }
    
    public void LoadPreviousScene(InputAction.CallbackContext context)
    {
        if (context.performed && gameObject.scene.IsValid())
        {
            GameManager.Instance.LevelManager.LoadPreviousScene();
        }
    } */

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer)) // Checks if box overlaps with ground
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
            isWallJumping = false;
            anim.SetBool("isOnGround", true);
        }
        else
        {
            isGrounded = false;
            anim.SetBool("isOnGround", false);
        }
    }

    private void IsOnGround()
    {
        if (isGrounded)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }
    
    

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    public bool PlayerDeathAnim()
    {
        if (!isDead && boxCollider.IsTouchingLayers(LayerMask.GetMask("Traps" )) )
        {
            audioManager.PlaySFX(audioManager.death);
            
            isDead = true;

            DisableControls();

            anim.SetTrigger("isDead");
            
            rb.bodyType = RigidbodyType2D.Static;

            return true;
        }
        
        else if (!isDead && boxCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            audioManager.PlaySFX(audioManager.death);
            
            isDead = true;

            DisableControls();

            anim.SetTrigger("isDead");

            rb.bodyType = RigidbodyType2D.Static;

            return true;
        }

        return false;
    
    }

    public void DisableControls()
    {
        playerInput.enabled = false;

        horizontalMovement = 0;
        verticalMovement = 0;

        rb.linearVelocity = Vector2.zero;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPosition.position, wallCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(trapCheckPosition.position, trapCheckSize);
    }
    
    // Checks collision with any object
    // If that object is on the traps layer, reload the scene
    /*  TODO: Refactor this to a player state script with an OnDeath function,
        that way you can do other things before reloading the scene, such as play animations, 
        particle effects, sounds etc, linger for a few seconds, fade and reload cleanly, rather
        than a harsh cut
     */

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        print("colliding");

        if (((1 << collision.gameObject.layer) & trapLayer.value) != 0)
        {
            print("Hit Trap");
            if (gameObject.scene.IsValid())
            {
                GameManager.Instance.LevelManager.ReloadLevel();
            }
        }
    }*/

    void OnPlayerDeath()
    {
        if (isDead)
        {
            Invoke("GoToDeathScreen", 5f);
        }

    }
    
    public void GoToDeathScreen()
    {
        SceneManager.LoadScene(4);
    }
}

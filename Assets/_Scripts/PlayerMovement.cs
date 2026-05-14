using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    // Movement
    public float moveSpeed = 5f;
    private float horizontalMovement;
    
    // Jumping
    public float jumpPower = 10f;
    public int maxJumps = 2;
    int jumpsRemaining;
    
    // Gravity
    public float baseGravity = 2;
    public float maxFallSpeed = 18;
    public float fallSpeedMultiplier = 2;
    
    // Jumping groundcheck
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.02f);
    public LayerMask groundLayer;
    public LayerMask trapLayer;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;
        GroundCheck();
    }

    private void Gravity()
    {
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        { 
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
            }
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
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
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
}

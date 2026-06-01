using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int startDirection = -1;
    private int currentDirection;
    private float halfWidth;
    private Vector2 movement;
    private void Start()
    {
        halfWidth = spriteRenderer.bounds.extents.x;
        currentDirection = startDirection;
    }

    private void FixedUpdate()
    {
        movement.x = moveSpeed * currentDirection;
        movement.y = myRigidBody.linearVelocity.y;
        myRigidBody.linearVelocity = movement;
        SetDirection();
    }

    private void SetDirection()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground")) && myRigidBody.linearVelocity.x > 0)
        {
            currentDirection *= -1;
            spriteRenderer.flipX = false;
        }
        
        else if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground")) &&
                 myRigidBody.linearVelocity.x < 0)
        {
            currentDirection *= -1;
            spriteRenderer.flipX = true;
        }
        
        Debug.DrawRay(transform.position, Vector2.right * (halfWidth + 0.1f), Color.red);
        Debug.DrawRay(transform.position, Vector2.left * (halfWidth + 0.1f), Color.red);
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerMovementSpeed = 5.0f;
    private Animator playerAnimator;
    private Collider2D playerCollider;
    private Rigidbody2D playerRigidbody;

    private Vector3 originalPosition;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        playerCollider = gameObject.GetComponent<Collider2D>();
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        
        originalPosition = transform.position;
        GameplayManager.instance.resetGame.AddListener(Reset);
        GameplayManager.instance.gameOver.AddListener(Reset);
    }

    private void Reset()
    {
        gameObject.SetActive(true);
        playerAnimator.Rebind();
        transform.position = originalPosition;
    }

    private void PlayerHit()
    {
        gameObject.SetActive(false);
    }

   
    void FixedUpdate()
    {
        if (GameplayManager.instance.gameFinished)
        {
            return;
        }

        Vector2 moveDir = Vector2.zero;

        // First get the player's input 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDir = Vector2.up;
        }

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir = Vector2.left;

        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDir = Vector2.down;

        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDir = Vector2.right;

        }

        if (moveDir == Vector2.zero)
        {
            return;
        }

        // Update the animations
        playerAnimator.SetFloat("MovementDirectionX", moveDir.x);
        playerAnimator.SetFloat("MovementDirectionY", moveDir.y);

        // Check for collisions before moving
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, moveDir);
        Debug.DrawRay((Vector2)transform.position, moveDir, Color.green);


        Vector2 moveDestination = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y) + moveDir, playerMovementSpeed);
        playerRigidbody.MovePosition(moveDestination);
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.gameObject.CompareTag("Enemy") == true)
        {
            GameplayManager.instance.PlayerHit();
            PlayerHit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MovingPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int runSpeed;
    [SerializeField] int jumpSpeed = 5;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 5f);
    bool isClimbing;
    Animator baseAnimator;
    Rigidbody2D myRigidbody;
    float gravity;
    bool isAlive;

    void Start()
    {
        isAlive = true;
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        gravity = myRigidbody.gravityScale;
        baseAnimator = gameObject.GetComponent<Animator>();
    }

    void MovedHorizontal(bool way)
    {
        if (way)
        {
            gameObject.transform.Translate(Vector3.right*Time.deltaTime);
            baseAnimator.SetBool("isRunning", true);
        }
        else
        {
            gameObject.transform.Translate(Vector3.left*Time.deltaTime);
            baseAnimator.SetBool("isRunning", true);

        }
       
    }

    void StopMoving()
    {
        baseAnimator.SetBool("isRunning", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            return;
        }
        if (gameObject.GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            isAlive = false;
            StopAllMoving();
        }
        Run();
        ClimbLadder();
        Jump();
        FlipSprite();

    }
    void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelosity = new Vector2(controlThrow*runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelosity;
        baseAnimator.SetBool("isRunning", true);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            gameObject.transform.localScale  =  new Vector2(Mathf.Sign(myRigidbody.velocity.x),1);

        }
        else
        {
            StopMoving();
        }

    }

    void Jump()
    {
        if (gameObject.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
        {

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidbody.velocity += jumpVelocityToAdd;
            }
        } 
    }

    void ClimbLadder()
    {
        if (!gameObject.GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            baseAnimator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = 1f;
            return;
        }
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbVelocityToAdd = new Vector2(myRigidbody.velocity.x, controlThrow* runSpeed);
            myRigidbody.velocity = climbVelocityToAdd;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
            baseAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
            myRigidbody.gravityScale = 0f;

    }


    void StopAllMoving()
    {
        
        baseAnimator.SetTrigger("Dying");
        myRigidbody.velocity = deathKick;
        

    }
}

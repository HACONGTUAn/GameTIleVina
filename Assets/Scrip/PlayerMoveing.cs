using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveing : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbingSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f,10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    bool IsAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive) { return; }
        Run();
        FlipSprite();
        ClimBing();
        Die();
    }

    void OnFire(InputValue value)
    {
        if (!IsAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }
    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Trapter")))
        {
            IsAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void ClimBing()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimBing", false);
            return;
        }
        Vector2 climBingVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbingSpeed);
        myRigidbody.velocity = climBingVelocity;
        myRigidbody.gravityScale = 0f;
        bool PlayerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimBing", PlayerHasVerticalSpeed);
    }

    void FlipSprite()
    {
        bool PlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (PlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
       
    }

    void OnMove(InputValue value)
    {
        if (!IsAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed , myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;


        bool PlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning",PlayerHasHorizontalSpeed);
    }

    void OnJump(InputValue value)
    {
        if (!IsAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }

    }
}

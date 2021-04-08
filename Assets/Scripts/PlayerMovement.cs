using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public float runSpeed = 40f;
    public float climbSpeed = 10f;

    public Animator animator;

    float horizontalMove = 0f;
    private float verticalMove = 0f;
    bool jump = false;
    bool crouch = false;


    private Rigidbody2D rb;
   
    public bool isClimbing = false;
    public bool onLadder = false;
    public LayerMask ladderMask;

    public enum detectionModes { 
        layerMask, tag
    }

    public detectionModes detectionMode;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump")) {

            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch")) {

            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {

            crouch = false;
        }

        
        if (isClimbing && Input.GetButton("Vertical"))
          {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, verticalMove);
           
          }
        else if(!onLadder)
          {
            rb.gravityScale = 3;

          }
        

        if (detectionMode == detectionModes.layerMask) {

            if (rb.IsTouchingLayers(ladderMask))
            {
                isClimbing = true;
                onLadder = true;
            }
            else {
                isClimbing = false;
                onLadder = false;
            }
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (detectionMode == detectionModes.tag) {

            if (collision.tag == "Ladder")
            {
                isClimbing = true;
                onLadder = true;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectionMode == detectionModes.tag)
        {

            if (collision.tag == "Ladder")
            {
                isClimbing = false;
                onLadder = false;
            }
        }
    }


    public void OnLanding() {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching) {

        animator.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

       
    }
}

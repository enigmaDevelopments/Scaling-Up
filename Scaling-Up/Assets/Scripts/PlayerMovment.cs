using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovment : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public Transform groundCheck2;
    public Transform pivot;
    public LayerMask groundLayer;
    private float horizantal;
    public float speed = 8f;
    public float jump = 16f;
    public float coyoteTime = .1f;
    private bool notJumped = false;
    private float timeFromGround = 100f;

    void Update()
    {
        horizantal = Input.GetAxisRaw("Horizontal") * speed;
        #region kyote timer
        if (trueGrounded())
        {
            notJumped = true;
            timeFromGround = 0;
        }
        else
            timeFromGround += Time.deltaTime;
        #endregion
        #region jump
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            notJumped = false;
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }
        #endregion
        #region flip
        Vector3 playerLocalScale = transform.localScale;
        if (pivot.rotation.eulerAngles.z > 10 && pivot.rotation.eulerAngles.z < 170)
            playerLocalScale.x = -Mathf.Abs(playerLocalScale.x);
        else if (pivot.rotation.eulerAngles.z < 350 && pivot.rotation.eulerAngles.z > 190)
            playerLocalScale.x = Mathf.Abs(playerLocalScale.x);
        transform.localScale = playerLocalScale;
        #endregion
        #region animation correction
        animator.SetBool("isJumping", !trueGrounded());
        animator.SetFloat("X Velocity",Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Y Velocity",rb.velocity.y);
        #endregion
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizantal, rb.velocity.y);
    }

    #region find groundedness
    public bool Grounded()
    {
        return notJumped && timeFromGround <= coyoteTime;
    }
    public bool trueGrounded()
    {
        return checkGround(groundLayer);
    }
    public bool checkGround(LayerMask layers)
    {
        return checkArea(groundCheck, groundCheck2, layers, gameObject);
    }
    public GameObject[] checkGroundAll(LayerMask layers)
    {
        return checkAreaAll(groundCheck, groundCheck2, layers, gameObject);
    }
    public static bool checkArea(Transform check, Transform check2, LayerMask layers, GameObject self)
    {
        return checkAreaAll(check, check2, layers, self).Count() != 0;
    }
    public static GameObject[] checkAreaAll(Transform check, Transform check2, LayerMask layers, GameObject self)
    {
        return (from col in Physics2D.OverlapAreaAll(check.position, check2.position, layers) where col.gameObject != self select col.gameObject).ToArray();
    }
    #endregion
}
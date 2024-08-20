using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    public LayerMask wallLayer;
    private float horizantal;
    public float maxRunSpeed = 8f;
    public float accel = .2f;
    public float deccel = .1f;
    public float airAccel = .1f;
    public float airDeccel = .5f;
    public float velPow = 2;
    public float jumpPower = 16f;
    public float unjumpPower = .5f;
    public float gravity = 1;
    public float sizePower = 1;
    public float fallingGravity = 3;
    public float coyoteTime = .1f;
    public float bufferTime = .2f;
    private bool notJumped = false;
    private bool unJumped = true;
    private float timeFromGround = 100f;
    private float timeFromJump = 100f;
    public float size = 1;
    private Vector2 scale = Vector2.one; 

    void Update()
    {
        horizantal = Input.GetAxisRaw("Horizontal") * maxRunSpeed * size * sizePower;
        #region gravity
        if (rb.velocity.y < -1f)
            rb.gravityScale = gravity * size * sizePower;
        else
            rb.gravityScale = fallingGravity * size * sizePower;
        #endregion
        #region kyote timer/input buffer
        if (trueGrounded())
        {
            notJumped = true;
            unJumped = true;
            timeFromGround = 0;
        }
        else
            timeFromGround += Time.deltaTime;
        timeFromJump += Time.deltaTime;
        #endregion
        #region jump
        if (Input.GetButtonDown("Jump"))
            timeFromJump = 0;
        else if (rb.velocity.y > 0f && unJumped && !Input.GetButton("Jump"))
        {
            unJumped = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * unjumpPower);
        }
        if (Grounded() && timeFromJump <= bufferTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            notJumped = false;
            timeFromJump = 100f;
        }
        #endregion
        #region flip
        Vector3 playerLocalScale = scale;
        if (pivot.rotation.eulerAngles.z > 10 && pivot.rotation.eulerAngles.z < 170)
            playerLocalScale.x = -Mathf.Abs(playerLocalScale.x);
        else if (pivot.rotation.eulerAngles.z < 350 && pivot.rotation.eulerAngles.z > 190)
            playerLocalScale.x = Mathf.Abs(playerLocalScale.x);
        transform.localScale = playerLocalScale;
        #endregion
        #region reset
        if (Input.GetButtonDown("Restart"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        #endregion
        #region animation correction
        animator.SetBool("isJumping", !trueGrounded());
        animator.SetFloat("X Velocity",Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Y Velocity",rb.velocity.y);
        #endregion
    }
    void FixedUpdate()
    {
        #region run
        float speedDif = horizantal -rb.velocity.x;
        float accelRate;
        if (Grounded())
            accelRate = (Mathf.Abs(horizantal) > 0.01f ? accel : deccel);
        else
            accelRate = (Mathf.Abs(horizantal) > 0.01f ? airAccel : airDeccel);
        float movment = Mathf.Pow(Math.Abs(speedDif * accelRate), velPow) * Mathf.Sign(speedDif);
        rb.AddForce(movment * Vector2.right);
        #endregion
        #region parent to wall
        GameObject[] walls = checkGroundAll(wallLayer);
        Transform parent;
        if (walls.Length > 0)
            parent = walls[0].transform.parent;
        else
            parent = null;
        //transform.parent = parent;
        //if (parent != null)
        //    scale = new Vector2(Mathf.Pow(parent.transform.lossyScale.x, -1) * size, Mathf.Pow(parent.transform.lossyScale.y, -1) * size);
        //else
            scale = size * Vector2.one;
        #endregion

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
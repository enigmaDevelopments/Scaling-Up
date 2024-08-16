using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

    public class PlayerMovment : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Transform groundCheck;
        public Transform groundCheck2;
        public Transform buttonCheck;
        public Transform buttonCheck2;
        public LayerMask groundLayer;
        public LayerMask buttonLayer;
        private float horizantal;
        public float speed = 8f;
        public float jump = 16f;
        public float coyoteTime = .1f;
        private bool _facingRight = true;
        private bool notJumped = false;
        private float timeFromGround = 100f;

        public bool FacingRight
        {
            get
            {
                return _facingRight;
            }
        }
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
            #region reset
            if (Input.GetButtonDown("Restart"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            #endregion
            Flip();
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
        private void Flip()
        {
            if (_facingRight && horizantal < 0f || !_facingRight && horizantal > 0f)
            {
                _facingRight = !_facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }
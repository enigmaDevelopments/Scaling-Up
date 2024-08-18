using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HairMovment : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject anchor;
    public float speed = 20f;
    public int hairs = 5;
    public float hairSize = .5f;
    public float minOffset;
    public float maxOffset;
    public float maxSpeed;
    public Vector2 offset = Vector2.zero;
    public Transform[] parts;
    public bool physics = true;
    void Start()
    {
        parts = new Transform[hairs];
        parts[0] = transform.Find("hair part");
        for (int i = 0; i < hairs; i++)
        {
            parts[i] = Instantiate(anchor.transform, transform);
            float scale = 1-(i+1)*(hairSize / hairs);
            parts[i].transform.localScale = new Vector2(scale,scale);
        }
    }

    
    void Update()
    {
        if (physics)
        {
            offset.y = -(minOffset + (maxOffset - minOffset) * Mathf.Clamp(rb.velocity.y / maxSpeed, -1, 1));
            offset.x = -maxOffset * Mathf.Clamp(rb.velocity.x / maxSpeed, -1, 1);
        }
        Transform follow = anchor.transform;
        foreach (Transform part in parts)
        {
            Vector2 target = (Vector2)follow.position + offset;
            part.position = Vector2.Lerp(part.position,target,speed*Time.deltaTime);
            follow = part;
        }
    }
}

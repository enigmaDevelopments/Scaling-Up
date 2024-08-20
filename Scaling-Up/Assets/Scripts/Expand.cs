using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Expand : MonoBehaviour
{
    public int dir = -1;
    private ExpandData data;
    private LayerMask mask;
    private Collider2D self1;
    private Collider2D self2;
    private GameObject player;
    private float speed;
    private Vector2 size;
    private Transform parent;
    private Transform changedPos;
    private Transform topParent;
    private Vector2 startingSize;
    private Vector2 startingPos;
    private int time;

    public void Data(int dir, float size)
    {
        data = gameObject.GetComponent<ExpandData>();
        self1 = data.self1;
        self2 = data.self2;
        player = GameObject.FindWithTag("Player");
        speed = data.speed;
        mask = ~data.mask;
        parent = transform.parent;
        changedPos = transform.parent;
        topParent = parent.parent.parent;
        if (dir > 0)
            parent = parent.parent;
        else
            changedPos = changedPos.parent;
        dir = Math.Abs(dir) - 1;
        foreach (Expand expand in gameObject.GetComponents<Expand>())
            if (expand.dir + 1 == dir + 1)
                Destroy(this);
        if (topParent.localScale[dir] * size < .5  || topParent.localScale[dir] * size > 2)
            Destroy(this);
        Vector2 end = parent.localScale;
        end[dir] *= size;
        startingSize = parent.localScale;
        startingPos = changedPos.position;
        this.dir = dir;
        this.size = end;
    }
    

    void FixedUpdate()
    {
        time++;
        parent.localScale = Vector2.Lerp(parent.localScale, size, time * speed);
        if (self1.IsTouchingLayers(mask))
        {
                TransferForce(); 
                size = startingSize;
        }
        if ((Vector2)parent.localScale == size)
        {
            parent.localScale = size;
            Vector2 position = new Vector2(Mathf.Round(transform.position.x*4)*.25f, Mathf.Round(transform.position.y * 4) * .25f);
            
            topParent.localScale *= size;
            parent.localScale = Vector2.one;
            topParent.position = position;
            TransferForce();
            Destroy(this);
        }
    }
    void TransferForce()
    {
        if (self2.IsTouching(player.GetComponent<Collider2D>()))
            player.gameObject.GetComponent<Rigidbody2D>().AddForce(((Vector2)changedPos.position - startingPos) / (time * Time.fixedDeltaTime), ForceMode2D.Impulse);
    }
}

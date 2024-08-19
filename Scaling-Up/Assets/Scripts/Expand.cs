using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Drawing;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class Expand : MonoBehaviour
{
    public int dir = -1;
    private ExpandData data;
    private Collider2D self;
    private Collider2D player;
    private float speed;
    private Vector2 size;
    private Transform parent;
    private Transform changedPos;
    private Transform topParent;
    private float startingsize;
    private Vector2 startingPos;
    private int time;
    

    public void Data(int dir, float size)
    {
        data = gameObject.GetComponent<ExpandData>();
        self = gameObject.GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        speed = data.speed;
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
        startingsize = parent.localScale[dir];
        startingPos = changedPos.position;
        this.dir = dir;
        this.size = end;
    }

    
    void FixedUpdate()
    {
        time++;
        parent.localScale = Vector2.Lerp(parent.localScale, size, time*speed);
        if ((Vector2)parent.localScale == size)
        {
            parent.localScale = size;
            Vector2 position = transform.position;
            topParent.localScale *= size;
            parent.localScale = Vector2.one;
            topParent.position = position;
            if (self.IsTouching(player))
                player.GetComponent<Rigidbody2D>().AddForce(((Vector2)changedPos.position - startingPos) / (time * Time.fixedDeltaTime), ForceMode2D.Impulse);
            Destroy(this);
        }
    }
    void unparent()
    {
        
    }
}

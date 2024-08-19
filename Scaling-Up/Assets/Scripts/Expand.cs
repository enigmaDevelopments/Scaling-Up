using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Drawing;

public class Expand : MonoBehaviour
{
    public int dir = -1;
    public float size;
    private Transform parent;
    private Transform topParent;
    private float velocity = .1f;

    public void Data(int dir, float size)
    {
        parent = transform.parent;
        topParent = parent.parent.parent;
        if (dir > 0)
            parent = parent.parent;
        dir = Math.Abs(dir) - 1;
        foreach (Expand expand in gameObject.GetComponents<Expand>())
            if (expand.dir + 1 == dir + 1)
                Destroy(this);
        if (topParent.localScale[dir] * size < .5  || topParent.localScale[dir] * size > 2)
            Destroy(this);
        velocity *= size == 2 ? 2 : -1;
        this.dir = dir;
        this.size = size;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentSize = parent.localScale;
        currentSize[dir] += velocity;
        parent.localScale = currentSize;
        if (MathF.Abs(currentSize[dir] - size) < MathF.Abs(velocity))
        {
            currentSize[dir] = size;
            parent.localScale = currentSize;
            Vector2 position = transform.position;
            topParent.localScale *= currentSize;
            parent.localScale = Vector2.one;
            topParent.position = position;
            Destroy(this);
        }
    }
    private float findScale() { 
        return (transform.parent.localScale[dir] * transform.parent.parent.localScale[dir] * transform.parent.parent.parent.localScale[dir]); 
    }
}

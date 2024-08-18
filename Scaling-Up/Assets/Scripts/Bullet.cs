using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType : byte
{
    blue,
    pink
}
public class Bullet : MonoBehaviour
{
    public SpriteRenderer sr;
    public BulletType bulletType;
    void Start()
    {
        Color color = Color.white;
        if (bulletType == BulletType.blue)
            ColorUtility.TryParseHtmlString("#5BCEFA", out color);
        else if (bulletType == BulletType.pink)
            ColorUtility.TryParseHtmlString("#F5A9B8", out color);
        sr.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

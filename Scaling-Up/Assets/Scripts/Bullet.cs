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
    public float speed = .5f;
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
    
    // Start is called before the first frame update
    void FixedUpdate()
    {
        transform.position += transform.up * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            transform.parent = collision.transform;
            Vector2 pos = transform.localPosition;
            int dir = (Mathf.Abs(pos.x) < Mathf.Abs(pos.y)? 2:1);
            dir *= pos[dir - 1] < 0 ? -1 : 1;
            Expand expand = collision.gameObject.AddComponent<Expand>();
            expand.Data(dir, (bulletType == BulletType.blue ? .5f : 2));
            Destroy(gameObject);
        }
    }
}

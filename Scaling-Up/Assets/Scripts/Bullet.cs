using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public enum BulletType : byte
{
    blue,
    pink
}
public class Bullet : MonoBehaviour
{
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public LayerMask excludeLayers;
    public LayerMask mirrorLayer;
    public BulletType bulletType;
    public float speed = .5f;
    private Collider2D[] colliders;
    private GameObject player;
    private List<GameObject> used;
    private Vector2 lastPos;
    private bool canCollide = true;
    private int timer = 0;
 
    void Start()
    {
        colliders = GetComponents<Collider2D>();
        foreach(Collider2D collider in colliders)
        {
            if (collider.IsTouchingLayers(mirrorLayer))
            {
                ScalePlayer();
                Destroy(gameObject);
            }
        }
        player = GameObject.FindWithTag("Player");
        used = new List<GameObject>();
        Color color = Color.white;
        if (bulletType == BulletType.blue)
            ColorUtility.TryParseHtmlString("#5BCEFA", out color);
        else if (bulletType == BulletType.pink)
            ColorUtility.TryParseHtmlString("#F5A9B8", out color);
        sr.color = color;
        rb.velocity =  (Vector2)transform.up * speed;
    }
    private void FixedUpdate()
    {
        if (timer != 0)
            timer--;
        if (timer == 1)
            transform.up = (Vector2)transform.position - lastPos;

    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used.Contains(collision.gameObject)) 
            return;
        used.Add(collision.gameObject);
        if (collision.gameObject.layer == 7)
        {
            transform.parent = collision.transform;
            Vector2 pos = transform.localPosition;
            int dir = (Mathf.Abs(pos.x) < Mathf.Abs(pos.y) ? 2 : 1);
            dir *= pos[dir - 1] < 0 ? -1 : 1;
            Expand expand = collision.gameObject.AddComponent<Expand>();
            expand.Data(dir, (bulletType == BulletType.blue ? .5f : 2));
        }
        else if (collision.gameObject.layer == 9)
        {
            foreach (Collider2D collider in colliders)
                collider.excludeLayers = excludeLayers;
            canCollide = false;
        }
        else if (collision.gameObject.layer == 3)
            ScalePlayer();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canCollide)
            Destroy(gameObject);
        canCollide = true;
        lastPos = transform.position;
        timer = 10;
    }
    void ScalePlayer()
    {
        player.transform.localScale = Mathf.Clamp(player.transform.localScale.x * (bulletType == BulletType.blue ? .5f : 2), .5f, 2) * Vector2.one;
    }
}

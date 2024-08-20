using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
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
    private bool canHitPlayer = false;
    private int timer = 0;
    private int killTimer = 1000;

    void Start()
    {
        colliders = GetComponents<Collider2D>();
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
        if (killTimer == 0)
            Destroy(gameObject);
        killTimer--;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used.Contains(collision.gameObject)) 
            return;
        if (collision.gameObject.layer != 9)
            used.Add(collision.gameObject);
        if (collision.gameObject.layer == 7)
        {
            wall(collision);
        }
        else if (collision.gameObject.layer == 9)
        {
            mirror();
        }
        else if (collision.gameObject.layer == 3)
            ScalePlayer();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canCollide)
        {
            if (collision.gameObject.layer == 7)
                wall(collision.collider);
            else if (collision.gameObject.layer == 9)
                mirror();
            else if (collision.gameObject.layer != 3)
                Destroy(gameObject);
            else if (canHitPlayer)
                ScalePlayer();
        }   
        canCollide = true;
        lastPos = transform.position;
        timer = 10;
    }
    void wall(Collider2D collision)
    {
        transform.parent = collision.transform;
        Vector2 pos = transform.localPosition;
        int dir = (Mathf.Abs(pos.x) < Mathf.Abs(pos.y) ? 2 : 1);
        dir *= pos[dir - 1] < 0 ? -1 : 1;
        Expand expand = collision.gameObject.AddComponent<Expand>();
        expand.Data(dir, (bulletType == BulletType.blue ? .5f : 2));
        Destroy(gameObject);
    }
    void mirror()
    {
        foreach (Collider2D collider in colliders)
            collider.excludeLayers = excludeLayers;
        canCollide = false;
        canHitPlayer = true;
    }
    void ScalePlayer()
    {
        player.GetComponent<PlayerMovment>().size = Mathf.Clamp(Mathf.Abs(player.transform.localScale.x) * (bulletType == BulletType.blue ? .5f : 2), .5f, 2f);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = dir;
        Debug.Log(transform.rotation.eulerAngles.z);
        Vector3 playerLocalScale = player.localScale;
        if (transform.rotation.eulerAngles.z > 10 && transform.rotation.eulerAngles.z < 170)
            playerLocalScale.x = -1f; 
        else if (transform.rotation.eulerAngles.z < 350 && transform.rotation.eulerAngles.z > 190)
            playerLocalScale.x = 1f;
        player.localScale = playerLocalScale;
    }
}

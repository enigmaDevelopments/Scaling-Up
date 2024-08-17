using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArmPositioner : MonoBehaviour
{
    public SpriteRenderer sr;

    void Update()
    {
        Vector2 pos = new Vector2();
        pos.x = -0.111f;
        if (sr.sprite.name == "player upSprite")
        {
            pos.y = 1.042f;
        }
        else
        {
            pos.y = 1.128f;
        }
        transform.localPosition = pos;
    }
}

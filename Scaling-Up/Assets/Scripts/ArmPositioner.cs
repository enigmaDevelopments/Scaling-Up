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
        if (sr.sprite.name == "player upSprite")
        {
            pos.y = -0.1397f;
        }
        else
        {
            pos.y = 0f;
        }
        transform.localPosition = pos;
    }
}

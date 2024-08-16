using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Velocity : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        transform.position += transform.up * speed;
    } 
}

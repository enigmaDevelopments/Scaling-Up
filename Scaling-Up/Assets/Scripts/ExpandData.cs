using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExpandData : MonoBehaviour
{
    public LayerMask mask;
    public float speed = .2f;
    private void Start()
    {
        mask = ~mask;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, transform.position,transform.rotation).GetComponent<Bullet>().bulletType = BulletType.blue;
        } else if (Input.GetMouseButtonDown(1))
        {
            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>().bulletType = BulletType.pink;
        }
    }
}

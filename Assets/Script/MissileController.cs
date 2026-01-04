using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
   public float Missilespeed = 25f;

    // Update is called once per frame
    void Update()
    {
         transform.Translate(Vector3.up * Missilespeed * Time.deltaTime);
    }
void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

           GameObject gm = Instantiate(GameManager.instance.ParticleEffect, transform.position, Quaternion.identity);
           Destroy(gm, 2f);
         Destroy(collision.gameObject);
            Destroy(gameObject); 
            }

    }
}
  

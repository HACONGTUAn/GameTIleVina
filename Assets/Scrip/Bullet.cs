using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigitbody;
    [SerializeField] float bulleSpeed = 10f;
    PlayerMoveing player;
    float xSpeed;
    // Start is called before the first frame update
    void Start()
    {
        myRigitbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMoveing>();
        xSpeed = player.transform.localScale.x * bulleSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRigitbody.velocity = new Vector2(xSpeed, 0f);
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);

    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}

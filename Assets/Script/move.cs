using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    bool ismove = false;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5);

    }
    private void FixedUpdate()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }
    }
    void Think()
    {
        nextMove = Random.Range(-1, 2);
        Invoke("Think", 5);
    }
}

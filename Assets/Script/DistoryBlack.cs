using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistoryBlack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
            GameManager.instance.isBlackTrue = true;
        }
    }
    private void Update()
    {
        if (GameManager.instance.isCharDie == true)
        {
            Destroy(gameObject);
            GameManager.instance.isBlackTrue = true;
        }
    }
}

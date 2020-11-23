using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFloor : MonoBehaviour
{
    public GameObject player;
    public GameObject platformPrefab;
    private GameObject myPlat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            Destroy(collision.gameObject);
            GameManager.instance.platforms.Add(myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-1.73f, 1.73f), GameManager.instance.lately.y + (2 + Random.Range(0.5f, 1f))), Quaternion.identity));
            GameManager.instance.lately = myPlat.transform.position;
        }
        
        if(collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }
}

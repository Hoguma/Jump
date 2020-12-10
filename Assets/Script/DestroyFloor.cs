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
            GameManager.instance.platforms.Add(myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-300, 300), GameManager.instance.lately.y + (384 + Random.Range(150f, 250f))), Quaternion.identity));

            GameManager.instance.lately = myPlat.transform.position;
        }
        
        if(collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }
}

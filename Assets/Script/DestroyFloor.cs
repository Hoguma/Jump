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
        if(collision.gameObject.tag == "floor")
        {
            Destroy(collision.gameObject);
            myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-1.73f, 1.73f), player.transform.position.y + (2 + Random.Range(0.5f, 1f))), Quaternion.identity);

        }
    }
}

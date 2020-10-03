using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject platformPrefab;
    private GameObject myPlat;

    // Update is called once per frame
    void Start()
    {
        //for(int i = 0; i<3; i++)
        //{
        //    myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-3f, 3f), player.transform.position.y + (2 + Random.Range(0.5f, 1f))), Quaternion.identity);
        //}
    }
}

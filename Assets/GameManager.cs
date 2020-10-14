using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject platformPrefab;
    private GameObject myPlat;
    [Header ("Wall")]
    public GameObject wall;
    [Header("Score")]
    public Text FScore;
    public float Scopos;
    public float FScore1;


    private void Awake()
    {
        Instantiate(wall, new Vector3(0, 0, 0), transform.rotation);
        Instantiate(wall, new Vector3(0, 10, 0), transform.rotation);
    }
    void Start()
    {
        //for(int i = 0; i<3; i++)
        //{
        //    myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-3f, 3f), player.transform.position.y + (2 + Random.Range(0.5f, 1f))), Quaternion.identity);
        //}
    }
    private void Update()
    {
        Scopos = player.transform.position.y;
        FScore1 = (int)Scopos;
        FScore.text = FScore1.ToString()+"m";
    }
}

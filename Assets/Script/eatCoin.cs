using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eatCoin : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetBool("isEatCoin", true);
            Invoke("DistroyCoin", 0.56f);
        }
    }
    void DistroyCoin()
    {
        Destroy(this.gameObject);
        //코인 1회 증가로 변경해야함
        PlayerPrefs.SetInt("CoinCount", PlayerPrefs.GetInt("CoinCount") + 1);
        gameObject.GetComponent<eatCoin>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eatCoin : MonoBehaviour
{
    Animator animator;
    //사운드
    public AudioSource myFx;
    public AudioClip coineat;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            eatcoinSound();
            animator.SetBool("isEatCoin", true);
            Invoke("DistroyCoin", 0.56f);
        }
    }
    void DistroyCoin()
    {
        Destroy(this.gameObject);
        PlayerPrefs.SetInt("CoinCount", PlayerPrefs.GetInt("CoinCount") + 1);
        GameManager.instance.UpDateCoinData();
        gameObject.GetComponent<eatCoin>().enabled = false;
    }
    public void eatcoinSound()
    {
        if (GameManager.instance.isSoundon == true)
        {
            myFx.PlayOneShot(coineat);
        }
    }
}

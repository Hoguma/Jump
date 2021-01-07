using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Sprite[] character;
    public RuntimeAnimatorController[] animators;
    public bool[] igotthis;
    public Button isiGot;
    public Sprite[] SorB;
    public Sprite[] charName;
    public Image nametag;
    public Text charCost;

    int num = 0;

    //사운드
    public AudioSource myFx;
    public AudioClip clickSound;

    public void Update()
    {
        if (igotthis[num])
        {
            isiGot.image.sprite = SorB[0];
            charCost.gameObject.SetActive(false);
        }
        else
        {
            isiGot.image.sprite = SorB[1];
            charCost.gameObject.SetActive(true);
            charCost.text = "1";
        }
    }

    public void Next()
    {
        UiClickSound();
        if (num + 1 < character.Length)
        {
            num++;
            Player.Instance.spriteRenderer.sprite = character[num];
            Player.Instance.anim.runtimeAnimatorController = animators[num];
            nametag.sprite = charName[num];
        }
    }

    public void Prev()
    {
        UiClickSound();
        if (num - 1 >= 0)
        {
            num--;
            Player.Instance.spriteRenderer.sprite = character[num];
            Player.Instance.anim.runtimeAnimatorController = animators[num];
            nametag.sprite = charName[num];
        }
    }
    
    public void Select()
    {
        UiClickSound();
        if (igotthis[num] == true)
            GameManager.instance.ShopOnOff();
        else
        {
            if(PlayerPrefs.GetInt("CoinCount", 0) > 1)
            {
                PlayerPrefs.SetInt("CoinCount", PlayerPrefs.GetInt("CoinCount", 0) - 1);
                igotthis[num] = true;
            }
            else
            {
                return;
            }
        }
    }
    public void UiClickSound()
    {
        myFx.PlayOneShot(clickSound);
    }
}

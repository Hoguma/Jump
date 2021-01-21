using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private static Shop _instance = null;

    public static Shop instance
    {
        get
        {
            if (_instance == null)
                return null;
            return _instance;
        }
    }

    public GameObject[] tail;

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

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

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
        //캐릭터 뒷 이미지
        if(num == 0)
        {
            tail[0].SetActive(true);
            tail[1].SetActive(false);
            tail[2].SetActive(false);
            tail[3].SetActive(false);
            tail[4].SetActive(false);
            tail[5].SetActive(false);
            tail[6].SetActive(false);
        }
        else if (num == 1)
        {
            tail[0].SetActive(false);
            tail[1].SetActive(true);
            tail[2].SetActive(false);
            tail[3].SetActive(false);
            tail[4].SetActive(false);
            tail[5].SetActive(false);
            tail[6].SetActive(false);
        }
        else if (num == 2)
        {
            tail[0].SetActive(false);
            tail[1].SetActive(false);
            tail[2].SetActive(true);
            tail[3].SetActive(false);
            tail[4].SetActive(false);
            tail[5].SetActive(false);
            tail[6].SetActive(false);
        }
        else if (num == 3)
        {
            tail[0].SetActive(false);
            tail[1].SetActive(false);
            tail[2].SetActive(false);
            tail[3].SetActive(true);
            tail[4].SetActive(false);
            tail[5].SetActive(false);
            tail[6].SetActive(false);
        }
        else if (num == 4)
        {
            tail[0].SetActive(false);
            tail[1].SetActive(false);
            tail[2].SetActive(false);
            tail[3].SetActive(false);
            tail[4].SetActive(true);
            tail[5].SetActive(false);
            tail[6].SetActive(false);
        }
        else if (num == 5)
        {
            tail[0].SetActive(false);
            tail[1].SetActive(false);
            tail[2].SetActive(false);
            tail[3].SetActive(false);
            tail[4].SetActive(false);
            tail[5].SetActive(true);
            tail[6].SetActive(false);
        }
        else if (num == 6)
        {
            tail[0].SetActive(false);
            tail[1].SetActive(false);
            tail[2].SetActive(false);
            tail[3].SetActive(false);
            tail[4].SetActive(false);
            tail[5].SetActive(false);
            tail[6].SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Sprite[] character;
    public bool[] igotthis;
    int num = 0;

    public void Next()
    {
        if (num + 1 < character.Length)
        {
            num++;
            Player.Instance.spriteRenderer.sprite = character[num];
        }
    }

    public void Prev()
    {
        if (num - 1 >= 0)
        {
            num--;
            Player.Instance.spriteRenderer.sprite = character[num];
        }
    }
    
    public void Select()
    {
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
}

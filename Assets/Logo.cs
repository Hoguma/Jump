using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        img.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private GameObject target;
    private Collider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        target = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.transform.position.y - (0.6371382 * 0.65) > transform.position.y)
        {
            coll.enabled = true;
        }
        else
            coll.enabled = false;
    }

}

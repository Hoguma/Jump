﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 Dpos;
    Vector3 wallpos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Dpos = GameObject.Find("Down").transform.position;
        wallpos = gameObject.transform.position;
        if (Dpos.y >= gameObject.transform.position.y)
        {
            wallpos.y += 20.0f;
            gameObject.transform.position = wallpos;
        }
    }
}
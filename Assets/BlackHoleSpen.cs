using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpen : MonoBehaviour
{
    public float rotateSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}

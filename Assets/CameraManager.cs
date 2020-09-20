using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private void Update()
    {
        if(transform.position.y < target.transform.position.y)
        {
            transform.position = new Vector3 (0, target.transform.position.y, transform.position.z);
        }
    }
}

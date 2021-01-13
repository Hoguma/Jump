using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public enum Kind { Lightning, Risk, NONE = 99 };
    public Kind destroyKind = Kind.NONE;
    
    void Start()
    {
        switch (destroyKind)
        {
            case Kind.Lightning:
                Destroy(gameObject, 0.8f);
                break;
            case Kind.Risk:
                Destroy(gameObject, 1.5f);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float power = 0.5f;
    [SerializeField]
    private float maxDrag = 1f;
    [SerializeField]
    private Rigidbody2D rigidbody;
    [SerializeField]
    private LineRenderer line;

    Vector3 dragStartPos;
    Touch touch;

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Dragging();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                DragRelease();
            }
        }
    }

    void DragStart()
    {
        //dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragStartPos.z = 0f;
        line.positionCount = 1;
        line.SetPosition(0, dragStartPos);
    }
    void Dragging()
    {
        //Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        draggingPos.z = 0f;
        line.positionCount = 2;
        line.SetPosition(1, draggingPos);
    }
    void DragRelease()
    {
        line.positionCount = 0;
        //Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragReleasePos.z = 0f;

        Vector3 force = dragStartPos - dragReleasePos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

        rigidbody.AddForce(clampedForce, ForceMode2D.Impulse);
    }
    private void OnMouseDown()
    {
        DragStart();
    }
    private void OnMouseDrag()
    {
        Dragging();
    }
    private void OnMouseUp()
    {
        DragRelease();
    }
}

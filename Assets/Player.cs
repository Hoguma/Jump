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

    [SerializeField]
    private GameObject PointPre;
    [SerializeField]
    private GameObject[] Points;

    public int numberOfpoints;

    Vector3 dragStartPos;
    Touch touch;

    Vector3 clampedForce = Vector3.zero;

    //벽 슬라이딩?
    float isRight = 1;
    bool isWall;
    public Transform wallChk;
    public float wallchkDistance;
    public float slidingSpeed;
    public LayerMask w_Layer;

    private void Start()
    {
        Points = new GameObject[numberOfpoints];

        for(int i = 0; i < numberOfpoints; i++)
        {
            Points[i] = Instantiate(PointPre, transform.position, Quaternion.identity);
        }
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isWall)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * slidingSpeed);
        }
    }
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
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, w_Layer);

        for(int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position = PointPosition(i * 0.1f);
        }
    }

    void DragStart()
    {
        //dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragStartPos.z = 0f;
        line.positionCount = 1;
        line.SetPosition(0, dragStartPos);
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(true);
        }
        Vector3 force = dragStartPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
    }
    void Dragging()
    {
        //Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        draggingPos.z = 0f;
        line.positionCount = 2;
        line.SetPosition(1, draggingPos);
        Vector3 force = dragStartPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
    }
    void DragRelease()
    {
        line.positionCount = 0;
        //Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragReleasePos.z = 0f;
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(false);
        }
        Vector3 force = dragStartPos - dragReleasePos;
        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos = (Vector2)transform.position + ((Vector2)clampedForce * t) + 0.5f * Physics2D.gravity * 4 * (t * t);
        return currentPointPos;
    }
}

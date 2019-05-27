using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ViewPoint
{
    Right = 0,　// 真上
    Diagonally = 1, // 斜め上
}

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    ViewPoint viewPoint = ViewPoint.Right; 

    void InitCameraPosition()
    {
        switch (viewPoint)
        {
            case ViewPoint.Diagonally:
                break;
            default: //case ViewPoint.Right:
                //transform.position = new vector3
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - new Vector3(0, 0, 0); //target.position;
        Debug.Log(name + ": offset=" + offset);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmp = target.position + offset;
        transform.position = new Vector3(tmp.x, tmp.y, 0);
    }
}

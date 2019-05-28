using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ViewPoint
{
    Right = 0,　// 真上
    Diagonally = 1, // 斜め上
    Dummy = 2,
}

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    ViewPoint viewPoint = ViewPoint.Right;
    float posY;
    float posZ;

    void InitCameraPosition(ViewPoint vp = ViewPoint.Right)
    {
        switch (vp)
        {
            case ViewPoint.Diagonally:
                offset = new Vector3(0, 6, -7) - new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(52, 0, 0);
                posZ = -7;
                break;
            default: //case ViewPoint.Right:
                offset = new Vector3(0, 11, 0) - new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(90, 0, 0);
                posZ = 0;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            viewPoint++;
            if (viewPoint == ViewPoint.Dummy) viewPoint = 0;
            Debug.Log("vp = " + viewPoint);
            InitCameraPosition(viewPoint);
        }

        Vector3 tmp = target.position + offset;
        transform.position = new Vector3(tmp.x, tmp.y, posZ); // Diagonally
    }
}

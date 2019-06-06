using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewPoint
{
    Right = 0,　// 真上
    Diagonally = 1, // 斜め上
    Dummy = 2,
}

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    float posY;
    float posZ;

    ViewPoint vp;

    // Start is called before the first frame update
    void Start()
    {
        vp = GameController.GetViewPoint();
        InitCameraPosition(vp);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            vp++;
            if (vp == ViewPoint.Dummy) vp = 0;
            InitCameraPosition(vp);
        }

        Vector3 tmp = target.position + offset;
        if (vp == ViewPoint.Diagonally)
        {
            transform.position = tmp;
        }
        else
        {
            transform.position = new Vector3(tmp.x, tmp.y, posZ); // Diagonally
        }
    }

    public ViewPoint GetViewPoint()
    {
        return this.vp;
    }

    void InitCameraPosition(ViewPoint v = ViewPoint.Right)
    {
        switch (v)
        {
            case ViewPoint.Diagonally:
                offset = new Vector3(0, 5.5f, -6f) - new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(50, 0, 0);
                break;
            default: //case ViewPoint.Right:
                offset = new Vector3(0, 11.5f, 0.5f) - new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
        }
        posZ = 0.5f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ViewPoint
{
    Right = 0,　// 真上
    Diagonally = 1, // 斜め上
    TPP = 2,
    FPP = 3,
}

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    float posY;
    float posZ;

    public Text debugtext;

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
            int vptmp = (int)vp + 1;
            if (vptmp > System.Enum.GetValues(typeof(ViewPoint)).Length - 1) vptmp = 0;
            vp = (ViewPoint)vptmp;
            InitCameraPosition(vp);

            debugtext.text = vp.ToString();
        }

        Vector3 tmp = target.position + offset;

        switch (vp) {
            case ViewPoint.Right:
                transform.position = new Vector3(tmp.x, tmp.y, posZ); // Diagonally
                break;
            case ViewPoint.TPP:
                transform.position = tmp; // Diagonally
                transform.LookAt(target);
                break;
            default:
                transform.position = tmp;
                break;
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
                //offset = new Vector3(0, 5.5f, -6f) - new Vector3(0, 0, 0);
                offset = new Vector3(0, 5.5f, -5f) - new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(50, 0, 0);
                break;
            case ViewPoint.TPP:
                offset = new Vector3(0, 3f, 3f) - new Vector3(0, 0, 0);
                //transform.rotation = Quaternion.Euler(45, 180, 0);
                break;
            case ViewPoint.FPP:
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

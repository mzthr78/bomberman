using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ViewPoint
{
    Right = 0,　// 真上
    Diagonally = 1, // 斜め上
    Dummy = 2,
}

public enum BObjects
{
    None,
    Player,
    Bomb,
    Door,
    HardBlock,
    SoftBlock,
    PowerUpItem,
    Enemy,
}

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    ViewPoint viewPoint = ViewPoint.Right;
    float posY;
    float posZ;

    List<List<BObjects>> bobjs;

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
        if (viewPoint == ViewPoint.Diagonally)
        {
            transform.position = tmp;
        }
        else
        {
            transform.position = new Vector3(tmp.x, tmp.y, posZ); // Diagonally
        }
    }

    void InitCameraPosition(ViewPoint vp = ViewPoint.Right)
    {
        switch (vp)
        {
            case ViewPoint.Diagonally:
                offset = new Vector3(0, 3.5f, -5f) - new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(50, 0, 0);
                break;
            default: //case ViewPoint.Right:
                offset = new Vector3(0, 11, 0) - new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
        }
        posZ = 0;
    }

    int maxRow = 13;
    int maxCol = 31;

    void InitBObjects()
    {
        for (int i = 0; i < maxRow; i++)
        {
            List<BObjects> tmpLine = new List<BObjects>();

            for (int j = 0; j < maxCol; j++)
            {
                BObjects tmpCol = BObjects.None;

                // top, bottom, right, left wall
                if (i == 0 || i == (maxRow - 1) || j == 0 || j == (maxCol - 1))
                {
                    tmpCol = BObjects.HardBlock;
                }
                // HardBlock
                else if (i % 2 == 0 && j % 2 ==0)
                {
                    tmpCol = BObjects.HardBlock;
                }
                // Player
                else if (i == 1 && j == 1)
                {
                    tmpCol = BObjects.Player;
                }
                // Player right side, bottom side
                else if ((i == 1 && j == 2) || (i == 2 && j == 1))
                {

                }

                tmpLine.Add(tmpCol);
            }

            bobjs.Add(tmpLine);
        }
    }
}

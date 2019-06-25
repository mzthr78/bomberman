using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallomController : MonoBehaviour
{
    public GameObject GameController;
    GameController controller;

    public Transform player;

    public GameObject debugPanelPrefab;

    int stageWidth = 0;
    int stageHeight = 0;

    private void Awake()
    {
        controller = GameController.GetComponent<GameController>();
    }

    Vector3 startPos;
    Vector3 markerPos;

    float posY = 0;

    bool freeze = false;
    float speed = 0.033f;

    Queue<Addr> queTarget;

    public void Freeze(bool b = true)
    {
        this.freeze = b;
    }

    public void UnFreeze()
    {
        Freeze(false);
    }

    private void Start()
    {
        posY = transform.position.y;
        markerPos = transform.position;

        queTarget = new Queue<Addr>();

        stageWidth = controller.GetStageWidth();
        stageHeight = controller.GetStageHeight();

        freeze = false;

        Wander();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            freeze = !freeze;
        }

        if (freeze) return;

        float distance = Vector3.Distance(transform.position, markerPos);

        if (distance < 0.05)
        {
            if (queTarget.Count > 0)
            {
                Addr tmp = queTarget.Dequeue();

                markerPos = new Vector3(tmp.x - 15, posY, 6 - tmp.z);
            }
            else
            {
                StartCoroutine(Robe());

                Addr tmp = queTarget.Dequeue();
                markerPos = new Vector3(tmp.x - 15, posY, 6 - tmp.z);

            }
        }
        else
        {
            float rx = Mathf.Round(transform.position.x);
            float ry = Mathf.Round(transform.position.y);
            float rz = Mathf.Round(transform.position.z);

            Vector3 rp = new Vector3(rx, ry, rz);

            if (Vector3.Distance(transform.position, rp) < 0.05f)
            {
                bool isEmpty = false;

                Vector3 left = new Vector3(-1, 0, 0);
                Vector3 right = new Vector3(1, 0, 0);
                Vector3 up = new Vector3(0, 0, 1);
                Vector3 down = new Vector3(0, 0, -1);

                Vector3 fd = transform.forward;
                Vector3 pos;

                if (fd == left || fd == right)
                {
                    pos = transform.position + up;
                    //Debug.Log(transform.position + "↑" + pos + " " + controller.GetObj(pos));

                    pos = transform.position + down;
                    //Debug.Log(transform.position + "↓" + pos + " " + controller.GetObj(pos));

                    if (IsEmpty(controller.GetObj(transform.position + up))) isEmpty = true;
                    if (IsEmpty(controller.GetObj(transform.position + down))) isEmpty = true;

                    Debug.Log(pos + " is " + isEmpty);
                }
                else
                {
                    Debug.Log("up or down");
                    if (IsEmpty(controller.GetObj(transform.position + left))) isEmpty = true;
                    if (IsEmpty(controller.GetObj(transform.position + right))) isEmpty = true;
                }

                if (isEmpty) Debug.Log("!!! Empty !!! " + " " + transform.position);
            }

            Toward(markerPos);
        }
    }

    bool IsEmpty(BMObj obj)
    {
        switch (obj)
        {
            case BMObj.HardBlock:
            case BMObj.SoftBlock:
            case BMObj.Bomb:
                return false;
            default:
                return true;
        }
    }

    // キョロキョロ
    IEnumerator Robe()
    {
        ChangeDirection(Direction.Down);

        float waitsec = Random.Range(0, 40) * 0.1f + 0.2f;

        yield return new WaitForSeconds(waitsec);

        Wander();
    }

    Direction PreDir = Direction.None;

    // ウロウロ
    private void Wander()
    {
        if (queTarget.Count > 0) queTarget.Clear();

        /*
        queTarget.Enqueue(new Addr(17, 5));
        queTarget.Enqueue(new Addr(17, 4));
        queTarget.Enqueue(new Addr(17, 3));
        queTarget.Enqueue(new Addr(17, 2));
        queTarget.Enqueue(new Addr(17, 1));
        queTarget.Enqueue(new Addr(17, 2));
        queTarget.Enqueue(new Addr(17, 3));
        queTarget.Enqueue(new Addr(17, 4));
        queTarget.Enqueue(new Addr(17, 5));
        queTarget.Enqueue(new Addr(17, 6));
        queTarget.Enqueue(new Addr(17, 7));
        */

        List<Direction> SearchDir = new List<Direction>();
        int[] dx = { 0, 0, 0, 0 };
        int[] dz = { 0, 0, 0, 0 };

        switch (PreDir)
        {
            case Direction.Left:
                SearchDir.Add(Direction.Right);
                SearchDir.Add(Direction.Left);
                SearchDir.Add(Direction.Up);
                SearchDir.Add(Direction.Down);

                int[] dx_right = { 1, -1, 0, 0 };
                int[] dz_right = { 0, 0, 1, -1 };

                dx = dx_right;
                dz = dz_right;

                break;
            case Direction.Up:
                SearchDir.Add(Direction.Down);
                SearchDir.Add(Direction.Up);
                SearchDir.Add(Direction.Left);
                SearchDir.Add(Direction.Right);

                int[] dx_down = { 0, 0, -1, 1 };
                int[] dz_down = { -1, 1, 0, 0 };

                dx = dx_down;
                dz = dz_down;

                break;
            case Direction.Down:
                SearchDir.Add(Direction.Up);
                SearchDir.Add(Direction.Down);
                SearchDir.Add(Direction.Left);
                SearchDir.Add(Direction.Right);

                int[] dx_up = { 0, 0, -1, 1 };
                int[] dz_up = { 1, -1, 0, 0 };

                dx = dx_up;
                dz = dz_up;

                break;
            default:
                SearchDir.Add(Direction.Left);
                SearchDir.Add(Direction.Right);
                SearchDir.Add(Direction.Up);
                SearchDir.Add(Direction.Down);

                int[] dx_left = { -1, 1, 0, 0 };
                int[] dz_left = { 0, 0, 1, -1 };

                dx = dx_left;
                dz = dz_left;

                break;
        }

        Direction SeekDir = Direction.None;

        BMObj obj;
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = transform.position + new Vector3(dx[i], transform.position.y, dz[i]);
            obj = controller.GetObj(pos);

            switch (obj)
            {
                case BMObj.HardBlock:
                case BMObj.SoftBlock:
                    break;
                default:
                    SeekDir = SearchDir[i];
                    PreDir = SeekDir;
                    break;
            }

            if (SeekDir != Direction.None) break;
        }

        int[] dx_tmp = { 1, 0, -1, 0 };
        int[] dz_tmp = { 0, -1, 0, 1 };

        dx = dx_tmp;
        dz = dz_tmp;

        ChangeDirection(SeekDir);

        bool isEmpty = true;

        Vector3 prePos = transform.position + new Vector3(dx[(int)SeekDir], transform.position.y, dz[(int)SeekDir]);
        while (isEmpty)
        {
            Vector3 pos = prePos + new Vector3(dx[(int)SeekDir], prePos.y, dz[(int)SeekDir]);
            obj = controller.GetObj(pos);

            switch (obj)
            {
                case BMObj.HardBlock:
                case BMObj.SoftBlock:
                    isEmpty = false;
                    break;
                default:
                    prePos = pos;
                    break;
            }
        }
        queTarget.Enqueue(controller.Pos2Addr(prePos));
    }

    void Toward(Vector3 pos)
    {
        Direction d = Direction.None;

        Vector3 sub = transform.position - pos;

        if (Mathf.Abs(sub.x) > 0.1f)
        {
            if (sub.x > 0) d = Direction.Left;
            else d = Direction.Right;
        }
        else if (Mathf.Abs(sub.z) > 0.1f)
        {
            if (sub.z > 0) d = Direction.Down;
            else d = Direction.Up;
        }

        Move(d);
    }

    void ChangeDirection(Direction d)
    {
        switch (d)
        {
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case Direction.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            default:
                break;
        }
    }

    void Move(Direction d)
    {
        ChangeDirection(d);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
        {
            if (IsObstruct(hit.transform.tag))
            {
                Debug.Log("enemy hit " + hit.transform.tag);

                //queTarget.Clear();
            }
        }

        transform.position += transform.forward * speed;
        //transform.Translate(transform.forward * 0.01f); // これだとダメ
    }

    bool IsObstruct(BMObj bo)
    {
        switch (bo)
        {
            case BMObj.HardBlock:
            case BMObj.SoftBlock:
            case BMObj.Bomb:
                return true;
            default:
                return false;
        }
    }

    bool IsObstruct(string s)
    {
        switch (s)
        {
            case "HardBlock":
            case "SoftBlock":
            case "Bomb":
                return true;
            default:
                return false;
        }
    }

    void PutPanel(Addr a, int n, bool b = false)
    {
        Vector3 pos = new Vector3(a.x - 15, -0.45f, 6 - a.z);
        GameObject infoPanel = Instantiate(debugPanelPrefab, pos, Quaternion.identity);
        infoPanel.GetComponent<DebugPanelController>().SetText(n.ToString());

        if (b)
        {
            infoPanel.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

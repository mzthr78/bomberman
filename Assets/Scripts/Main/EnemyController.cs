using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    None = -1,
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3,
}

enum EnemyAction
{
    Idle,
    Robe,
    Wander,
}

public class EnemyController : MonoBehaviour
{
    public GameObject GameController;
    GameController controller;

    public Transform player;
    public GameObject targetObj;

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
    float speed = 0.035f;

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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            freeze = !freeze;
            Wander();
        }

        if (freeze) return;

        float distance = Vector3.Distance(transform.position, markerPos);

        if (distance < 0.05)
        {
            if (queTarget.Count > 0)
            {
                Addr tmp = queTarget.Dequeue();

                // ここでオブジェクトチェック

                markerPos = new Vector3(tmp.x - 15, posY, 6 - tmp.z);
            } else
            {
                Wander();

                Addr tmp = queTarget.Dequeue();
                markerPos = new Vector3(tmp.x - 15, posY, 6 - tmp.z);

            }
        }
        else
        {
            Toward(markerPos);
        }
    }

    void Toward(Vector3 pos)
    {
        Direction d = Direction.None;

        Vector3 sub = transform.position - pos;
        //Debug.Log("sub="+sub);

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

        //Debug.Log("(toward) d = " + d + " transform.position=" + transform.position + " target=" + target);

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
        //if (Physics.Raycast(transform.position, transform.forward, out hit, 28)) {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1)) {
            if (IsObstruct(hit.transform.tag))
            {
                Debug.Log("enemy hit " + hit.transform.tag);

                queTarget.Clear();

                //return;
            }
        }

        transform.position += transform.forward * speed;

        //transform.Translate(transform.forward * 0.01f); // これだとダメ
    }

    // キョロキョロ
    void Robe()
    {

    }

    Direction preDir = Direction.None;

    // ウロウロ
    private void Wander()
    {
        Direction[] SearchDir = { Direction.Left, Direction.Right, Direction.Up, Direction.Down };

        int[] dx = { -1, 1, 0, 0 };
        int[] dz = { 0, 0, 1, -1 };


        Vector3 tmpPos = new Vector3(0, 0, 0);

        bool find = false;

        for (int i = 0; i < 4; i++)
        {
            ChangeDirection(SearchDir[i]);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 28))
            {
                switch (hit.transform.tag)
                {
                    case "HardBlock":
                    case "SoftBlock":
                        if (Vector3.Distance(transform.position, hit.transform.position) > 1.05f)
                        {
                            tmpPos = hit.transform.position - new Vector3(dx[i], 0, dz[i]);
                            find = true;
                        }
                        break;
                    case "Player":
                        if (Vector3.Distance(transform.position, hit.transform.position) > 1.05f)
                        {
                            tmpPos = hit.transform.position;
                            find = true;
                        }
                        break;
                    default:
                        break;
                }
            }

            targetObj.transform.position = tmpPos;

            Debug.Log("dir=" + SearchDir[i] + " find? " + find);
            Debug.Log("pos=" + transform.position + " hit(" + hit.transform.tag + ")=" + hit.transform.position);
            Debug.Log("distance=" + Vector3.Distance(transform.position, hit.transform.position));

            if (find) break;
        }

        SearchShortestPath(targetObj.transform.position);
    }

    void SearchShortestPath(Vector3 targetPos)
    {
        queTarget.Clear();

        float rx;
        float rz;

        rx = Mathf.Round(transform.position.x);
        rz = Mathf.Round(transform.position.z);
        Addr start = new Addr((int)rx + 15, 6 - (int)rz);

        //rx = Mathf.Round(player.position.x);
        //rz = Mathf.Round(player.position.z);
        rx = Mathf.Round(targetPos.x);
        rz = Mathf.Round(targetPos.z);

        Addr target = new Addr((int)rx + 15, 6 - (int)rz);

        BFS(start, target, false);

        int counter = 0;
        bool find = false;

        Addr curr = target;

        List<Addr> shortest = new List<Addr>();
        shortest.Add(curr);

        while (!find && counter < 10000)
        {
            curr = route[curr.z][curr.x];
            //shortest.Push(curr);
            shortest.Add(curr);

            if (curr.x == start.x && curr.z == start.z)
            {
                find = true;
                break;
            }

            counter++;
        }

        shortest.Reverse();

        GameObject[] panels = GameObject.FindGameObjectsWithTag("Debug");
        foreach (GameObject panel in panels)
        {
            Destroy(panel);
        }

        for (int i = 0; i < shortest.Count; i++)
        {
            PutPanel(shortest[i], i, false);
            //Debug.Log("shortest[" + i + "] = (" + shortest[i].x + ", " + shortest[i].z);
            queTarget.Enqueue(shortest[i]);
        }
    }

    List<List<Addr>> route;

    bool BFS(Addr start, Addr goal, bool full = false, bool debug = false)
    {
        bool find = false;

        // BFS variables;
        int width = 31;
        int height = 13;

        List<List<bool>> visited = new List<List<bool>>();
        route = new List<List<Addr>>();

        // BFS Init
        List<List<BMObj>> map = controller.GetMap();
        Queue<Addr> que = new Queue<Addr>();

        for (int i = 0; i < height; i++)
        {
            List<bool> linev = new List<bool>();
            List<Addr> linea = new List<Addr>();

            for (int j = 0; j < width; j++)
            {
                linev.Add(false);
                linea.Add(new Addr(-1, -1));
            }
            visited.Add(linev);
            route.Add(linea);
        }

        visited[start.z][start.x] = true;
        que.Enqueue(start);
        route[start.z][start.x] = start;

        int[] dx = { -1, 0, 1, 0 };
        int[] dz = { 0, 1, 0, -1 };

        int BFSCount = 0;

        while (que.Count > 0)
        {
            Addr curr = que.Dequeue();
            if (debug) PutPanel(curr, BFSCount, false);

            if (curr.x == goal.x && curr.z == goal.z)
            {
                find = true;

                if (!full) return find;
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = curr.x + dx[i];
                int nz = curr.z + dz[i];

                if (nz >=0 && nz < height && nx >= 0 && nx < width)
                {
                    if (!visited[nz][nx] && !IsObstruct(map[nz][nx]))
                    {
                        visited[nz][nx] = true;
                        que.Enqueue(new Addr(nx, nz));
                        route[nz][nx] = curr;
                    }
                }
            }
            BFSCount++;
        }
        return find;
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

    bool IsObstruct(string t)
    {
        switch (t)
        {
            case "HardBlock":
            case "SoftBlock":
            case "Bomb":
                return true;
            default:
                return false;
        }
    }

    void PutPanel(Addr p, int n, bool b = false)
    {
        Vector3 pos = new Vector3(p.x - 15, -0.45f, 6 - p.z);
        GameObject infoPanel = Instantiate(debugPanelPrefab, pos, Quaternion.identity);
        infoPanel.GetComponent<DebugPanelController>().SetText(n.ToString());

        if (b)
        {
            infoPanel.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    //for DFS
    /*
    List<List<BMObj>> map;
    int DFSCount = 0;
    Stack<Addr> DFSroute;

    void SearchWithDFS()
    {
        Debug.Log("search!");

        InitDFS();
        DFS(new Addr(15, 6));

        int a = 0;
        while (DFSroute.Count > 0)
        {
            PutPanel(DFSroute.Pop(), a, true);
            a++;
        }
        Debug.Log("aaa? = " + a);

        Debug.Log("Count = " + DFSCount);
    }

    void InitDFS()
    {
        find = false;

        GameObject[] panels = GameObject.FindGameObjectsWithTag("Debug");
        foreach (GameObject panel in panels)
        {
            Destroy(panel);
        }

        DFSCount = 0;

        visited = new List<List<bool>>();
        map = controller.GetMap();

        for (int i = 0; i < height; i++)
        {
            List<bool> lineb = new List<bool>();
            List<int> linei = new List<int>();

            for (int j = 0; j < width; j++)
            {
                lineb.Add(false);
                linei.Add(-1);
            }

            visited.Add(lineb);
        }

        DFSroute = new Stack<Addr>();
    }

    void DFS(Addr p)
    {
        if (DFSCount > 10000)
        {
            Debug.Log("okashii!");
            return;
        }

        if (find) return;

        int[] dx = { -1, 0, 1, 0 };
        int[] dz = { 0, -1, 0, 1 };

        visited[p.z][p.x] = true;

        PutPanel(p, DFSCount);
        DFSCount++;

        DFSroute.Push(p);

        if (target.x == p.x && target.z == p.z)
        {
            find = true;
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (p.z + dz[i] >= 0 && p.z + dz[i] < height && p.x + dx[i] >= 0 && p.x + dx[i] < width)
            {
                if (!visited[p.z + dz[i]][p.x + dx[i]] && !IsObstruct(map[p.z + dz[i]][p.x + dx[i]]))
                {
                    DFS(new Addr(p.x + dx[i], p.z + dz[i]));

                    if (!find) DFSroute.Pop();
                }
            }
        }
    }
    */
}

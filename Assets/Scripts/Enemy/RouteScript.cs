using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteScript : MonoBehaviour
{
    bool isDebug = false;
    LogController Logger;

    //public GameObject GameController;
    //public Transform RouteAllParent;
    //public Transform RouteShortestParent;
    //public Transform RouteDeadEndParent;

    GameObject GameController;
    Transform RouteAllParent;
    Transform RouteShortestParent;
    Transform RouteDeadEndParent;

    public void SetDebug(bool b = true)
    {
        isDebug = b;
    }

    private GameController controller;
    private List<List<Addr>> shortestArr;

    public GameObject InfoPanelPrefab;

    private void Awake()
    {
        // for Prefab
        GameController = GameObject.Find("GameController");
        RouteAllParent = GameObject.Find("RouteAll").transform;
        RouteShortestParent = GameObject.Find("RouteShortest").transform;
        RouteDeadEndParent = GameObject.Find("RouteDeadEnd").transform;

        //controller = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameController.GetComponent<GameController>();
    }

    private void Start()
    {
        width = controller.GetStageWidth();
        height = controller.GetStageHeight();

        isDebug = true;
    }

    // BFS variables;
    private int width; // = 31;
    private int height; // = 13;
    private bool find;

    // 全探索する場合
    // 全探索して「行き止まり」のいずれかを選択する場合
    // 全探索して適当な一地点を選択する場合

    // 最短経路探索をする場合

    List<Addr> DeadEnd;

    public List<Addr> GetDeadEndOne(Addr start)
    {
        BFS(start, start, true, isDebug);
        return GetShortestRoute(start, DeadEnd[Random.Range(0, DeadEnd.Count)]);
    }

    public List<Addr> GetShortestRoute(Addr start, Addr goal)
    {
        List<Addr> shortest = new List<Addr>();

        BFS(start, goal, true, isDebug);

        Addr curr = goal;
        shortest.Add(curr);

        int count = 0;

        while (count < 1000)
        {
            curr = shortestArr[curr.z][curr.x];
            shortest.Add(curr);

            if (curr.x == start.x && curr.z == start.z) break;
            count++;
        }

        shortest.Reverse();

        //Debug.Log("shortest count = " + shortest.Count);

        /*
        for (int i = 0; i < shortest.Count; i++)
        {
            //Debug.Log(i + " " + controller.Addr2Pos(shortest[i]) + ", (" + shortest[i].x + ", " + shortest[i].z + ")");
            PutPanel(shortest[i], i, Color.blue, RouteShortestParent);
        }
        */

        return shortest;
    }

    public bool BFS(Addr start, Addr goal, bool full = false, bool debug = false)
    {
        // Init
        DeadEnd = new List<Addr>();

        GameObject[] panels = GameObject.FindGameObjectsWithTag("Debug");
        foreach (GameObject panel in panels)
        {
            Destroy(panel);
        }

        find = false;

        List<List<bool>> visited = new List<List<bool>>();
        shortestArr = new List<List<Addr>>();

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
            shortestArr.Add(linea);
        }

        visited[start.z][start.x] = true;
        que.Enqueue(start);
        shortestArr[start.z][start.x] = start;

        int[] dx = { -1, 0, 1, 0 };
        int[] dz = { 0, 1, 0, -1 };

        int BFSCount = 0;

        while (que.Count > 0)
        {
            Addr curr = que.Dequeue();
            // if (debug) PutPanel(curr, BFSCount, false); // for debug

            int count = 0;

            for (int i = 0; i < 4; i++)
            {
                if (!IsObstruct(map[curr.z + dz[i]][curr.x + dx[i]])) count++;
            }

            if (count > 1)
            {
                //if (isDebug) PutPanel(curr, BFSCount, Color.blue);
            } else
            {
                //if (isDebug) PutPanel(curr, BFSCount, Color.blue);
                DeadEnd.Add(curr);
            }

            if (curr.x == goal.x && curr.z == goal.z)
            {
                find = true;

                if (!full) return find;
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = curr.x + dx[i];
                int nz = curr.z + dz[i];

                if (nz >= 0 && nz < height && nx >= 0 && nx < width)
                {
                    if (!IsObstruct(map[nz][nx]))
                    {
                        if (!visited[nz][nx])
                        {
                            visited[nz][nx] = true;
                            que.Enqueue(new Addr(nx, nz));
                            shortestArr[nz][nx] = curr;
                        }
                    }

                }
            }
            BFSCount++;
        }

        return find;
    }

    void PutPanel(Addr a, int n, Color c, Transform parent = null)
    {
        Vector3 pos = new Vector3(a.x - 15, -0.45f, 6 - a.z);
        GameObject infoPanel = Instantiate(InfoPanelPrefab, pos, Quaternion.identity);
        if (parent) infoPanel.transform.parent = parent;

        infoPanel.GetComponent<InfoPanelController>().SetText(n.ToString());
        infoPanel.GetComponent<Renderer>().material.color = c;
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    public GameObject GameController;
    GameController controller;
    public Transform player;
    public GameObject debugPanelPrefab;

    Vector3 startPos;
    Vector3 targetPos;

    bool freeze = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    float lerpspeed = 1.8f;
    float startTime;
    float distance;

    private void StartLerp()
    {
        Vector3 ggg = new Vector3(0, 0, 2);

        startPos = transform.position;
        targetPos = transform.position + ggg;

        startTime = Time.time;
        distance = Vector3.Distance(startPos, targetPos);

    }

    void LerpMove()
    {
        if (freeze) return;

        float passed = (Time.time - startTime) * lerpspeed;
        float delta = passed / distance;

        transform.position = Vector3.Lerp(startPos, targetPos, delta);

        Debug.Log("distance = " + Vector3.Distance(transform.position, targetPos));

        if (Input.GetMouseButtonDown(0))
        {
            freeze = false;
            startTime = Time.time;
        }
    }

    void HitTest()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                float rx = Mathf.Round(hit.point.x);
                float rz = Mathf.Round(hit.point.z);
            }
        }
    }

    void TestBFS()
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Debug");
        foreach (GameObject panel in panels)
        {
            Destroy(panel);
        }

        float rx = Mathf.Round(player.position.x);
        float rz = Mathf.Round(player.position.z);

        Addr target = new Addr((int)rx + 15, 6 - (int)rz);
        Addr start = new Addr(15, 6);
        BFS(start, target, false);

        int counter = 0;
        bool find = false;

        Addr curr = target;

        //Stack<Addr> shortest = new Stack<Addr>();
        //shortest.Push(curr);
        List<Addr> shortest = new List<Addr>();

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

        counter = 0;
        shortest.Reverse();
        //while (shortest.Count > 0)
        while (counter < shortest.Count)
        {
            //PutPanel(shortest.Pop(), counter, true);
            PutPanel(shortest[counter], counter, true);
            counter++;
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

                if (nz >= 0 && nz < height && nx >= 0 && nx < width)
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

    void dummy()
    {
        int[] dx = { -1, 0, 1, 0 };
        int[] dz = { 0, 1, 0, -1 };

        for (int i = 0; i < 4; i++)
        {
            GameObject infoPanel = Instantiate(debugPanelPrefab, transform.position, Quaternion.identity);
            infoPanel.transform.Translate(dx[i], -0.8f, dz[i]);
            infoPanel.GetComponent<InfoPanelController>().SetText(i.ToString());
        }
    }

    bool IsObstruct(BMObj bo)
    {
        switch (bo)
        {
            case BMObj.HardBlock:
            case BMObj.SoftBlock:
                return true;
            default:
                return false;
        }
    }

    void PutPanel(Addr p, int n, bool b = false)
    {
        Vector3 pos = new Vector3(p.x - 15, -0.45f, 6 - p.z);
        GameObject infoPanel = Instantiate(debugPanelPrefab, pos, Quaternion.identity);
        infoPanel.GetComponent<InfoPanelController>().SetText(n.ToString());

        if (b)
        {
            infoPanel.GetComponent<Renderer>().material.color = Color.red;
        }
    }

}

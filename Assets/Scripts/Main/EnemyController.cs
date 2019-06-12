using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject GameController;
    GameController controller;

    public GameObject debugPanelPrefab;

    private void Awake()
    {
        controller = GameController.GetComponent<GameController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                float rx = Mathf.Round(hit.point.x);
                float rz = Mathf.Round(hit.point.z);

                //Vector3 pos = new Vector3(rx, -0.45f, rz);
                //GameObject infoPanel = Instantiate(debugPanelPrefab, pos, Quaternion.identity);
                //infoPanel.GetComponent<DebugPanelController>().SetText(0.ToString());

                target = new Pos((int)rx + 15, 6 - (int)rz);

                Debug.Log("target = (" + target.x + ", " + target.z + ")");

                SearchWithDFS();
            }

        }

    }

    void dummuy()
    {
        int[] dx = { -1, 0, 1, 0 };
        int[] dz = { 0, 1, 0, -1 };

        for (int i = 0; i < 4; i++)
        {
            GameObject infoPanel = Instantiate(debugPanelPrefab, transform.position, Quaternion.identity);
            infoPanel.transform.Translate(dx[i], -0.8f, dz[i]);
            infoPanel.GetComponent<DebugPanelController>().SetText(i.ToString());
        }
    }

    struct Pos
    {
        public int x;
        public int z;

        public Pos(int px, int pz)
        {
            x = px;
            z = pz;
        }
    }

    List<List<bool>> visited;
    Stack<Pos> route;
    List<List<BMObj>> map;

    int width = 31;
    int height = 13;

    int DFSCount = 0;

    Pos target = new Pos(1, 1);

    bool find = false;

    void SearchWithDFS()
    {
        Debug.Log("search!");

        InitDFS();
        DFS(new Pos(15, 6));

        int a = 0;
        while (route.Count > 0) {
            PutPanel(route.Pop(), a, true);
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

        route = new Stack<Pos>();
    }

    void DFS(Pos p)
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

        route.Push(p);

        if (target.x == p.x && target.z == p.z)
        {
            find = true;
            return;
        }

        for (int i = 0;  i < 4; i++)
        {
            if (p.z + dz[i] >= 0 && p.z + dz[i] < height && p.x + dx[i] >= 0 && p.x + dx[i] < width)
            {
                if (!visited[p.z + dz[i]][p.x + dx[i]] && !IsObstruct(map[p.z + dz[i]][p.x + dx[i]]))
                {
                    DFS(new Pos(p.x + dx[i], p.z + dz[i]));

                    if (!find) route.Pop();
                }
            }
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

    void PutPanel(Pos p, int n, bool b = false)
    {
        Vector3 pos = new Vector3(p.x - 15, -0.45f, 6 - p.z);
        GameObject infoPanel = Instantiate(debugPanelPrefab, pos, Quaternion.identity);
        infoPanel.GetComponent<DebugPanelController>().SetText(n.ToString());

        if (b)
        {
            infoPanel.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

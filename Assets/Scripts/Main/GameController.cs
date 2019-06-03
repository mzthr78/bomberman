using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BMObj
{
    None = -1,
    Empty = 0,
    Player = 1,
    HardBlock = 2,
    SoftBlock = 3,
    Bomb = 4,
    Enemy = 5,
    Item = 6,
    Door = 7,
}

public class GameController : MonoBehaviour
{
    public Text MousePosition;
    public GameObject stage;
    public GameObject player;

    int stageNum = 1;
    int maxStage = 50;

    List<List<BMObj>> map = new List<List<BMObj>>();

    private void Awake()
    {
        float posX = ((stage.transform.localScale.x * 10 - 1) / 2) * -1 + 1;
        float posZ = ((stage.transform.localScale.z * 10 - 1) / 2) - 1;

        player.transform.position = new Vector3(posX, 0, posZ);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitSeed();
        InitBObjects();

        Debug.Log(name + " map count=" + map.Count);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            MousePosition.text = "(" + hit.point.x + ", " + hit.point.z + ")";
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("ClearScene");
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public BMObj GetObj(Vector3 pos)
    {
        return map[6 - (int)pos.z][15 + (int)pos.x];
    }

    public void SetObj(Vector3 pos, BMObj bmObj)
    {
        map[6 - (int)pos.z][15 + (int)pos.x] = bmObj;
    }

    void Shuffle(List<BMObj> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(0, list.Count);
            BMObj tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }

    Queue<BMObj> seed;

    void InitSeed()
    {
        // outer floor = 31 * 13 = 403
        // inner floor = 29 * 11 = 319
        // hardblock = 14 * 5 = 70
        // walkable = 319 - 70 = 249

        int SoftBlockCount = 50 + (stageNum - 1);

        List<BMObj> tmp = Enumerable.Repeat(BMObj.None, 246 - (SoftBlockCount)).ToList();
        tmp.AddRange(Enumerable.Repeat(BMObj.SoftBlock, SoftBlockCount).ToList());

        Shuffle(tmp);

        seed = new Queue<BMObj>();
        for (int i = 0; i < tmp.Count; i++)
        {
            seed.Enqueue(tmp[i]);
        }
    }

    //int maxRow = 13;
    //int maxCol = 31;

    void InitBObjects()
    {
        int maxRow = stage.GetComponent<StageController>().GetHeight();
        int maxCol = stage.GetComponent<StageController>().GetWidth();


        Debug.Log(maxRow + ", " + maxCol);

        for (int i = 0; i < maxRow; i++)
        {
            List<BMObj> tmpLine = new List<BMObj>();

            for (int j = 0; j < maxCol; j++)
            {
                BMObj tmpCol = BMObj.Empty;

                // top, bottom, right, left wall
                if (i == 0 || i == (maxRow - 1) || j == 0 || j == (maxCol - 1))
                {
                    tmpCol = BMObj.HardBlock;
                }
                // HardBlock
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    tmpCol = BMObj.HardBlock;
                }
                // Player
                else if (i == 1 && j == 1)
                {
                    //tmpCol = BMObj.Player;
                    tmpCol = BMObj.Empty;
                }
                // Player right side, bottom side
                else if ((i == 1 && j == 2) || (i == 2 && j == 1))
                {
                    tmpCol = BMObj.Empty;
                }
                else
                {
                    if (seed.Count > 0)
                    {
                        tmpCol = seed.Dequeue();
                    }
                }
                tmpLine.Add(tmpCol);
            }
            map.Add(tmpLine);
        }
    }

    public List<List<BMObj>> GetMap()
    {
        return this.map;
    }
}

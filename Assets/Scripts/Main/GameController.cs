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

public enum PowerUpItem
{
    None = 0,        // なし
    Fire = 1,　      // 火力
    Bomb = 2,　      // 爆弾
    Remote = 3,      // リモコン
    Speed = 4,       // ブーツ
    PassBomb = 5,    // 爆弾通過
    PassWall = 6,    // 壁通過
    Barrier = 7,     // 火炎バリア
    Immortality = 8, // パーフェクトマン
}

public struct Addr
{
    public int x;
    public int z;

    public Addr(int px, int pz)
    {
        x = px;
        z = pz;
    }
}

public class GameController : MonoBehaviour
{
    public GameObject stage;
    public GameObject mainCamera;
    public GameObject player;
    PlayerController pcon;

    public Text MousePosition;
    public Text DebugInfoText;

    public AudioClip StageClearSE;

    static int stageNum = 0;
    static int[] PlayerStatusNumArr = new int[9] { 0, 1, 1, 0, 0, 0, 0, 0, 0 };
    static ViewPoint viewPoint;

    int maxStage = 50;

    string ItemString = "012342213562235162523532358123762385731683653865378";　// Items of Stage[0 .. 50]

    List<List<BMObj>> map = new List<List<BMObj>>();

    public int GetItemPerStage()
    {
        return (int)(ItemString[stageNum] - '0');
    }

    public static void AllInit()
    {
        stageNum = 0;
        PlayerInit();
        viewPoint = ViewPoint.Right;
    }

    public static void PlayerInit()
    {
        PlayerStatusNumArr = new int[9] { 0, 1, 1, 0, 0, 0, 0, 0, 0 };
    }

    public static void SetStageNum(int num)
    {
        stageNum = num;
    }

    public static ViewPoint GetViewPoint()
    {
        return viewPoint;
    }

    public static int NextStageNum()
    {
        return ++stageNum;
    }

    public static int GetStageNum()
    {
        return stageNum;
    }

    public static int[] GetPlayerStatusNumArr()
    {
        return PlayerStatusNumArr;
    }

    public static int GetPlayerStatusNum(PowerUpItem itemNum)
    {
        return PlayerStatusNumArr[(int)itemNum];
    }

    public static void SetPlayerStatusNum(PowerUpItem itemNum, int num)
    {
        PlayerStatusNumArr[(int)itemNum] = num;
    }

    public static void IncreasePlayerStatusNum(PowerUpItem itemNum)
    {
        PlayerStatusNumArr[(int)itemNum]++;
    }

    AudioSource aud;

    private void Awake()
    {
        if (stageNum == 0) stageNum = 1;

        float posX = ((stage.transform.localScale.x * 10 - 1) / 2) * -1 + 1;
        float posZ = ((stage.transform.localScale.z * 10 - 1) / 2) - 1;

        pcon = player.GetComponent<PlayerController>();
        player.transform.position = new Vector3(posX, 0, posZ);

        aud = GetComponent<AudioSource>();
    }

    void Start()
    {
        InitSeed();
        InitBObjects();

        aud.Play();
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

        DebugInfo();
    }

    public void DebugInfo()
    {
        string tmp = "";

        tmp += "ViewPoint = " + viewPoint + "\n";

        DebugInfoText.text = tmp;
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

        // Empty
        List<BMObj> tmp = Enumerable.Repeat(BMObj.None, 246 - (SoftBlockCount)).ToList();

        // SoftBlock
        tmp.AddRange(Enumerable.Repeat(BMObj.SoftBlock, SoftBlockCount).ToList());

        // Door
        tmp.Add(BMObj.Door);

        // Item
        tmp.Add(BMObj.Item);

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
                else if ((i % 2 == 0) && (j % 2 == 0))
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

    void SaveGlovbalSettings()
    {
        int[] playerStatus = player.GetComponent<PlayerController>().GetPlayerStatus();

        for (int i = 0; i < playerStatus.Length; i++)
        {
            PlayerStatusNumArr[i] = playerStatus[i];
        }

        viewPoint = mainCamera.GetComponent<CameraController>().GetViewPoint();
    }

    public void StageClear()
    {
        SaveGlovbalSettings();
        StartCoroutine(StageClearProc());
    }

    IEnumerator StageClearProc()
    {
        aud.Stop();
        aud.PlayOneShot(StageClearSE);

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene("LoadScene");
    }
}

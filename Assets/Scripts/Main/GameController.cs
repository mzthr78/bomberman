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
    PassFire = 7,     // 火炎バリア
    Immortality = 8, // パーフェクトマン
}

public enum Enemy
{
    Ballom = 0,
    Onil = 1,
    Daru = 2,
    Minbo = 3,
    Chondria = 4,
    Obapi = 5,
    Pass = 6,
    Pontan = 7,
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
    bool isDebug = true;

    bool freeze = false;

    public GameObject stage;
    public GameObject mainCamera;
    public GameObject player;
    PlayerController pcon;

    public Text MousePosition;

    // BGM/SSE
    public AudioClip StageClearSE;
    public AudioClip LosingLifeSE;
	public AudioClip ExplosionSE;

    static int stageNum;
    static int[] PlayerStatusNumArr = { 0, 1, 1, 0, 0, 0, 0, 0, 0 };
    public int playerRemainInit = 2;
    static int playerRemain = 0;
    static ViewPoint viewPoint;

    public int SoftBlockNum = 0;
    public int maxStageNum = 1;

    string ItemString = "012342213562235162523532358123762385731683653865378"; // Item(s) of Stage[0 .. 50]

    int[,] EnemiesOfStage = {
      {3, 0, 0, 0, 0, 0, 0, 0 }, // Stage 01 ballom:6, onil:0, ...
      {0, 3, 0, 0, 0, 0, 0, 0 }, // Stage 02 ballom:3, onil:3, ...
      {6, 0, 0, 0, 0, 0, 0, 0 }, // Stage 01 ballom:6, onil:0, ...
      {3, 3, 0, 0, 0, 0, 0, 0 }, // Stage 02 ballom:3, onil:3, ...
      {2, 2, 2, 0, 0, 0, 0, 0 }, // Stage 03 ballom:2, onil:2, daru:2, ...
      {1, 1, 2, 2, 0, 0, 0, 0 }, // Stage 04
      {0, 3, 3, 0, 0, 0, 0, 0 }, // Stage 05
    };

    // Enemies
    public GameObject BallomPrefab;
    public GameObject OnilPrefab;
    public GameObject DaruPrefab;
    public GameObject MinboPrefab;
    public GameObject ChondriaPrefab;
    public GameObject ObapiPrefab;
    public GameObject PassPrefab;
    public GameObject PontanPrefab;

    // Point / Score
    public GameObject PointTextPrefab;
    public Text ScoreText;
    static int score;
    public Text PlayerRemainText;

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
        playerRemain = 2;
    }

    public static void SetStageNum(int num)
    {
        stageNum = num;
    }

    public static void InitScore()
    {
        score = 0;
    }

    public static int GetScore()
    {
        return score;
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

    public int GetStageWidth()
    {
        return stage.GetComponent<StageController>().GetWidth();
    }

    public int GetStageHeight()
    {
        return stage.GetComponent<StageController>().GetHeight();
    }

    public Addr Pos2Addr(Vector3 pos)
    {
        return new Addr((int)Mathf.Round(pos.x) + 15, 6 - (int)Mathf.Round(pos.z));
    }

    public Vector3 Addr2Pos(Addr addr)
    {
        return new Vector3(addr.x - 15, 0, 6 - addr.z);
    }

    public void Freeze(bool b = true)
    {
        this.freeze = b;
    }

    public void UnFreeze()
    {
        Freeze(false);
    }

    public bool IsFreeze()
    {
        return this.freeze;
    }

    private void Awake()
    {
        if (stageNum == 0)
        {
            stageNum = 1;
            playerRemain = playerRemainInit;
        }

        float posX = ((stage.transform.localScale.x * 10 - 1) / 2) * -1 + 1;
        float posZ = ((stage.transform.localScale.z * 10 - 1) / 2) - 1;

        pcon = player.GetComponent<PlayerController>();
        player.transform.position = new Vector3(posX, 0, posZ);

        aud = GetComponent<AudioSource>();

        this.freeze = false;
	}

	void Start()
    {
        bool isAutoPlacement = true;

        if (isAutoPlacement)
        {
            InitMapObj();
            InitBObjects();
            InitEnemies();
        }
        else
        {
            LoadMapText();
            InitBObjects();
        }

        aud.Play();

        /*
        for (int i = 0; i < EnemiesOfStage.GetLength(1); i++)
        {
            Debug.Log(i + "(" + (Enemy)i + ")="  + EnemiesOfStage[GetStageNum() - 1, i]);
        }
        */

        /*
        for (int i = 0; i < EnemiesOfStage.GetLength(0); i++)
        {
            for (int j = 0; j < EnemiesOfStage.GetLength(1); j++)
            {
                Debug.Log("[" + i + "][" + j + "]" + EnemiesOfStage[i, j]);
            }
        }
        */

        /*
		for (int i = 0; i < EnemiesOfStage.Count; i++)
		{
            string s = "";
			for (int j = 0; j < EnemiesOfStage[i].Count; j++)
			{
                s += EnemiesOfStage[i][j];
			}
            Debug.Log(stage + "[" + i + "]" + s);
		}
        */

        PlayerRemainText.text = playerRemain.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            MousePosition.text = "(" + hit.point.x + ", " + hit.point.z + ")";

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(GetObj(new Vector3(Mathf.Round(hit.point.x), hit.point.y, Mathf.Round(hit.point.z))) + "(" + Mathf.Round(hit.point.x) + ", " + Mathf.Round(hit.point.z) + ")");

                GameObject Ballom = Instantiate(BallomPrefab, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.identity);
                Ballom.transform.parent = GameObject.Find("Ballom").transform;
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.freeze = !this.freeze;
        }

        ScoreText.text = score.ToString();
    }

    public BMObj GetObj(Vector3 pos)
    {
        return map[6 - (int)Mathf.Round(pos.z)][15 + (int)Mathf.Round(pos.x)];
    }

    public void SetObj(Vector3 pos, BMObj bmObj)
    {
        map[6 - (int)Mathf.Round(pos.z)][15 + (int)Mathf.Round(pos.x)] = bmObj;
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

    Queue<BMObj> queObj = new Queue<BMObj>();

    void InitMapObj()
    {
        // outer floor = 31 * 13 = 403
        // inner floor = 29 * 11 = 319
        // hardblock = 14 * 5 = 70
        // walkable = 319 - 70 = 249

        int SoftBlockCount = SoftBlockNum + (stageNum - 1);

        // Empty
        List<BMObj> tmp = Enumerable.Repeat(BMObj.Empty, 246 - (SoftBlockCount)).ToList(); //Enumerable(System.Linq)

        // SoftBlock
        tmp.AddRange(Enumerable.Repeat(BMObj.SoftBlock, SoftBlockCount).ToList());  //Enumerable(System.Linq)

        // Door
        tmp.Add(BMObj.Door);

        // Item
        tmp.Add(BMObj.Item);

        Shuffle(tmp);

        queObj = new Queue<BMObj>();
        for (int i = 0; i < tmp.Count; i++)
        {
            queObj.Enqueue(tmp[i]);
        }
    }

    void LoadMapText()
    {
        TextAsset textAsset = new TextAsset();
        textAsset = Resources.Load("testmap", typeof(TextAsset)) as TextAsset;
        string text = textAsset.text;
        string[] lines = text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (i == 0)                       { continue; } // 1行目
                else if (i == (GetStageHeight() - 1))  { continue; } // 最終行
                else if (j == 0)                       { continue; } // 1列目
                else if (j == (GetStageWidth() - 1))   { continue; } // 最終列
                else if (i == 1 && j == 1)             { continue; } // 1マス目（プレイヤー初期位置）
                else if (i == 1 && j == 2)             { continue; } // プレイヤーの右隣
                else if (i == 2 && j == 1)             { continue; } // プレイヤーの真下
                else if ((i % 2) == 0 && (j % 2) == 0) { continue; } // 柱
                else
                {
                    BMObj obj = BMObj.Empty;

                    Addr addr = new Addr(j, i);

                    switch (lines[i][j])
                    {
                        case '*':
                            obj = BMObj.SoftBlock;
                            break;
						case 'A':
                            Debug.Log("Ballom");
                            GameObject Ballom = Instantiate(BallomPrefab, Addr2Pos(addr), Quaternion.identity);
                            Ballom.transform.parent = GameObject.Find("Ballom").transform;
							break;
                        case 'B':
                            Debug.Log("Onil");
                            GameObject Onil = Instantiate(OnilPrefab, Addr2Pos(addr), Quaternion.identity);
                            Onil.transform.parent = GameObject.Find("Onil").transform;
                            break;
                        case 'C':
                            Debug.Log("Daru");
                            break;
                        case 'D':
                            Debug.Log("Minbo");
                            break;
                        case 'E':
                            Debug.Log("Chondria");
                            break;
                        case 'F':
                            Debug.Log("Obapi");
                            break;
                        case 'G':
                            Debug.Log("Pass");
                            break;
                        case 'H':
                            Debug.Log("Pontan");
                            break;
                        default:
                            obj = BMObj.Empty;
                            break;
                    }
                    queObj.Enqueue(obj);
                }
            }
        }
        //Debug.Log("queobj.count=" + queObj.Count);
    }

    //int maxRow = 13;
    //int maxCol = 31;

    private void InitEnemies()
    {
		List<int> emptyCell = new List<int>();

        //　Emptyセルの抽出
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (i < 5 && j < 5) continue;

                if (GetObj(Addr2Pos(new Addr(j, i))) == BMObj.Empty)
				{
                    emptyCell.Add(-1);
				}
            }
        }

        // 敵情報の読み込み
        int enemyCount = 0;
        for (int i = 0; i < EnemiesOfStage.GetLength(1); i++)
        {
            for (int j = 0; j < EnemiesOfStage[GetStageNum() - 1, i]; j++)
            {
                emptyCell[enemyCount] = i;
                enemyCount++;
            }
        }

        // データシャッフル
        for (int i = 0; i < emptyCell.Count; i++)
        {
            int n = Random.Range(0, emptyCell.Count);
            int tmp = emptyCell[i];
            emptyCell[i] = emptyCell[n];
            emptyCell[n] = tmp;
        }

        // 敵配置
        int count = 0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (i < 5 && j < 5) continue;

                Addr addr = new Addr(j, i);

                if (GetObj(Addr2Pos(addr)) == BMObj.Empty)
                {
                    if (emptyCell[count] > -1)
                    {
                        GameObject Enemy;
                        switch (emptyCell[count])
                        {
                            case 0:
                                Enemy = Instantiate(BallomPrefab, Addr2Pos(addr), Quaternion.identity);
                                Enemy.transform.parent = GameObject.Find("Ballom").transform;
                                break;
                            case 1:
                                Enemy = Instantiate(OnilPrefab, Addr2Pos(addr), Quaternion.identity);
                                Enemy.transform.parent = GameObject.Find("Onil").transform;
                                break;
                            /*
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            case 5:
                                break;
                            case 6:
                                break;
                            case 7:
                                break;
                            */
                            default:
                                Enemy = Instantiate(BallomPrefab, Addr2Pos(addr), Quaternion.identity);
                                Enemy.transform.parent = GameObject.Find("Ballom").transform;
                                break;
                        }
                    }
                    count++;
                }
            }
        }
    }

    void InitBObjects()
    {
        int maxRow = stage.GetComponent<StageController>().GetHeight();
        int maxCol = stage.GetComponent<StageController>().GetWidth();

        int count = 0;

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
                    count++;
                    if (queObj.Count > 0)
                    {
                        tmpCol = queObj.Dequeue();
                    }
                }
                tmpLine.Add(tmpCol);

            }
            map.Add(tmpLine);
        }
    }

    // for Test
    void PutEnemies()
    {
        int height = stage.GetComponent<StageController>().GetHeight();
        int width = stage.GetComponent<StageController>().GetWidth();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {

            }
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

    public void Death()
	{
		StageMiss();
	}

    public void StageMiss()
    {
        Freeze();
        playerRemain--;
        StartCoroutine(StageMissProc());
    }

    public void StageClear()
    {
        Freeze();
        SaveGlovbalSettings();
        StartCoroutine(StageClearProc());
    }

    IEnumerator StageMissProc()
    {
        aud.Stop();
        aud.PlayOneShot(LosingLifeSE);

        yield return new WaitForSeconds(3.5f);

        stageNum--;
        if (playerRemain >= 0)
        {
            SceneManager.LoadScene("LoadScene");
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    IEnumerator StageClearProc()
    {
        aud.Stop();
        aud.PlayOneShot(StageClearSE);

        yield return new WaitForSeconds(3.5f);

        if (stageNum + 1 <= maxStageNum)
        {
            SceneManager.LoadScene("LoadScene");
        }
        else
        {
            SceneManager.LoadScene("GameClearScene");
        }
    }

    public void GetPoint(Vector3 pos, int point)
    {
        GameObject PointText = Instantiate(PointTextPrefab);
        PointText.transform.parent = GameObject.Find("Canvas").transform;
        PointText.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        PointText.GetComponent<PointTextController>().InitPointText(pos, point);
        score += point;
    }

    public void PlayExplosionSE()
	{
		GetComponent<AudioSource>().PlayOneShot(ExplosionSE);
	}
}

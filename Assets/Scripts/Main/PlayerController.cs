using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Direction
{
    None = -1,
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3,
}

public class PlayerController : MonoBehaviour
{
    GameController controller;
    public GameObject bomberman;

    public Text FirePowerText;
    public Text BombMaxText;
    public Text BombRemainText;

    float speed = 0.04f;

    Direction dir = Direction.None;

    int[] PlayerStatus = new int[9];

    int BombRemain = 0;

    public GameObject BombPrefab;

    void LoadPlayerStatus()
    {
        for (int i = 0; i < PlayerStatus.Length; i++)
        {
            PlayerStatus[i] = GameController.GetPlayerStatusNum((PowerUpItem)i);
        }
    }

    public int GetPlayerStatus(PowerUpItem num)
    {
        return PlayerStatus[(int)num];
    }

    public int[] GetPlayerStatus()
    {
        return PlayerStatus;
    }

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        LoadPlayerStatus();

        BombRemain = PlayerStatus[(int)PowerUpItem.Bomb];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
            if (BombRemain > 0 && (controller.GetObj(pos) == BMObj.Empty || controller.GetObj(pos) == BMObj.None))
            {
                GameObject Bomb = Instantiate(BombPrefab, pos, Quaternion.identity);

                controller.SetObj(pos, BMObj.Bomb);

                this.BombRemain--;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Direction.Right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Move(Direction.Up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(Direction.Down);
        }

        FirePowerText.text = "FirePower = " + PlayerStatus[(int)PowerUpItem.Fire].ToString();
        BombMaxText.text = "BombMax = " + PlayerStatus[(int)PowerUpItem.Bomb].ToString();

        BombRemain = PlayerStatus[(int)PowerUpItem.Bomb] - GameObject.FindGameObjectsWithTag("Bomb").Length;
        BombRemainText.text = "BombRemain = " + BombRemain.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            DebugPlayerStatus();
        }
    }

    public void DebugPlayerStatus()
    {
        for (int i = 0; i < PlayerStatus.Length; i++)
        {
            Debug.Log(i + " " + (PowerUpItem)i + "=" + PlayerStatus[i]);
        }
    }

    public void IncreasePlayerStatus(PowerUpItem itemNum)
    {
        Debug.Log(name + "(IncreasePlayerStatus) itemNum=" + itemNum);
        Debug.Log(name + "(IncreasePlayerStatus) PlayerStatus[(int)itemNum]=" + PlayerStatus[(int)itemNum] + "(before)");

        PlayerStatus[(int)itemNum]++;

        Debug.Log(name + "(IncreasePlayerStatus) PlayerStatus[(int)itemNum]=" + PlayerStatus[(int)itemNum] + "(after)");

        for (int i = 0; i < PlayerStatus.Length; i++)
        {
            Debug.Log(i + ":" + (PowerUpItem)i + "=" + PlayerStatus[i]);
        }
    }

    private void Move(Direction d) 
    {
        Vector3 pos = new Vector3(0, 0, 0);

        switch (d)
        {
            case Direction.Right:
                bomberman.transform.rotation = Quaternion.Euler(0, 90, 0);
                pos = new Vector3(1, 0, 0);
                break;
            case Direction.Down:
                bomberman.transform.rotation = Quaternion.Euler(0, 180, 0);
                pos = new Vector3(0, 0, -1);
                break;
            case Direction.Left:
                bomberman.transform.rotation = Quaternion.Euler(0, 270, 0);
                pos = new Vector3(-1, 0, 0);
                break;
            case Direction.Up:
                bomberman.transform.rotation = Quaternion.Euler(0, 0, 0);
                pos = new Vector3(0, 0, 1);
                break;
            default:
                break;
        }

        RaycastHit hit;
        if (Physics.Linecast(transform.position, transform.position + pos * 0.45f, out hit))
        {
            switch (hit.transform.tag)
            {
                case "HardBlock":
                case "SoftBlock":
                case "Bomb":
                    pos = new Vector3(0, 0, 0);
                    break;
            }
        }

        transform.Translate(pos.x * speed, pos.y * speed, pos.z * speed);
    }
}



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

    int FirePower = 1;
    int BombMax = 1;
    int BombRemain = 1;

    float speed = 0.04f;

    Direction dir = Direction.None;

    public GameObject BombPrefab;

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
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

        BombRemain = BombMax - GameObject.FindGameObjectsWithTag("Bomb").Length;

        FirePowerText.text = "FirePower = " + FirePower.ToString();
        BombMaxText.text = "BombMax = " + BombMax.ToString();
        BombRemainText.text = "BombRemain = " + BombRemain.ToString();

    }

    public void IncreaseFirePower()
    {
        this.FirePower++;
    }

    public void IncreaseBombMax()
    {
        this.BombMax++;
        this.BombRemain++;
    }

    public int GetFirePower()
    {
        return this.FirePower;
    }

    public int GetBombMax()
    {
        return this.BombMax;
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



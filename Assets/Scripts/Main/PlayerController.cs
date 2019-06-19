using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    GameController controller;
    public GameObject bomberman;
    public GameObject mainCamera;

    public Text FirePowerText;
    public Text BombMaxText;
    public Text BombRemainText;

    Animator animator;

    bool freeze = false;

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
        LoadPlayerStatus();

        BombRemain = PlayerStatus[(int)PowerUpItem.Bomb];

        animator = bomberman.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.freeze) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
            if (BombRemain > 0 && (controller.GetObj(pos) == BMObj.Empty || controller.GetObj(pos) == BMObj.None))
            {
                GameObject Bomb = Instantiate(BombPrefab, pos, Quaternion.identity);

                controller.SetObj(pos, BMObj.Bomb);

                this.BombRemain--;
            } else
            {
                //Debug.Log("obj = " + controller.GetObj(pos));
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
        else
        {
            animator.SetBool("IsWalk", false);
        }

        FirePowerText.text = "FirePower = " + PlayerStatus[(int)PowerUpItem.Fire].ToString();
        BombMaxText.text = "BombMax = " + PlayerStatus[(int)PowerUpItem.Bomb].ToString();

        BombRemain = PlayerStatus[(int)PowerUpItem.Bomb] - GameObject.FindGameObjectsWithTag("Bomb").Length;
        BombRemainText.text = "BombRemain = " + BombRemain.ToString();

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
        ViewPoint vp = mainCamera.GetComponent<CameraController>().GetViewPoint();

        switch (vp)
        {
            case ViewPoint.TPP:
            case ViewPoint.FPP:
                switch (d)
                {
                    case Direction.Right:
                        d = Direction.Left;
                        break;
                    case Direction.Left:
                        d = Direction.Right;
                        break;
                    case Direction.Up:
                        d = Direction.Down;
                        break;
                    case Direction.Down:
                        d = Direction.Up;
                        break;
                }
                break;
            default:
                break;
        }

        Vector3 pos = new Vector3(0, 0, 0);

        animator.SetBool("IsWalk", true);

        switch (d)
        {
            case Direction.Right:
                gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                //pos = new Vector3(1, 0, 0);
                break;
            case Direction.Down:
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                //pos = new Vector3(0, 0, -1);
                break;
            case Direction.Left:
                gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                //pos = new Vector3(-1, 0, 0);
                break;
            case Direction.Up:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                //pos = new Vector3(0, 0, 1);
                break;
            default:
                break;
        }

        //Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.BoxCast(transform.position, Vector3.one * 0.2f, transform.forward, out hit, Quaternion.identity, 0.25f))
        {
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            //Debug.Log("ray hit = " + hit.transform.tag);
            switch (hit.transform.tag)
            {
                case "HardBlock":
                case "SoftBlock":
                    return;
                default:
                    break;
            }
        }

        /*
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.65f)) {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            Debug.Log("ray hit = " + hit.transform.tag);
        }
        */

        /*
        RaycastHit hit;
        if (Physics.Linecast(transform.position, transform.position + pos * 0.45f, out hit))
        {
            Debug.Log("linecasthit " + transform.transform.tag);
            switch (hit.transform.tag)
            {
                case "HardBlock":
                case "SoftBlock":
                case "Bomb":
                    pos = new Vector3(0, 0, 0);
                    break;
            }
        }
        */

        animator.SetBool("IsWalk", true);
        transform.position += transform.forward * speed;
        //transform.Translate(pos.x * speed, pos.y * speed, pos.z * speed);
    }

}



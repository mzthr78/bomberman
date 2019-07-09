using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum Direction
{
    None = -1,
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3,
}
*/

public class OnilController : MonoBehaviour
{
    GameController controller;
    RouteScript routeScript;

    bool freeze = false;

    private readonly int[] dx = { 1, 0, -1, 0 };
    private readonly int[] dz = { 0, -1, 0, 1 };
    private readonly float speed = 0.04f;
    private Direction currDir = Direction.None;

    // Start is called before the first frame update
    void Start()
    {
        ChangeDirection();
        StartCoroutine(Whim());

        controller = GameObject.Find("GameController").GetComponent<GameController>();
        routeScript = GetComponent<RouteScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsFreeze()) return;

        /*
        RaycastHit hit;
        Vector3 dirPos = new Vector3(dx[(int)currDir], 0, dz[(int)currDir]);
        if (Physics.Raycast(transform.position, dirPos, out hit, 0.4f))
        {
            switch (hit.transform.tag)
            {
                case "Enemy":
                case "Fire":
                    break;
                case "Player":
                    break;
                default:
                    ChangeDirection();
                    break;
            }
        }
        */

        bool findPlayer = false;

        RaycastHit hit;
        bool findRight = false;

        float rx = 0;
        float ry = transform.position.y;
        float rz = 0;

        switch (currDir)
        {
            case Direction.Right:
            case Direction.Left:
                rx = Mathf.Round(transform.position.x);
                rz = transform.position.z;
                break;
            case Direction.Up:
            case Direction.Down:
                rx = transform.position.x;
                rz = Mathf.Round(transform.position.z);
                break;
            default:
                break;
        }

        Vector3 rp = new Vector3(rx, ry, rz);

        if (Vector3.Distance(transform.position, rp) < 0.1f)
        {
            if (Physics.Raycast(transform.position, transform.right, out hit, 31.0f) && hit.transform.tag == "Player")
            {
                findPlayer = true;
                findRight = true;
            }
            else if (Physics.Raycast(transform.position, transform.right * -1, out hit, 31.0f) && hit.transform.tag == "Player")
            {
                findPlayer = true;
            }
        }

        if (findPlayer)
        {
            if (findRight)
            {
                switch (currDir)
                {
                    case Direction.Right:
                        currDir = Direction.Down;
                        break;
                    case Direction.Down:
                        currDir = Direction.Left;
                        break;
                    case Direction.Left:
                        currDir = Direction.Up;
                        break;
                    case Direction.Up:
                        currDir = Direction.Right;
                        break;
                }
            }
            else
            {
                switch (currDir)
                {
                    case Direction.Right:
                        currDir = Direction.Up;
                        break;
                    case Direction.Down:
                        currDir = Direction.Left;
                        break;
                    case Direction.Left:
                        currDir = Direction.Down;
                        break;
                    case Direction.Up:
                        currDir = Direction.Left;
                        break;
                }
            }
            Toward(currDir);
        }
        else
        {
            Vector3 dirPos = new Vector3(dx[(int)currDir], 0, dz[(int)currDir]);
            if (Physics.Raycast(transform.position, dirPos, out hit, 0.4f))
            {
                switch (hit.transform.tag)
                {
                    case "Enemy":
                    case "Fire":
                        break;
                    case "Player":
                        break;
                    default:
                        ChangeDirection();
                        break;
                }
            }
        }

        transform.position += transform.forward * speed;
    }

    void ChangeDirection()
    {
        int counter = 0;
        while (true)
        {
            int dir = Random.Range(0, 4);

            RaycastHit hit;
            Vector3 dirPos = new Vector3(dx[dir], 0, dz[dir]);
            if (Physics.Raycast(transform.position, dirPos, out hit, 30))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance > 1)
                {
                    currDir = (Direction)dir;
                    Toward((Direction)dir);
                    break;
                }
            }
            counter++;

            if (counter > 10)
            {
                StartCoroutine(Sleep(1));
                counter = 0;
            }
        }
    }

    // きまぐれ（とりあえずボツ）
    IEnumerator Whim()
    {
        while (true)
        {
            float sec = Random.Range(0, 6) + 1;
            yield return new WaitForSeconds(sec);

            float rx = Mathf.Round(transform.position.x);
            float ry = Mathf.Round(transform.position.y);
            float rz = Mathf.Round(transform.position.z);
            Vector3 rp = new Vector3(rx, ry, rz);

            if (Vector3.Distance(transform.position, rp) < 0.4)
            {
                ChangeDirection();
            }

            //Debug.Log("whim " + sec + "s dir=" + currDir);
        }
    }

    IEnumerator Sleep(float sec)
    {
        Debug.Log("Zzz...");
        yield return new WaitForSeconds(sec);
    }

    void Toward(Direction dir)
    {
        switch (dir)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            controller.GetPoint(transform.position, 200);

            Destroy(gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallomController : MonoBehaviour
{
    GameController controller;

    private readonly int[] dx = { 1, 0, -1, 0 };
    private readonly int[] dz = { 0, -1, 0, 1 };
    private readonly float speed = 0.02f;
    private Direction currDir = Direction.None;

    // Start is called before the first frame update
    void Start()
    {
        ChangeDirection();
        //StartCoroutine(Whim());

        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsFreeze()) return;

        RaycastHit hit;
        // ここ範囲チェックしないとあかん
        Vector3 dirPos = new Vector3(dx[(int)currDir], 0, dz[(int)currDir]);
        if (Physics.Raycast(transform.position, dirPos, out hit, 0.3f))
        {
            switch (hit.transform.tag)
            {
                case "Player":
                    break;
                case "Enemy":
                case "Fire":
                    break;
                default:
                    currDir = Direction.None;
                    ChangeDirection();
                    break;
            }
        }

        /*
        if (!Physics.Raycast(transform.position, transform.right, out hit, 1))
        {
            Debug.Log("right!");
        }

        if (!Physics.Raycast(transform.position, transform.right * -1, out hit, 1))
        {
            Debug.Log("left!");
        }
        */

        transform.position += transform.forward * speed;

    }

    void ChangeDirection()
    {
        int counter = 0;
        while (counter < 10)
        {
            Debug.Log("count=" + counter);

            int dir = Random.Range(0, 4);

            RaycastHit hit;
            Vector3 dirPos = new Vector3(dx[dir], 0, dz[dir]);
            if (Physics.Raycast(transform.position, dirPos, out hit, 30))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance > 1.2f)
                {
                    currDir = (Direction)dir;
                    Toward((Direction)dir);
                    break;
                }
            }
            counter++;
        }
    }

    // きまぐれ（とりあえずボツ）
    IEnumerator Whim()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            float rx = Mathf.Round(transform.position.x);
            float ry = Mathf.Round(transform.position.y);
            float rz = Mathf.Round(transform.position.z);

            Vector3 rp = new Vector3(rx, ry, rz);

            if (Vector3.Distance(transform.position, rp) < 0.05)
            {
                Debug.Log("pos=" + transform.position);
                //ChangeDirection();
            }
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
        Debug.Log("trigger enter?");
        if (other.tag == "Fire")
        {
            controller.GetPoint(transform.position, 100);
            Destroy(gameObject);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotsuController : MonoBehaviour
{
    //public GameObject GameController;
    GameObject GameController;
    GameController controller;
    RouteScript routeScript;

    //public Transform player;
    Transform player;

    public GameObject TargetPrefab;
    public GameObject RoutePanelPrefab;
    
    private GameObject TargetObj1;
    private GameObject TargetObj2;
    private int targetNum = 0;

    int stageWidth = 0;
    int stageHeight = 0;

    private void Awake()
    {
        //Find Object (for Prefab)
        GameController = GameObject.Find("GameController");
        player = GameObject.Find("Player").transform;
        
        controller = GameController.GetComponent<GameController>();

        TargetObj1 = Instantiate(TargetPrefab, transform.position, Quaternion.identity);
        TargetObj1.transform.parent = GameObject.Find("Target").transform;
        //TargetObj1.GetComponent<Renderer>().material.color = Color.blue;

        TargetObj2 = Instantiate(TargetPrefab, transform.position, Quaternion.identity);
        TargetObj2.transform.parent = GameObject.Find("Target").transform;
        //TargetObj2.GetComponent<Renderer>().material.color = Color.red;

        routeScript = gameObject.GetComponent<RouteScript>();

        //Debug.Log("routeScript" + routeScript.ToString());
    }

    Vector3 startPos;
    Vector3 markerPos;

    float posY = 0;

    bool freeze = false;
    float speed = 0.033f;

    Queue<Addr> queRoute;

    public void Freeze(bool b = true)
    {
        this.freeze = b;
    }

    public void UnFreeze()
    {
        Freeze(false);
    }

    int fuga = 0;

    void Hoge()
    {
        Hoge(fuga);
        fuga++;
    }

    void Hoge(int n)
    {
        Debug.Log("hoge " + n);
    }

    private void Start()
    {
        posY = transform.position.y;
        markerPos = transform.position;

        queRoute = new Queue<Addr>();

        stageWidth = controller.GetStageWidth();
        stageHeight = controller.GetStageHeight();

        freeze = false;

        //Wander();
        // 初期の目的地[0]を設定
        List<Addr> route = new List<Addr>();
        route = routeScript.GetDeadEndOne(controller.Pos2Addr(transform.position));

        for (int i = 0; i < route.Count; i++)
        {
            queRoute.Enqueue(route[i]);
        }
        TargetObj1.transform.position = controller.Addr2Pos(route[route.Count - 1]);

        // 初期の目的地[1]を設定
        route = new List<Addr>();
        route = routeScript.GetDeadEndOne(controller.Pos2Addr(transform.position));
        TargetObj2.transform.position = controller.Addr2Pos(route[route.Count - 1]);

        int count = 0;
        while (TargetObj1.transform.position == TargetObj2.transform.position && count < 1000)
        {
            route = new List<Addr>();
            route = routeScript.GetDeadEndOne(controller.Pos2Addr(transform.position));
            TargetObj2.transform.position = controller.Addr2Pos(route[route.Count - 1]);
            count++;
        }

        //Debug.Log("target1 " + TargetObj1.transform.position);
        //Debug.Log("target2 " + TargetObj2.transform.position);
    }

    private void ChangeTarget(GameObject obj)
    {
        Addr start = controller.Pos2Addr(transform.position);
        List<Addr> route = routeScript.GetDeadEndOne(start);
        obj.transform.position = controller.Addr2Pos(route[route.Count - 1]);
    }

    float span = 2;
    float delta = 0;

    private void Update()
    {
        if (freeze) return;

        delta += Time.deltaTime;

        if (delta > 2)
        {
            delta = 0;

            if (Random.Range(0, 10) < 1)
            {
                ChangeTarget(TargetObj1);
            }

            if (Random.Range(0, 10) < 1)
            {
                ChangeTarget(TargetObj2);
            }
        }

        RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
		{
			if (hit.transform.tag == "Bomb")
			{
                Debug.Log("bomb?");
                markerPos = transform.position;
                ChangeTarget(TargetObj1);
                ChangeTarget(TargetObj2);
                FlipTarget();
            }
        }

		float distance = Vector3.Distance(transform.position, markerPos);

        if (distance < 0.05)
        {
            if (queRoute.Count > 0)
            {
                Addr tmp = queRoute.Dequeue();

                markerPos = new Vector3(tmp.x - 15, posY, 6 - tmp.z);
            }
            else
            {
                StartCoroutine(Robe());

                //Addr tmp = queRoute.Dequeue();
                //markerPos = new Vector3(tmp.x - 15, posY, 6 - tmp.z);
                FlipTarget();
            }
        }
        else
        {
            Toward(markerPos);
        }
    }

    private void FlipTarget()
    {
        if (targetNum == 0) targetNum = 1;
        else targetNum = 0;

        Addr start = controller.Pos2Addr(transform.position);
        Addr goal;

        if (targetNum == 0)
        {
            goal = controller.Pos2Addr(TargetObj1.transform.position);
        }
        else
        {
            goal = controller.Pos2Addr(TargetObj2.transform.position);
        }

        List<Addr> route = routeScript.GetShortestRoute(start, goal);

        queRoute.Clear();
        for (int i = 0; i < route.Count; i++)
        {
            queRoute.Enqueue(route[i]);
        }
    }

    bool IsEmpty(BMObj obj)
    {
        switch (obj)
        {
            case BMObj.HardBlock:
            case BMObj.SoftBlock:
            case BMObj.Bomb:
                return false;
            default:
                return true;
        }
    }

    // キョロキョロ
    IEnumerator Robe()
    {
        ChangeDirection(Direction.Down);

        float waitsec = Random.Range(0, 40) * 0.1f + 0.2f;

        yield return new WaitForSeconds(waitsec);
    }

    Direction PreDir = Direction.None;

    void Toward(Vector3 pos)
    {
        Direction d = Direction.None;

        Vector3 sub = transform.position - pos;

        if (Mathf.Abs(sub.x) > 0.1f)
        {
            if (sub.x > 0) d = Direction.Left;
            else d = Direction.Right;
        }
        else if (Mathf.Abs(sub.z) > 0.1f)
        {
            if (sub.z > 0) d = Direction.Down;
            else d = Direction.Up;
        }

        Move(d);
    }

    void ChangeDirection(Direction d)
    {
        switch (d)
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

    void Move(Direction d)
    {
        ChangeDirection(d);

        transform.position += transform.forward * speed;
        //transform.Translate(transform.forward * 0.01f); // これだとダメ
    }

    bool IsObstruct(BMObj bo)
    {
        switch (bo)
        {
            case BMObj.HardBlock:
            case BMObj.SoftBlock:
            case BMObj.Bomb:
                return true;
            default:
                return false;
        }
    }

    bool IsObstruct(string s)
    {
        switch (s)
        {
            case "HardBlock":
            case "SoftBlock":
            case "Bomb":
                return true;
            default:
                return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            controller.GetPoint(transform.position, 100);

            Destroy(gameObject);
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Fire")
        {
            Destroy(gameObject);
        }
    }
    */
}

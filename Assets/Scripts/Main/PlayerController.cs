using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameController controller;

    int FirePower = 1;
    int BombMax = 1;
    int BombCount = 0;

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
            if (BombCount < BombMax && (controller.GetObj(pos) == BMObj.Empty || controller.GetObj(pos) == BMObj.None))
            {
                GameObject Bomb = Instantiate(BombPrefab, pos, Quaternion.identity);

                controller.SetObj(pos, BMObj.Bomb);

                this.BombCount++;
            }
            else
            {
                Debug.Log("Bomb set incorrect !!!");
                Debug.Log("BombCount=" + BombCount);
                Debug.Log("BombMax=" + BombMax);
                Debug.Log("obj=" + controller.GetObj(pos));
            }
        }
    }

    public void IncreaseFirePower()
    {
        this.FirePower++;
    }

    public void IncreaseBombMax()
    {
        this.BombMax++;
    }

    public int GetFirePower()
    {
        return this.FirePower;
    }

    public void DecreaseBombCount()
    {
        if (this.BombCount > 0) { this.BombCount--; }
    }
}

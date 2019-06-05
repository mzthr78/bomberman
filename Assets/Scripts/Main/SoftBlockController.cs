using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBlockController : MonoBehaviour
{
    GameController controller;

    public GameObject DoorPrefab;
    public GameObject ItemFirePrefab;
    public GameObject ItemBombPrefab;

    bool isItem = false;
    bool isDoor = false;

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void SetIsDoor(bool b = true)
    {
        this.isDoor = b;
    }

    public void SetIsItem(bool b = true)
    {
        this.isItem = b;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            //StartCoroutine(Broken());
            Broken();
        }
    }

    void Broken()
    {
        if (isDoor)
        {
            GameObject Door = Instantiate(DoorPrefab, transform.position, Quaternion.Euler(10, 180, 0));
            controller.SetObj(transform.position, BMObj.Door);
        }
        else if (isItem)
        {
            GameObject Item;

            int ItemNum = controller.GetItemPerStage();

            Debug.Log("stageNum=" + GameController.GetStageNum());
            Debug.Log("ItemNum=" + ItemNum + ":" + (PowerUpItem)ItemNum);

            switch (ItemNum)
            {
                case (int)PowerUpItem.Fire:
                    Item = Instantiate(ItemFirePrefab, transform.position, Quaternion.Euler(10, 180, 0));
                    break;
                case (int)PowerUpItem.Bomb:
                    Item = Instantiate(ItemBombPrefab, transform.position, Quaternion.Euler(10, 180, 0));
                    break;
                default:
                    break;
            }
            controller.SetObj(transform.position, BMObj.Item);
        }
        else
        {
            controller.SetObj(transform.position, BMObj.Empty);
        }
        Destroy(gameObject);
        //yield return null;
    }
}

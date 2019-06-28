using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBlockController : MonoBehaviour
{
    GameController controller;

    public GameObject DoorPrefab;

    public GameObject ItemFirePrefab;
    public GameObject ItemBombPrefab;
    public GameObject ItemSpeedPrefab;
    public GameObject ItemRemotePrefab;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Fire")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            Broken();
            Destroy(gameObject);
        }
    }

    int broken = 0;

    private void Broken()
    {
        //Debug.Log(name + "(OnDestroy)" + transform.position);

        //Debug.Log(transform.position + "=" + controller.GetObj(transform.position) + "(before)");

        if (isDoor)
        {
            if (GameObject.FindGameObjectsWithTag("Door").Length == 0)
            {
                GameObject Door = Instantiate(DoorPrefab, transform.position, Quaternion.Euler(10, 180, 0));
            }
            controller.SetObj(transform.position, BMObj.Door);
        }
        else if (isItem)
        {
            int ItemNum = controller.GetItemPerStage();

            if (GameObject.FindGameObjectsWithTag("Item").Length == 0)
            {
                switch (ItemNum)
                {
                    case (int)PowerUpItem.Fire:
                        GameObject fire = Instantiate(ItemFirePrefab, transform.position, Quaternion.Euler(10, 180, 0));
                        break;
                    case (int)PowerUpItem.Bomb:
                        GameObject bomb = Instantiate(ItemBombPrefab, transform.position, Quaternion.Euler(10, 180, 0));
                        break;
                    case (int)PowerUpItem.Speed:
                        GameObject speed = Instantiate(ItemSpeedPrefab, transform.position, Quaternion.Euler(10, 180, 0));
                        break;
                    case (int)PowerUpItem.Remote:
                        GameObject remote = Instantiate(ItemRemotePrefab, transform.position, Quaternion.Euler(10, 180, 0));
                        break;
                    default:
                        break;
                }
            }
            controller.SetObj(transform.position, BMObj.Item);
        }
        else
        {
            controller.SetObj(transform.position, BMObj.Empty);
        }

        //Debug.Log(transform.position + "=" + controller.GetObj(transform.position) + "(after)");
    }
}

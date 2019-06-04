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

    public void SetIsItem()
    {
        this.isItem = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            StartCoroutine(Broken());
        }
    }

    IEnumerator Broken()
    {
        if (isItem)
        {
            GameObject ItemFire = Instantiate(ItemFirePrefab, transform.position, Quaternion.Euler(10, 180, 0));
            controller.SetObj(transform.position, BMObj.Item);
        }
        else
        {
            controller.SetObj(transform.position, BMObj.Empty);
        }
        Destroy(gameObject);
        yield return null;
    }
}

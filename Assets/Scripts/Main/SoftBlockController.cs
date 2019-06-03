using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBlockController : MonoBehaviour
{
    GameController controller;

    public GameObject DoorPrefab;
    public GameObject ItemFirePrefab;

    bool isItem = false;
    bool isDoor = false;

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
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
        controller.SetObj(transform.position, BMObj.Empty);
        Destroy(gameObject);
        yield return null;
    }
}

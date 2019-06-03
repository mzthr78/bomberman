using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBlockController : MonoBehaviour
{
    public GameObject DoorPrefab;
    public GameObject ItemFirePrefab;

    bool isItem = false;
    bool isDoor = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            StartCoroutine(Broken());
        }
    }

    IEnumerator Broken()
    {
        Destroy(gameObject);
        yield return null;
    }
}

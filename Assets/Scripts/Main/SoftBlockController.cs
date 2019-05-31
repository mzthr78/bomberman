using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBlockController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("n?");

        if (other.tag == "Fire")
        {
            Debug.Log("explosion!");
            StartCoroutine(Broken());
        }
    }

    IEnumerator Broken()
    {
        Destroy(gameObject);
        yield return null;
    }
}

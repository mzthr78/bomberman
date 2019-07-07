using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTextController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flee());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Flee() 
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}

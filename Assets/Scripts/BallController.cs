using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject stage;
    Rigidbody rgbody;
    float power = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rgbody = GetComponent<Rigidbody>();
//        transform.position = stage.transform.localScale
    }

    // Update is called once per frame
    void Update()
    {
        rgbody.AddForce(Input.GetAxis("Horizontal") * power, 0, Input.GetAxis("Vertical") * power);
    }
}

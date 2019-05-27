using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody rgbody;
    float power = 5f;
    public GameObject bombPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rgbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rgbody.AddForce(Input.GetAxis("Horizontal") * power, 0, Input.GetAxis("Vertical") * power);
    }
}

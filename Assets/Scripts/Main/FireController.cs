using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject GameController;
    GameController controller;

    List<List<BMObj>> map;

    float span = 2.5f;
    float delta = 0;

    Vector3 power = new Vector3(0, 0, 0);
    Vector3 target;

    float speed = 0.8f;
    private float startTime;
    private float journeyLength;

    public void SetPower(Vector3 power)
    {
        this.power = power;
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, (transform.position + power));

        target = transform.position + power;

        /*
        GameController = GameObject.Find("GameController");
        controller = GameController.GetComponent<GameController>();
        map = controller.GetMap();
        */      
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (delta > span)
        {
            delta = 0;
            Destroy(gameObject);
        }

        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(transform.position, target, fracJourney);
    }

}

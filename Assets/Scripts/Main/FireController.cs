using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject GameController;
    GameController controller;

    List<List<BMObj>> map;

    float span = 1.4f;
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
        controller = GameObject.Find("GameController").GetComponent<GameController>();

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
        if (controller.IsFreeze()) return;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SoftBlock")
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}

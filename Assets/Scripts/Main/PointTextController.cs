using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointTextController : MonoBehaviour
{
    GameController controller;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Flee());
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsFreeze()) return;
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, startPos);
    }

    public void InitPointText(Vector3 pos, int p)
    {
        startPos = pos;
        GetComponent<Text>().text = p.ToString();
    }

    IEnumerator Flee() 
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}

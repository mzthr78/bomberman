using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanelController : MonoBehaviour
{
    //GameObject chidText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string s)
    {
        transform.GetChild(0).GetComponent<TextMesh>().text = s;
    }
}

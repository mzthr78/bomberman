using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelController : MonoBehaviour
{
    public void SetText(string s)
    {
        transform.GetChild(0).GetComponent<TextMesh>().text = s;
    }
}

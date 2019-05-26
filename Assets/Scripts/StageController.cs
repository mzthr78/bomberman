using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject HardBlockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.localScale);

        int width = (int)(transform.localScale.x * 10 + 1);
        int height = (int)(transform.localScale.z * 10 + 1);

        float posX = ((transform.localScale.x * 10 - 1) / 2) * - 1;
        float posZ = ((transform.localScale.z * 10 - 1) / 2);

        // top line
        for (int i = 0; i < width; i++)
        {

            GameObject HardBlock = Instantiate(HardBlockPrefab);
            HardBlock.transform.position = new Vector3(posX + i, 0, posZ);
        }

        for (int i = 1; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

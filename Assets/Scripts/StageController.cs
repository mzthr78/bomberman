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

        float posX;
        float posY = -0.5f;
        float posZ;

        // top line
        posX = ((transform.localScale.x * 10 - 1) / 2) * -1;
        posZ = ((transform.localScale.z * 10 - 1) / 2);
        for (int i = 0; i < width; i++)
        {
            GameObject HardBlock = Instantiate(HardBlockPrefab);
            HardBlock.transform.position = new Vector3(posX + i, posY, posZ);
        }

        // 
        for (int i = 1; i < height; i++)
        {
            if (i % 2 != 0)
            {
                GameObject HardBlock;

                HardBlock = Instantiate(HardBlockPrefab);
                HardBlock.transform.position = new Vector3(posX + 0, posY, posZ - i);

                HardBlock = Instantiate(HardBlockPrefab);
                HardBlock.transform.position = new Vector3(posX + width - 1, posY, posZ - i);
            }
            else
            {
                for (int j = 0; j < width; j++)
                {
                    if (j % 2 == 0)
                    {
                        GameObject HardBlock = Instantiate(HardBlockPrefab);
                        HardBlock.transform.position = new Vector3(posX + j, posY, posZ - i);
                    }
                }
            }
        }

        // bottom line
        posZ = ((transform.localScale.z * 10 - 1) / 2) * -1;
        for (int i = 0; i < width; i++)
        {
            GameObject HardBlock = Instantiate(HardBlockPrefab);
            HardBlock.transform.position = new Vector3(posX + i, posY, posZ);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

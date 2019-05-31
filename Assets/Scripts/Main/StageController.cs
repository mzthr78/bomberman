using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject HardBlockPrefab;
    public GameObject SoftBlockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int width = (int)(transform.localScale.x * 10 + 1);
        int height = (int)(transform.localScale.z * 10 + 1);

        float posX;
        float posY = 0f;
        float posZ;

        // top(0) line
        posX = ((transform.localScale.x * 10 - 1) / 2) * -1;
        posZ = ((transform.localScale.z * 10 - 1) / 2);
        for (int i = 0; i < width; i++)
        {
            GameObject HardBlock = Instantiate(HardBlockPrefab);
            HardBlock.transform.position = new Vector3(posX + i, posY, posZ);
        }

        // 1 .. n - 1 line
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

        // bottom(n) line
        posZ = ((transform.localScale.z * 10 - 1) / 2) * -1;
        for (int i = 0; i < width; i++)
        {
            GameObject HardBlock = Instantiate(HardBlockPrefab);
            HardBlock.transform.position = new Vector3(posX + i, posY, posZ);
        }
    }

}

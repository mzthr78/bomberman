using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject GameController;

    public GameObject HardBlockPrefab;
    public GameObject SoftBlockPrefab;

    GameController controller;

    int width;
    int height;

    private void Awake()
    {
        controller = GameController.GetComponent<GameController>();

        width = (int)(transform.localScale.x * 10 + 1);
        height = (int)(transform.localScale.z * 10 + 1);
    }

    // Start is called before the first frame update
    void Start()
    {
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

        List<List<BMObj>> map = controller.GetMap();

        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                posX = (width - 1) / 2 * -1 + j;
                posZ = (height - 1) / 2 - i;

                Vector3 pos = new Vector3(posX, posY, posZ);

                switch (map[i][j])
                {
                    case BMObj.SoftBlock:
                        GameObject SoftBlock = Instantiate(SoftBlockPrefab, pos, Quaternion.identity);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public int GetWidth()
    {
        return this.width;
    }

    public int GetHeight()
    {
        return this.height;
    }
}

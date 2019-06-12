using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject GameController;

    public Transform HardBlockParent;
    public Transform SoftBlockParent;

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
            Vector3 pos = new Vector3(posX + i, posY, posZ);
            //GameObject HardBlock = Instantiate(HardBlockPrefab, pos, Quaternion.identity, HardBlockParent);
            GameObject HardBlock = Instantiate(HardBlockPrefab, pos, Quaternion.identity);
            HardBlock.transform.parent = HardBlockParent;
        }

        // 1 .. n - 1 line
        for (int i = 1; i < height; i++)
        {
            if (i % 2 != 0)
            {
                GameObject HardBlock;

                Vector3 pos;

                pos = new Vector3(posX + 0, posY, posZ - i);
                HardBlock = Instantiate(HardBlockPrefab, pos, Quaternion.identity);
                HardBlock.transform.parent = HardBlockParent;

                pos = new Vector3(posX + width - 1, posY, posZ - i);
                HardBlock = Instantiate(HardBlockPrefab, pos, Quaternion.identity);
                HardBlock.transform.parent = HardBlockParent;
            }
            else
            {
                for (int j = 0; j < width; j++)
                {
                    if (j % 2 == 0)
                    {
                        Vector3 pos = new Vector3(posX + j, posY, posZ - i);
                        GameObject HardBlock = Instantiate(HardBlockPrefab, pos, Quaternion.identity);
                        HardBlock.transform.parent = HardBlockParent;
                    }
                }
            }
        }

        // bottom(n) line
        posZ = ((transform.localScale.z * 10 - 1) / 2) * -1;
        for (int i = 0; i < width; i++)
        {
            Vector3 pos = new Vector3(posX + i, posY, posZ);
            GameObject HardBlock = Instantiate(HardBlockPrefab, pos, Quaternion.identity);
            HardBlock.transform.parent = HardBlockParent;
        }

        List<List<BMObj>> map = controller.GetMap();

        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                posX = (width - 1) / 2 * -1 + j;
                posZ = (height - 1) / 2 - i;

                Vector3 pos = new Vector3(posX, posY, posZ);

                GameObject SoftBlock;

                switch (map[i][j])
                {
                    case BMObj.SoftBlock:
                        SoftBlock = Instantiate(SoftBlockPrefab, pos, Quaternion.identity);
                        SoftBlock.transform.parent = SoftBlockParent;
                        break;
                    case BMObj.Door:
                        SoftBlock = Instantiate(SoftBlockPrefab, pos, Quaternion.identity);
                        SoftBlock.GetComponent<SoftBlockController>().SetIsDoor();
                        SoftBlock.transform.parent = SoftBlockParent;
                        controller.SetObj(pos, BMObj.SoftBlock);
                        break;
                    case BMObj.Item:
                        SoftBlock = Instantiate(SoftBlockPrefab, pos, Quaternion.identity);
                        SoftBlock.GetComponent<SoftBlockController>().SetIsItem();
                        SoftBlock.transform.parent = SoftBlockParent;
                        controller.SetObj(pos, BMObj.SoftBlock);
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

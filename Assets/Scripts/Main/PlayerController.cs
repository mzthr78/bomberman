using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    GameController controller;

    public Text FirePowerText;
    public Text BombMaxText;
    public Text BombRemainText;

    int FirePower = 1;
    int BombMax = 1;
    int BombRemain = 1;

    public GameObject BombPrefab;

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
            if (BombRemain > 0 && (controller.GetObj(pos) == BMObj.Empty || controller.GetObj(pos) == BMObj.None))
            {
                GameObject Bomb = Instantiate(BombPrefab, pos, Quaternion.identity);

                controller.SetObj(pos, BMObj.Bomb);

                this.BombRemain--;
            }
        }

        BombRemain = BombMax - GameObject.FindGameObjectsWithTag("Bomb").Length;

        FirePowerText.text = "FirePower = " + FirePower.ToString();
        BombMaxText.text = "BombMax = " + BombMax.ToString();
        BombRemainText.text = "BombRemain = " + BombRemain.ToString();

    }

    public void IncreaseFirePower()
    {
        this.FirePower++;
    }

    public void IncreaseBombMax()
    {
        this.BombMax++;
        this.BombRemain++;
    }

    public int GetFirePower()
    {
        return this.FirePower;
    }

    public int GetBombMax()
    {
        return this.BombMax;
    }

}

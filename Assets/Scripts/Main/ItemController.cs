using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    GameController controller;
    PlayerController player;

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        player = GameObject.Find("Sphere").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (tag)
            {
                case "PowerUp_Fire":
                    player.IncreaseFirePower();
                    break;
                case "PowerUp_Bnum":
                    player.IncreaseBombMax();
                    break;
            }
            controller.SetObj(transform.position, BMObj.Item);
            Destroy(gameObject);
        }
    }
}

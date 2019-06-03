using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    PlayerController player;

    private void Awake()
    {
        player = GameObject.Find("Sphere").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(tag + " get!!!");
            switch (tag)
            {
                case "PowerUp_Fire":
                    player.IncreaseFirePower();
                    break;
                case "PowerUp_Bnum":
                    player.IncreaseBombMax();
                    break;
            }

            Destroy(gameObject);
        }
    }
}

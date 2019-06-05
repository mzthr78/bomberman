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
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (tag)
            {
                case "PowerUp_Fire":
                    StartCoroutine(PowerUp(PowerUpItem.Fire));
                    break;
                case "PowerUp_Bnum":
                    StartCoroutine(PowerUp(PowerUpItem.Bomb));
                    break;
            }
        }
    }

    IEnumerator PowerUp(PowerUpItem item)
    {
        player.IncreasePlayerStatus(item);
        controller.SetObj(transform.position, BMObj.Empty);
        Destroy(gameObject);
        yield return null;
    }
}

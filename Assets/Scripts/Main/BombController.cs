﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    GameController controller;
    PlayerController player;

    public GameObject FirePrefab;
    public AudioClip ExplosionSE;

    float delta = 0;
    float span = 3.5f;

    int[] dx = { 1,  0, -1, 0 };
    int[] dz = { 0, -1,  0, 1 };

    bool explosion = false;

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsFreeze()) return;

        delta += Time.deltaTime;

        if (delta > span)
        {
            if (!explosion) StartCoroutine(Explosion());
            delta = 0;
        }
    }

    IEnumerator Explosion()
    {
        //GetComponent<AudioSource>().PlayOneShot(ExplosionSE);
        controller.PlayExplosionSE();

        explosion = true;

        GameObject Fire;

        Fire = Instantiate(FirePrefab);
        Fire.transform.position = transform.position;

        int explode = player.GetPlayerStatus(PowerUpItem.Fire);

        bool[] stop = { false, false, false, false };

        for (int j = 0; j < explode; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                if (stop[i]) continue;

                Vector3 power = new Vector3(dx[i] * (j + 1), 0, dz[i] * (j + 1));
                Vector3 pos = transform.position + power;

                if (pos.x + 15 < 0 || pos.x > 30 || 6 - pos.z < 0 || 6 - pos.z > 12) continue;

                BMObj tmp = controller.GetObj(pos);

                switch (tmp)
                {
                    case BMObj.HardBlock:
                        stop[i] = true;
                        break;
                    default:
                        if (tmp == BMObj.SoftBlock) stop[i] = true;

                        Fire = Instantiate(FirePrefab);
                        //Fire.transform.position = new Vector3(transform.position.x + dx[i], transform.position.y, transform.position.z + dz[i]);

                        Fire.GetComponent<FireController>().SetPower(power);
                        Fire.transform.position = transform.position;
                        break;
                }
            }
        }

        controller.SetObj(transform.position, BMObj.Empty);

        Destroy(gameObject);

        yield return null;
    }

    IEnumerator Explosion(float lag)
    {
        yield return new WaitForSeconds(lag);
        StartCoroutine(Explosion());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            if (!explosion) StartCoroutine(Explosion(0.25f));
        }
    }
}

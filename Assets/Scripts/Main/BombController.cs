using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject FirePrefab;

    float delta = 0;
    float span = 5f;

    int[] dx = { 1,  0, -1, 0 };
    int[] dz = { 0, -1,  0, 1 };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (delta > span)
        {
            StartCoroutine(Explosion());
            delta = 0;
        }
    }

    IEnumerator Explosion()
    {
        GameObject Fire;

        Fire  = Instantiate(FirePrefab);
        Fire.transform.position = transform.position;

        for (int i = 0; i < 4; i++)
        {
            Fire = Instantiate(FirePrefab);
            Fire.transform.position = new Vector3(transform.position.x + dx[i], transform.position.y, transform.position.z + dz[i]);
        }

        Destroy(gameObject);
        yield return null;
    }
}

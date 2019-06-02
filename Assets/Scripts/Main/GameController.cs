using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text MousePosition;
    public GameObject stage;
    public GameObject player;

    List<List<char>> map = new List<List<char>>();

    private void Awake()
    {
        float posX = ((stage.transform.localScale.x * 10 - 1) / 2) * -1 + 1;
        float posZ = ((stage.transform.localScale.z * 10 - 1) / 2) - 1;

        player.transform.position = new Vector3(posX, 0, posZ);
    }
    // Start is called before the first frame update
    void Start()
    {
        List<char> list = new List<char>();
        list.Add('a');
        list.Add('b');
        list.Add('c');
        list.Add('d');
        list.Add('e');

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("list[" + i + "] -> " + list[i]);
        }

        shuffle(list);
        shuffle(list);
        shuffle(list);
    }

    void shuffle(List<char> list)
    {
        Debug.Log("### shuffle ###");
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(0, list.Count);
            char tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("list[" + i + "] -> " + list[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            MousePosition.text = "(" + hit.point.x + ", " + hit.point.z + ")";
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("ClearScene");
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

}

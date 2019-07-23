using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RankingController : MonoBehaviour
{
    public Button Return2TitleButton;
    public Button TestButton;

    public GameObject RankLinePrefab;
    public GameObject Canvas;

    // Start is called before the first frame update
    void Start()
    {
        Return2TitleButton.onClick.AddListener(() => LoadTitleScene());

        StartCoroutine(GetRanking());

        TestButton.onClick.AddListener(() => StartCoroutine(UpdateScore()));

        /*
        for (int i = 0; i < 10; i++)
        {
            GameObject RankLine = Instantiate(RankLinePrefab, RankLinePrefab.transform.position, Quaternion.identity, Canvas.transform);
            RankLine.transform.Translate(new Vector3(0, -50 * (i + 1), 0));
        }
        */
    }

    void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator GetRanking()
    {

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            RankingData[] data;

            string jsonText = www.downloadHandler.text;

            data = JsonHelper.FromJson<RankingData>(jsonText);

            GameObject[] RankLines = GameObject.FindGameObjectsWithTag("Rank");
            for (int i = 0; i < RankLines.Length; i++)
            {
                Destroy(RankLines[i]);
            }

            for (int i = 0; i < data.Length; i++)
            {
                GameObject RankLine = Instantiate(RankLinePrefab, Canvas.transform);
                RankLine.transform.Translate(0, -50 * i, 0);
                //RankLine.transform.Translate(new Vector3(0, -50 * i, 0));

                RankLine.transform.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
                RankLine.transform.Find("Name").GetComponent<Text>().text = data[i].name;
                RankLine.transform.Find("Score").GetComponent<Text>().text = data[i].score.ToString();
            }
        }
    }

    IEnumerator UpdateScore()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/update?name=XXX&score=999");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            StartCoroutine(GetRanking());
        }
    }
}

[System.Serializable]
public class RankingData
{
    public string name;
    public int score;
}

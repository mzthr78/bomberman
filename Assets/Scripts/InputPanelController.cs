using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class InputPanelController : MonoBehaviour
{
    public Button OKButton;
    public Text NameText;

    private void Start()
    {
        OKButton.onClick.AddListener(() => StartCoroutine(UpdateScore()));
    }

    IEnumerator UpdateScore()
    {
        string inputName = NameText.text;
        int score = GameController.GetScore();

        string url = string.Format("http://localhost:5000/update?name={0}&score={1}", inputName, score);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //StartCoroutine(GetRanking());
            SceneManager.LoadScene("RankingScene");
        }
    }
}

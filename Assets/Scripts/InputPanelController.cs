using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class InputPanelController : MonoBehaviour
{
    public Text nameInput;
    public Button OKButton;

    // Start is called before the first frame update
    void Start()
    {
        OKButton.onClick.AddListener(() => OKButtonClick());
    }

    void OKButtonClick()
    {
        string inputName = nameInput.text; //nameInput.GetComponent<Text>().text;
        int score = GameController.GetScore();
        StartCoroutine(UpdateScore(inputName, score));
    }

    IEnumerator UpdateScore(string s, int score)
    {
        string url = string.Format("http://localhost:5000/update?name={0}&score={1}", s, score);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //StartCoroutine(GetRanking());
            Debug.Log("load ranking scene");
            SceneManager.LoadScene("RankingScene");
        }
    }
}

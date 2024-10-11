using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    public TMP_InputField usernameInput;
    public Button startButton;
    public Button exitButton;
    public GameObject latestScoresContent;
    public GameObject rankScoresContent;
    public GameObject scoreTextPrefab;

    private void Start()
    {
        startButton.onClick.AddListener(StartNew);
        exitButton.onClick.AddListener(Exit);
        bestScoreText.text = $"Best Score: {DataManager.Instance.bestScore}";

        PopulateScrollList(latestScoresContent, DataManager.Instance.latestScores);
        PopulateScrollList(rankScoresContent, DataManager.Instance.rankScores);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
            return;
        }
    }

    public void StartNew()
    {
        DataManager.Instance.username = usernameInput.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit(); // original code to quit Unity player
        #endif
    }

    public void PopulateScrollList(GameObject content, List<DataManager.SessionData> items)
    {
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(scoreTextPrefab, content.transform);
            TextMeshProUGUI itemText = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = $"{item.username}: {item.score}";
            }
        }
    }
}

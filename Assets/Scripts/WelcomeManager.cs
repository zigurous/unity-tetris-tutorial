using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeManager : MonoBehaviour
{
    public GameObject Panel;
    public int clickCount = 0;
    private string userId;

    private void PrepareExperimentDirectory()
    {
        string assetsPath = Application.streamingAssetsPath;
        string rootPath = Application.persistentDataPath;
        string path = rootPath + "/ExperimentData/" + userId;
        Debug.Log("Creating directory: " + path);
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        string sourcePath = assetsPath + "/markers-survey.csv";
        string destinationPath = path + "/markers-survey-" + userId + ".csv";
        System.IO.File.Copy(sourcePath, destinationPath, true);
    }

    public void ConfirmStartingExperiment()
    {
        Panel.SetActive(true);

        clickCount++;
        if (clickCount == 2)
        {
            StartExperiment();
        }
    }

    public void StartExperiment()
    {
        Debug.Log("Starting experiment for a User: " + userId);
        PlayerPrefs.SetString("userId", userId);
        // StartDataAquistion(userId);
        // StartTimer();
        PrepareExperimentDirectory();
        // SceneManager.LoadScene("Experiment");
    }


    void Start()
    {
        userId = System.Guid.NewGuid().ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}

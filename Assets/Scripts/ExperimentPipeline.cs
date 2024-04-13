using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentPipeline : MonoBehaviour
{
    public static ExperimentPipeline Instance { get; private set; }

    [SerializeField] private Experiment experiment;
    [SerializeField] private bool isDebugMode;
    private int currentSceneIndex = 0;
    private float currentTime = 0;
    private bool hasStarted = false;
    private List<SceneDuration> sceneDurations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sceneDurations = experiment.sceneDurations;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
    }


    public void StartExperiment()
    {
        currentSceneIndex = 0;
        hasStarted = true;
        ResetTimer();
        SceneManager.LoadScene(sceneDurations[currentSceneIndex].sceneName);
    }

    public void ResetTimer()
    {
        currentTime = 0;
    }

    public void StopTimer()
    {
        hasStarted = false;
    }

    public void NextScene()
    {
        currentSceneIndex++;
        if (currentSceneIndex < sceneDurations.Count)
        {
            ResetTimer();
            SceneManager.LoadScene(sceneDurations[currentSceneIndex].sceneName);
        }
        else
        {
            StopTimer();
            Debug.Log("Experiment Finished");
        }
    }

    public void Update()
    {
        if(hasStarted)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= sceneDurations[currentSceneIndex].durationInSeconds)
            {
                NextScene();
            }
        }
        
    }

    public void OnGUI()
    {
        if (hasStarted && isDebugMode)
        {
            GUILayout.Label("Current Scene: " + sceneDurations[currentSceneIndex].sceneName);
            GUILayout.Label("Time Left: " + (sceneDurations[currentSceneIndex].durationInSeconds - currentTime));
        }
    }
}

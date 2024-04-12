using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Survey : MonoBehaviour
{
    public TMP_Text surveyTitle;
    public List<SurveySection> surveySections = new List<SurveySection>();

    public void GetSurvey()
    {
        var content = surveyTitle.text + "\n";

        foreach (SurveySection section in surveySections)
        {
            content += section.GetSection() + "\n";
        }
        Debug.Log(content);

        string rootPath = Application.persistentDataPath;
        string path = rootPath + "/ExperimentData/"; // TODO: add '+ userId/'

        string destinationPath = path + surveyTitle.text + ".txt";
        System.IO.File.WriteAllText(destinationPath, content);
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: surveyTitle.title += userId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

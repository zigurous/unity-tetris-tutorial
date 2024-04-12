using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SurveySection : MonoBehaviour
{

    // TextMeshPro
    public GameObject question1;
    public TMP_Dropdown answer1;

    public GameObject question2;
    public TMP_InputField answer2;

    public GameObject question3;
    public TMP_Dropdown answer3;

    public string GetSection()
    {
        var q1Text = question1.GetComponent<TextMeshProUGUI>().text;
        var a1Text = answer1.options[answer1.value].text;

        var q2Text = question2.GetComponent<TextMeshProUGUI>().text;
        var a2Text = answer2.text;

        var q3Text = question3.GetComponent<TextMeshProUGUI>().text;
        var a3Text = answer3.options[answer3.value].text;

        return q1Text + "\n" + a1Text + "\n" + q2Text + "\n" + a2Text + "\n" + q3Text + "\n" + a3Text + "\n";
    }
    // Start is called before the first frame update
    void Start()
    {
        // question1 = questionObject1.GetComponent<TextMeshProUGUI>().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

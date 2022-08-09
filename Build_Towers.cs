using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build_Towers : MonoBehaviour
{
    public Button buildButton;
    private bool isEnabled;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = buildButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick(){
        if(isEnabled){
            isEnabled = false;
            panel.SetActive(false);
        }

        else if(!isEnabled){
            isEnabled = true;
            panel.SetActive(true);
        }
    }
}

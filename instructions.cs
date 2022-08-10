using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class instructions : MonoBehaviour
{
    public GameObject page1, page2;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        //btn.onClick.AddListener(ToggleInstructions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleInstructions(){
        if(page1.activeSelf){
            print("active");
            page1.SetActive(false);
            page2.SetActive(true);
        }
        else if(!page1.activeSelf){
            print("else");
            page2.SetActive(false);
            page1.SetActive(true);
        }
    }
}

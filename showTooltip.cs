using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showTooltip : MonoBehaviour
{

    Camera cam;
    public GameObject canvas;
    Vector3 min, max;
    RectTransform rect;
    float offset = 10f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        rect = GetComponent<RectTransform>();
        min = new Vector3(0, 0, 0);
        max = new Vector3(cam.pixelWidth, cam.pixelHeight, 0);
        print(min);
        print(max);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf){
            Vector3 position = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 worldPosition = new Vector3(position.x, position.y, 0f);
            //transform.position = new Vector3(Mathf.Clamp(position.x, min.x, max.x - rect.rect.width), Mathf.Clamp(position.y, min.y, max.y - rect.rect.height), 0f);
            //transform.position = new Vector3(worldPosition.x-canvas.GetComponent<RectTransform>().rect.width/2 + rect.rect.width, worldPosition.y-canvas.GetComponent<RectTransform>().rect.height/2 - rect.rect.height/2, 0f);
            transform.position = new Vector3(worldPosition.x + rect.rect.width*3/4, worldPosition.y - rect.rect.height, 0f);

        }
    }
}

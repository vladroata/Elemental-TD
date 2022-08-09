using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw_circle : MonoBehaviour
{
    myInterface script;
    float range = 0;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void OnEnable()
    {
        script = transform.parent.GetComponent<myInterface>();
        range = script.getRange();
        //range = transform.parent.GetComponent<Basic_Tower>().getRange();
        DrawCircle(100, range);
        lineRenderer.transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Draws the circle highlighting the range of the tower
    void DrawCircle(int steps, float radius){ //steps = how many straight lines the circle is made of
        lineRenderer.positionCount = steps;

        for(int currentStep = 0; currentStep < steps; currentStep++){
            float circumferenceProgress = (float)currentStep/(steps-1);
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);
            float x = xScaled * radius+script.getXpos();
            float y = yScaled * radius+script.getYpos();

            Vector2 currentPosition = new Vector2(x, y);
            lineRenderer.SetPosition(currentStep, currentPosition);
        }
        //lineRenderer.transform.position = new Vector3(getXpos(), getYpos(), 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 1f;
    GameObject target;
    Vector3 targetDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){
            transform.LookAt(target.transform);
            if(Vector3.Distance(target.transform.position, transform.position) > 0.5f){
                transform.Translate((transform.forward * speed * Time.deltaTime));
            }
        }
        else{
            Destroy(gameObject);
        }
        
        
        //targetDir = target.transform.position - transform.position;
        //angleBetween = Vector3.Angle(transform.forward, targetDir);
        
        //if(target == null){
        //    Destroy(gameObject);
        //}
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Enemy"){
            Destroy(gameObject);
        }
    }

    public void setTarget(GameObject x){
        target = x;
    }
}
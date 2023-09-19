using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Controller : MonoBehaviour
{
    private float start_pos;
    private float length; 
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Virtual Camera");
        start_pos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1-parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);
        
        transform.position = new Vector3(start_pos + distance, transform.position.y,transform.position.z); //moving game object in the x range
        
        
        if (temp > start_pos + length)
        {
            start_pos += length;
        }else if (temp < start_pos - length)
        {
            start_pos -= length;
        }
        
    }

}

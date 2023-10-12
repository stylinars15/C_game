using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dectection_Zone : MonoBehaviour
{

    public string tagtarget = "Player";

    public List<Collider2D> detectedObjs = new List<Collider2D>();


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == tagtarget)
        {
            detectedObjs.Add(collider);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == tagtarget)
        {
            detectedObjs.Remove(collider);
        }
    }
}

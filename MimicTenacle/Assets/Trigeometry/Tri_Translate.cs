using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tri_Translate : MonoBehaviour
{

    [SerializeField, Range(0, 2)]
    float Rate;

    // Update is called once per frame
    void Update()
    {
        float baseAngle = 1.5f * Mathf.PI; // 270 degrees
        float halfAngleRange = 0.25f * Mathf.PI; // 45 degrees
        float c = Mathf.Cos(Rate * Time.time);
        float angle = halfAngleRange * c + baseAngle;
        transform.position =
          new Vector3
          (
            Rate * Mathf.Cos(angle),
            Rate * Mathf.Sin(angle),
            0.0f
          );

        //transform.position = new Vector3(
        //        Mathf.Cos(Rate * Time.time),
        //        Mathf.Sin(Rate * Time.time),
        //        0
        //    ); 
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMoveCurve : MonoBehaviour
{
    Vector3[] smoothLine;
    int targetIndex = 0;
    float speed = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        //test
        Vector3[] linePoints = DLGCurve.randomLine(15, new Vector3(-5, 0, 0), new Vector3(5, 0, 0));
        DLGCurve.DrawALine(linePoints, Color.black);

        smoothLine = DLGCurve.DLG(linePoints, 3);
        DLGCurve.DrawALine(smoothLine, Color.red);

        transform.position = smoothLine[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (targetIndex < smoothLine.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, smoothLine[targetIndex], speed * Time.deltaTime);
            if (transform.position == smoothLine[targetIndex])
            {
                targetIndex++;
            }
        }
        
    }
}

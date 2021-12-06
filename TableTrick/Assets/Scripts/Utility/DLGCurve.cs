using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// using DLG to generate a curve
/// </summary>
public static class DLGCurve
{
    const float offset = 0.3f;

    //support spawn area
    static float halfWidth, halfLength;
    static float minSpawnX, maxSpawnX, minSpawnY, maxSpawnY;

    static public void Initailize()
    {
        GameObject tempCup = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("OpenedCup"));
        BoxCollider2D bc2d = tempCup.GetComponent<BoxCollider2D>();
        halfWidth = bc2d.size.x / 2 + offset;
        halfLength = bc2d.size.y / 2 + offset;
        UnityEngine.Object.Destroy(tempCup);

        //calculate spawn area
        minSpawnX = ScreenUtils.ScreenLeft + halfWidth;
        maxSpawnX = ScreenUtils.ScreenRight - halfWidth;
        minSpawnY = ScreenUtils.ScreenBottom + halfLength;
        maxSpawnY = ScreenUtils.ScreenTop - halfLength;
    }

    /// <summary>
    /// using AveragingMask to calculate interpolation
    /// </summary>
    /// <param name="point1">point 1</param>
    /// <param name="point2">point 2</param>
    /// <param name="point3">point 3</param>
    /// <param name="point4">point 4</param>
    /// <param name="point5">point 5</param>
    /// <returns></returns>
    static float DLGAveragingMask(float point1, float point2, float point3, float point4, float point5)
    {
        return (-2 * point1 + 5 * point2 + 10 * point3 + 5 * point4 + (-2) * point5) / 16.0f;
    }

    /// <summary>
    /// return a smoth line according to given line
    /// </summary>
    /// <param name="originalLine">original Line</param>
    /// <param name="interpolationNumber">Number of the interpolation</param>
    /// <returns>smooth line</returns>
    public static Vector3[] DLG(Vector3[] originalLine, int interpolationNumber)
    {

        if (originalLine.Length < 3)
        {//the number of points in the original line must more than 3 
            return null;
        }
        
        //calculate new size after interpolate 
        int newLenth = originalLine.Length + (originalLine.Length - 1) * ((int)Mathf.Pow(2, interpolationNumber) - 1);
        int interval = (int)Mathf.Pow(2, interpolationNumber);// support calculating points

        Vector3[] curveLine = new Vector3[newLenth];
        Vector3[] tempLine = new Vector3[newLenth];

        for(int i =0;i< originalLine.Length; i++)
        {
            curveLine[i * interval] = originalLine[i];
            tempLine[i * interval] = originalLine[i];
        }

        while (interval >= 2)
        {
            //calculate the midpoint
            for (int i = interval / 2; i < curveLine.Length - 1; i = i + interval)
            {
                tempLine[i].x = (tempLine[i - interval / 2].x + tempLine[i + interval / 2].x) / 2.0f;
                tempLine[i].y = (tempLine[i - interval / 2].y + tempLine[i + interval / 2].y) / 2.0f;
                tempLine[i].z = (tempLine[i - interval / 2].z + tempLine[i + interval / 2].z) / 2.0f;
            }
            
            //(interpolate) calculate the curve point (alter the midpoint)
            curveLine[interval / 2].x = DLGAveragingMask(tempLine[0].x, tempLine[0].x,
                                                        tempLine[interval / 2].x,
                                                        tempLine[interval].x, tempLine[interval + interval / 2].x);
            curveLine[interval / 2].y = DLGAveragingMask(tempLine[0].y, tempLine[0].y,
                                                        tempLine[interval / 2].y,
                                                        tempLine[interval].y, tempLine[interval + interval / 2].y);
            curveLine[interval / 2].z = DLGAveragingMask(tempLine[0].z, tempLine[0].z,
                                                        tempLine[interval / 2].z,
                                                        tempLine[interval].z, tempLine[interval + interval / 2].z);
            
            for (int i = interval + interval / 2; i < curveLine.Length - 1 - interval; i = i + interval)
            {
                curveLine[i].x = DLGAveragingMask(tempLine[i - interval].x, tempLine[i - interval / 2].x, tempLine[i].x,
                                                    tempLine[i + interval / 2].x, tempLine[i + interval].x);
                curveLine[i].y = DLGAveragingMask(tempLine[i - interval].y, tempLine[i - interval / 2].y, tempLine[i].y,
                                                    tempLine[i + interval / 2].y, tempLine[i + interval].y);
                curveLine[i].z = DLGAveragingMask(tempLine[i - interval].z, tempLine[i - interval / 2].z, tempLine[i].z,
                                                    tempLine[i + interval / 2].z, tempLine[i + interval].z);
            }
            int lastPointIdx = curveLine.Length - 1;
            curveLine[lastPointIdx - interval / 2].x = DLGAveragingMask(tempLine[lastPointIdx - interval - interval / 2].x, tempLine[lastPointIdx - interval].x,
                                                                    tempLine[lastPointIdx - interval / 2].x,
                                                                    tempLine[lastPointIdx].x, tempLine[lastPointIdx].x);
            curveLine[lastPointIdx - interval / 2].y = DLGAveragingMask(tempLine[lastPointIdx - interval - interval / 2].y, tempLine[lastPointIdx - interval].y,
                                                                    tempLine[lastPointIdx - interval / 2].y,
                                                                    tempLine[lastPointIdx].y, tempLine[lastPointIdx].y);
            curveLine[lastPointIdx - interval / 2].z = DLGAveragingMask(tempLine[lastPointIdx - interval - interval / 2].z, tempLine[lastPointIdx - interval].z,
                                                                    tempLine[lastPointIdx - interval / 2].z,
                                                                    tempLine[lastPointIdx].z, tempLine[lastPointIdx].z);
            
            //completed one interpolation, prepare for next one
            Array.Copy(curveLine, tempLine, curveLine.Length);
            interval = interval / 2;
        }

        return curveLine;
    }



    public static Vector3[] randomLine(int pointNumber, Vector3 startPoint, Vector3 backPoint)
    {
        Vector3[] linePoints = new Vector3[pointNumber];
        int sum = 0;
        float distanceSqr;

        //spawn points
        linePoints[0] = startPoint;

        for (int i = 1; i < linePoints.Length - 1; i++)
        {
            linePoints[i] = new Vector3();
            
            //linePoints[i].z = 0;
            /*
            float magnitude = UnityEngine.Random.Range(3.0f, 5.0f);
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            linePoints[i].x = linePoints[i - 1].x + Mathf.Cos(angle) * magnitude;
            linePoints[i].y = linePoints[i - 1].y + Mathf.Sin(angle) * magnitude;
            */
            
            sum = 0;
            do
            {
                sum++;
                linePoints[i].x = UnityEngine.Random.Range(minSpawnX, maxSpawnX);
                linePoints[i].y = UnityEngine.Random.Range(minSpawnY, maxSpawnY);

                distanceSqr = (linePoints[i] - linePoints[i - 1]).sqrMagnitude;
                if (sum > 100)
                {
                    break;
                }
            } while (distanceSqr < 1.5f * 1.5f || 6 * 6 < distanceSqr);
        }
        linePoints[linePoints.Length - 1] = backPoint;

        return linePoints;
    }

    public static void DrawALine(Vector3[] linePoints, Color color)
    {
        //draw a rectangle to enclose the target
        float lineWidth = 0.1f;
        //Vector3[] rectanglePoints = new Vector3[4];
        //Collider2D coll2D = target.GetComponent<Collider2D>();


        // create a group of lines for rectangle
        GameObject Line = new GameObject();
        LineRenderer rectangleLine = Line.AddComponent<LineRenderer>();
        rectangleLine.positionCount = linePoints.Length;
        rectangleLine.sortingOrder = 1;
        //rectangleLine.material = Resources.Load<Material>("LineMaterial");
        rectangleLine.material = new Material(Shader.Find("Hidden/Internal-Colored"));
        //smooth
        rectangleLine.alignment = LineAlignment.TransformZ;


        //rectangleLine.startColor = new Color(102 / 255f, 255 / 255f, 0 / 255f);
        //rectangleLine.endColor = new Color(102 / 255f, 255 / 255f, 0 / 255f);
        rectangleLine.startColor = color;
        rectangleLine.endColor = color;

        //rectangleLine.startWidth = lineWidth;
        //rectangleLine.endWidth = lineWidth;
        rectangleLine.widthMultiplier = lineWidth;

        // set rectangle upper right
        rectangleLine.useWorldSpace = false;

        //lineWidth = lineWidth / 2;
        //rectangleLine.loop = true;

        //draw lines
        rectangleLine.SetPositions(linePoints);
    }
}


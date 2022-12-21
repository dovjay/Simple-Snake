using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour
{
    public int xMargin = 8;
    public int yMargin = 11;
    public float marginStep = .3f;
    public bool deadlyBoundary = false;
    public Vector2 scale;
    public Vector2 boundary;

    void Awake()
    {
        deadlyBoundary = (PlayerPrefs.GetInt(OptionKey.DEADLY_BOUNDARY, 0) == 1) ? true : false;
        float xScale = xMargin * 2 * marginStep + marginStep;
        float yScale = yMargin * 2 * marginStep + marginStep;
        scale = new Vector2(xScale, yScale);
        transform.localScale = new Vector3(xScale, yScale, 1f);
        boundary = new Vector2(xMargin * marginStep, yMargin * marginStep);
    }
}

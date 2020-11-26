using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUlilities;

public class Stack : MonoBehaviour
{
    GameObject box;
    float speed;
    float boxScale;
    int amountOfBoxes;
    List<GameObject> stack = new List<GameObject>();

    HSBColor color;
    float colorGap;
    int colorPosition;

    public Action onDestroy;

    #region Public Properties
    public HSBColor GetColor => color;
    public float GetSpeed => speed; 
    public void Init(GameObject _box, HSBColor _color, float _speed, float _colorGap, int _amountOfBoxes)
    {
        box = _box;
        color = _color;
        speed = _speed;
        colorGap = _colorGap;
        amountOfBoxes = _amountOfBoxes;
        colorPosition = new IntRange(0, amountOfBoxes - 1).Random;
        MakeStack();
    }
    public void SetColor(HSBColor _color)
    {
        color = _color;
        HSBColor baseColor = new HSBColor(color.h - colorGap * colorPosition, color.s, color.b);

        for (int i = 0; i < amountOfBoxes; i++)
        {
            baseColor = ClampColor(baseColor);

            stack[i].GetComponent<Box>().SetColor(baseColor);

            baseColor = new HSBColor(baseColor.h + colorGap, baseColor.s, baseColor.b);
        }
    }
    public Vector3 GetLowestPosition()
    {
        Vector3 pos = new Vector3(0, ScreenHelper.screenHeight / 2, 0);
        foreach (GameObject box in stack)
        {
            if (box.transform.position.y < pos.y)
                pos = box.transform.position;
        }
        Vector3 size = box.GetComponent<Renderer>().bounds.size;
        pos = new Vector3(pos.x, pos.y - size.y/2, pos.z);
        return pos;
    }
    public Vector3 GetHighestPosition()
    {
        Vector3 pos = new Vector3(0, -ScreenHelper.screenHeight/2, 0);
        foreach (GameObject box in stack)
        {
            if (box.transform.position.y > pos.y)
                pos = box.transform.position;
        }
        Vector3 size = box.GetComponent<Renderer>().bounds.size;
        pos = new Vector3(pos.x, pos.y + size.y/2, pos.z);
        return pos;
    }
    public void DestroyStack()
    {
        foreach (GameObject go in stack)
            Destroy(go);

        onDestroy();
        Destroy(gameObject);
    }

    public void PaintToBlackAndWhite()
    {
        foreach(GameObject go in stack)
        {
            if (go.GetComponent<Box>().GetColor == color)
            {
                go.GetComponent<Box>().SetColor(HSBColor.FromRGB(Color.black));
                continue;
            }
            go.GetComponent<Box>().SetColor(HSBColor.FromRGB(Color.white));
        }
    }

    #endregion

    #region Private Properties
    private void Update()
    {
        foreach (GameObject go in stack)
            if (go.transform.position.y < -(ScreenHelper.screenHeight / 2) - (boxScale / 2))
            {
                Destroy(go);
                stack.Remove(go);
                return;
            }

        if(stack.Count == 0)
        {
            onDestroy();
            Destroy(gameObject);
        }
    }
    private void MakeStack()
    {
        boxScale = ScreenHelper.screenWidth / amountOfBoxes;

        if (color.ToColor() == Color.black)
        {
            for(int i = 0; i < amountOfBoxes; i++)
            {
                if(i == colorPosition)
                {
                    stack.Add(Instantiate(box, new Vector2((ScreenHelper.screenWidth / 2 - (boxScale * i)) - (boxScale / 2), (ScreenHelper.screenHeight / 2) + (boxScale / 2)), Quaternion.identity));
                    stack[i].GetComponent<Box>().Init(boxScale, speed, HSBColor.FromRGB(Color.black), this);
                    continue;
                }

                stack.Add(Instantiate(box, new Vector2((ScreenHelper.screenWidth / 2 - (boxScale * i)) - (boxScale / 2), (ScreenHelper.screenHeight / 2) + (boxScale / 2)), Quaternion.identity));
                stack[i].GetComponent<Box>().Init(boxScale, speed, HSBColor.FromRGB(Color.white), this);
            }
            return;
        }

        HSBColor baseColor = new HSBColor(color.h - colorGap * colorPosition, color.s, color.b);

        for (int i = 0; i < amountOfBoxes; i++)
        {
            baseColor = ClampColor(baseColor);

            stack.Add(Instantiate(box, new Vector2((ScreenHelper.screenWidth / 2 - (boxScale * i)) - (boxScale / 2), (ScreenHelper.screenHeight / 2) + (boxScale / 2)), Quaternion.identity));
            stack[i].GetComponent<Box>().Init(boxScale, speed, baseColor, this);

            baseColor = new HSBColor(baseColor.h + colorGap, baseColor.s, baseColor.b);
        }
    }
    private HSBColor ClampColor(HSBColor color)
    {
        if (color.h > 1.0f)
            color.h -= 1.0f;
        else if (color.h < 0.0)
            color.h = 1.0f + color.h;
        return color;
    }

    #endregion
}

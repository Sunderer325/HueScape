using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUlilities;

public class Box : MonoBehaviour
{
    float speed;
    Stack stack;
    HSBColor color;
    bool colorChanged;
    Renderer renderer;

    public Stack GetParentStack => stack;
    public HSBColor GetColor => color;
    public void SetColor(HSBColor _color)
    {
        color = _color;
        colorChanged = true;
    }

    public void Init(float _boxScale, float _speed, HSBColor _color, Stack _stack)
    {
        speed = _speed;
        color = _color;
        stack = _stack;
        transform.localScale = new Vector3(_boxScale, _boxScale, _boxScale);
        renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_Color", _color.ToColor());
    }
    public void FixedUpdate()
    {
        transform.position = transform.position - new Vector3(0, speed, 0) * Time.fixedDeltaTime;
        if (colorChanged)
        {
            if (renderer.material.GetColor("_Color") == color.ToColor())
                colorChanged = false;
            renderer.material.SetColor("_Color", Color.Lerp(renderer.material.GetColor("_Color"), color.ToColor(), 10 * Time.fixedDeltaTime));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PaintWall"))
        {
            stack.SetColor(other.GetComponent<PaintWall>().GetColor);
        }
    }
}

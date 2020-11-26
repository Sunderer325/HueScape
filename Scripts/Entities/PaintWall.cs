using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUlilities;

public class PaintWall : MonoBehaviour
{
    float speed;
    HSBColor color;

    public HSBColor GetColor => color;

    public void Init(float _speed, HSBColor _color)
    {
        speed = _speed;
        color = _color;
        transform.localScale = new Vector3(ScreenHelper.screenWidth, transform.localScale.x, transform.localScale.z);
        transform.position = new Vector2(0, ScreenHelper.screenHeight / 2 + 0.5f);

        GetComponent<Renderer>().material.SetColor("_Color", color.ToColor());

        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.startColor = color.ToColor();
        color.a = 0;
        trail.endColor = color.ToColor();
        color.a = 1;

    }

    private void Update()
    {
        if (transform.position.y < -ScreenHelper.screenHeight)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position = transform.position - new Vector3(0, speed, 0) * Time.fixedDeltaTime;
    }

}

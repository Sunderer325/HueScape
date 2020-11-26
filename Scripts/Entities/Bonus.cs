using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUlilities;

public class Bonus : MonoBehaviour
{
    float speed;
    Renderer renderer;
    public BonusType type;
    public Material colorBoostM;
    public Material addPointsM;
    public static int BonusPoints = 15;

    public void Init(float _speed, BonusType _type)
    {
        speed = _speed;
        type = _type;
        renderer = GetComponent<Renderer>();
        if (type == BonusType.ColorBoost)
            renderer.material = colorBoostM;
        else if (type == BonusType.AddPoints)
            renderer.material = addPointsM;

        float randomX = new FloatRange(-(ScreenHelper.screenWidth / 2), (ScreenHelper.screenWidth / 2)).Random;
        transform.position = new Vector2(randomX, ScreenHelper.screenHeight / 2 + 0.45f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }
    private void FixedUpdate()
    {
        transform.position = transform.position - new Vector3(0, speed, 0) * Time.fixedDeltaTime;
        if(type == BonusType.ColorBoost)
        {
            renderer.material.SetColor("_Color", HSBColor.GetRandomColor(new FloatRange(0.0f, 1.0f), new FloatRange(1.0f, 1.0f), new FloatRange(1.0f, 1.0f)).ToColor());
        }
    }
}

public enum BonusType
{
    BlackAndWhite,
    ColorBoost,
    AddPoints
}
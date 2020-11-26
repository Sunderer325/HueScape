using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    float speed;
    public void InitWhite(float _speed)
    {
        speed = _speed;
    }
    public void InitBlack(float _speed)
    {
        speed = _speed;
        StartBlackAnim();
    }
    public void InitError()
    {
        StartErrorAnim();
    }

    private void StartBlackAnim()
    {
        GetComponent<Animator>().SetTrigger("Black");
    }
    private void StartErrorAnim()
    {
        GetComponent<Animator>().SetTrigger("Error");
    }

    public void EndAnim()
    {
        Destroy(gameObject);
    }
}

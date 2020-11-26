using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroy[] objects = FindObjectsOfType<DontDestroy>();
        if (objects.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}

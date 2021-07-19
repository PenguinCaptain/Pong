using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackGround : MonoBehaviour
{
    void Awake()
    {
        if (!(Camera.main is null)) transform.localScale = Camera.main.ViewportToWorldPoint(Vector2.one) * 2;
    }
}

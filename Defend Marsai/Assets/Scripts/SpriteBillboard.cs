using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffset : MonoBehaviour {

    public float scrollSpeed;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float offset = (Time.time + scrollSpeed)*0.01f;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset,0));
    }


}

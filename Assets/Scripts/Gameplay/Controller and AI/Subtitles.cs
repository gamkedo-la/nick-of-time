using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// descriptive text for game events, one per player
// example: "picked up a potion"
public class Subtitles : MonoBehaviour
{
    public TextMeshProUGUI SubtitlesTextMesh;
    public float fadePerFrame = 0.005f;

    private float alpha = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!SubtitlesTextMesh) return;
        Caption("Subtitles: ON");
    }

    // Update is called once per frame
    void Update()
    {
        if (!SubtitlesTextMesh) return;
        alpha -= fadePerFrame;
        if (alpha < 0f) alpha = 0f;
        SubtitlesTextMesh.color = Color.Lerp(Color.white, Color.clear, 1f - alpha);
    }

    public void Caption(string Str) {
        Debug.Log("Caption: " + Str);
        SubtitlesTextMesh.text = Str;
        alpha = 1f;
    }
}

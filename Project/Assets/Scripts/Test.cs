using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    void Start()
    {
        sr.sprite = SpriteGenerator.I.ConvertToSprite(SpriteGenerator.I.GenerateOutline(SpriteGenerator.I.GetHuman(50, 50, Color.red), false), 50);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapSprites : MonoBehaviour
{
    private Image image;
    private Image shadow;

    public Sprite[] sprites;
    public Sprite[] shadows;
    public float swapSpeed = 1f;

    private float spriteIndex = 0f;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        shadow = transform.GetChild(3).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteIndex += swapSpeed * Time.deltaTime;
        int index = (int)spriteIndex;

        image.sprite = sprites[((int)spriteIndex) % sprites.Length];
        shadow.sprite = shadows[((int)spriteIndex) % shadows.Length];
    }
}

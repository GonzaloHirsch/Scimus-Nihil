using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2Controller : MonoBehaviour
{
    Dictionary<char, Vector2> keysMap;
    char[] charAllowed = { 'q', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
    char currentChar;

    public Sprite[] HotColdSprites;

    public GameObject BlackScreen;
    SpriteRenderer blackScreenRenderer;
    public float FadeOutSpeed = 5f;

    private void Start()
    {
        currentChar = getRandomChar();
        blackScreenRenderer = BlackScreen.GetComponent<SpriteRenderer>();
        initKeysMap();
    }

    private void Update()
    {
        fadeOut();
        checkKeyDown();

    }

    void checkKeyDown()
    {
        foreach (char c in charAllowed)
        {
            if (Input.GetKeyDown(c.ToString()))
            {
                if (c == currentChar)
                {
                    charFound();
                    currentChar = getRandomChar();
                } else
                {
                    //Vector2.Distance(keysMap[c]
                }
            }
        }
    }

    void charFound()
    {
        Color color = blackScreenRenderer.color;
        color.a = 0;
        blackScreenRenderer.color = color;

    }

    char getRandomChar()
    {
        return charAllowed[Random.Range(0, charAllowed.Length)];
    }

    void fadeOut()
    {
        Color color = blackScreenRenderer.color;
        color.a += FadeOutSpeed * Time.deltaTime;
        blackScreenRenderer.color = color;
    }

    void initKeysMap()
    {
        keysMap = new Dictionary<char, Vector2>();
        keysMap.Add('q', new Vector2(0, 0));
        keysMap.Add('e', new Vector2(2, 0));
        keysMap.Add('r', new Vector2(2, 0));
        keysMap.Add('t', new Vector2(3, 0));
        keysMap.Add('y', new Vector2(4, 0));
        keysMap.Add('u', new Vector2(5, 0));
        keysMap.Add('i', new Vector2(6, 0));
        keysMap.Add('o', new Vector2(7, 0));
        keysMap.Add('p', new Vector2(8, 0));
        keysMap.Add('f', new Vector2(3, 1));
        keysMap.Add('g', new Vector2(4, 1));
        keysMap.Add('h', new Vector2(5, 1));
        keysMap.Add('j', new Vector2(6, 1));
        keysMap.Add('k', new Vector2(7, 1));
        keysMap.Add('l', new Vector2(8, 1));
        keysMap.Add('z', new Vector2(0, 2));
        keysMap.Add('x', new Vector2(1, 2));
        keysMap.Add('c', new Vector2(2, 2));
        keysMap.Add('v', new Vector2(3, 2));
        keysMap.Add('b', new Vector2(4, 2));
        keysMap.Add('n', new Vector2(5, 2));
        keysMap.Add('m', new Vector2(6, 2));
    }
}

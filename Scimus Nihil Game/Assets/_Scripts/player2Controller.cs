using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class player2Controller : MonoBehaviour
{
    Dictionary<char, Vector2> keysMap;
    char[] charAllowed = { 'q', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
    char currentChar;

    public AudioSource proxSound;

    public RawImage[] HotColdSprites;
    public SpriteRenderer ProxText;

    public GameObject BlackScreen;
    SpriteRenderer blackScreenRenderer;
    public float FadeOutSpeed = 5f;

    player1Controller p1c;

    private void Start()
    {
        p1c = GetComponent<player1Controller>();
        currentChar = getRandomChar();
        blackScreenRenderer = BlackScreen.GetComponent<SpriteRenderer>();
        initKeysMap();
    }

    private void Update()
    {
        fadeIn(blackScreenRenderer, FadeOutSpeed);
        checkKeyDown();
        /*fadeOut(HotColdSprites[0], 0.75f);
        fadeOut(HotColdSprites[1], 0.75f);
        fadeOut(HotColdSprites[2], 0.75f);*/
        //print(p1c.energy);
    }

    int count = 0;

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
                    print(count);
                    count = 0;
                    //p1c.IncrementEnergyDecrement();
                }
                else
                {
                    float d = Vector2.Distance(keysMap[c], keysMap[currentChar]);
                    if (d < 2)
                    {
                        ShowSprite(HotColdSprites[0], 1);
                        HotColdSprites[0].DOFade(0.1f, 1);
                        proxSound.pitch = 3;
                        proxSound.volume = 1;
                    }
                    else if (d < 3)
                    {
                        ShowSprite(HotColdSprites[1], 1);
                        HotColdSprites[1].DOFade(0, 1);
                        proxSound.pitch = 2;
                        proxSound.volume = 0.5f;
                    }
                    else
                    {
                        ShowSprite(HotColdSprites[2], 1);
                        HotColdSprites[2].DOFade(0, 1);
                        proxSound.pitch = 1;
                        proxSound.volume = 0.2f;
                    }

                    p1c.LowerEnergy();
                    proxSound.Play();
                    count++;
                }
            }
        }
    }

    void charFound()
    {
        HideSprite(blackScreenRenderer);
    }

    char getRandomChar()
    {
        return charAllowed[Random.Range(0, charAllowed.Length)];
    }

    void fadeOut(RawImage renderer, float speed)
    {
        Color color = renderer.color;
        color.a -= 5 * Time.deltaTime;
        renderer.color = color;
    }

    void fadeIn(SpriteRenderer renderer, float speed)
    {
        Color color = renderer.color;
        color.a += speed * Time.deltaTime;
        renderer.color = color;
    }

    void HideSprite(SpriteRenderer renderer)
    {
        Color color = renderer.color;
        color.a = 0;
        renderer.color = color;

    }

    void ShowSprite(RawImage renderer, float amount)
    {
        Color color = renderer.color;
        color.a = amount;
        renderer.color = color;

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

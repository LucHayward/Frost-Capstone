using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBoss : MonoBehaviour
{
    public Sprite sprite1; // Drag your first sprite here
    public Sprite sprite2; // Drag your second sprite here
    public Sprite sprite3;
    public Sprite sprite4;
    private Boss boss;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
        if (spriteRenderer.sprite == null) // if the sprite on spriteRenderer is null then
            spriteRenderer.sprite = sprite1; // set the sprite to sprite1
    }

    void Update()
    {
        if (boss.lvl == 2)
        {
            changeSprite(sprite2); // call method to change sprite
        }
        else if (boss.lvl == 3)
        {
            changeSprite(sprite3);
        }
        else if (boss.lvl == 4)
        {
            changeSprite(sprite4);
        }

        else if (boss.lvl == 5)
        {
            changeSprite(sprite4);
        }


    }

    void changeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
}

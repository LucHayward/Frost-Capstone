using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemy : MonoBehaviour
{
    public Sprite sprite1; // Drag your first sprite here
    public Sprite sprite2; // Drag your second sprite here
    public Sprite sprite3;
    public Sprite sprite4;
    private Enemy enemy;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
        if (spriteRenderer.sprite == null) // if the sprite on spriteRenderer is null then
            spriteRenderer.sprite = sprite1; // set the sprite to sprite1
    }

    void Update()
    {
        if (enemy.lvl == 2)
        {
            changeSprite(sprite2); // call method to change sprite
        }
        else if (enemy.lvl == 3)
        {
            changeSprite(sprite3);
        }
        else if (enemy.lvl == 4)
        {
            changeSprite(sprite4);
        }
        

    }

    void changeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
}

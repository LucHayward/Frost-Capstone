using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawShield : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    private Player player;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.hasShield == true && player.isFast == true)
        {
            showShield(sprite1);
        }

        else if (player.hasShield == true && player.isFast == false)
        {
            showShield(sprite2);
        }



        else if (player.hasShield == false && player.isFast == true)
        {
            showShield(sprite3);
        }
        else if (player.isFast == false && player.isFast==false)
        {
            showShield(sprite4);
        }
            
        
    }

    public void showShield(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
}

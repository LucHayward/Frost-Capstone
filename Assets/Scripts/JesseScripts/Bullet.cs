using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 shot;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        shot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, shot, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position,shot) <0.2f)
        {
            Destroy(gameObject);
        }
        
    }
}

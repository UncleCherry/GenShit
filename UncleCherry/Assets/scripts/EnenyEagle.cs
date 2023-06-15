using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnenyEagle : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform topPoint,bottomPoint;
    public Animator anim;
    public Collider2D coll;
    private float topPointY,bottomPointY;
    public bool isUp=true;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        
        transform.DetachChildren();
        topPointY = topPoint.position.y;
        bottomPointY = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement(){
        if(isUp){
            rb.velocity = new Vector2(rb.velocity.x,speed); 
            if(rb.position.y > topPointY){                
                isUp = false;
               
            }
        }
        else{
            rb.velocity = new Vector2(rb.velocity.x,-speed); 
            if(rb.position.y < bottomPointY){
                isUp = true;                 
            }
        }
    }
}

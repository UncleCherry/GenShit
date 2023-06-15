using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform leftPoint,rightPoint;
    public Animator anim;
    public Collider2D coll;
    public LayerMask ground;
    public AudioSource deathAudio;

    private float leftPointX,rightPointX;
    public bool faceLeft=true;
    public float speed,jumpForce;
    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        deathAudio = GetComponent<AudioSource>();


        transform.DetachChildren();
        leftPointX = leftPoint.position.x;
        rightPointX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement(){
        if(faceLeft){
            if(coll.IsTouchingLayers(ground)){
                anim.SetBool("jumping",true);
                rb.velocity = new Vector2(-speed,jumpForce);
                if(rb.position.x < leftPointX){
                    transform.localScale = new Vector3(-1,1,1);
                    faceLeft = false;

                }
            }

        }
        else{
            if(coll.IsTouchingLayers(ground)){
                anim.SetBool("jumping",true);
                rb.velocity = new Vector2(speed,jumpForce);
                if(rb.position.x > rightPointX){
                    transform.localScale = new Vector3(1,1,1);
                    faceLeft = true;
                }    
            }
        }
    }

    void SwitchAnim(){
        if(anim.GetBool("jumping")){
            if(rb.velocity.y < 0.1f){
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        else if(anim.GetBool("falling")&&coll.IsTouchingLayers(ground)){
            anim.SetBool("falling",false);
        }
    }

    void Death(){


        Destroy(gameObject);        


    }

    public void JumpOn(){
        deathAudio.Play();
        anim.SetTrigger("death");
    }
}

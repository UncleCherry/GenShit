using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerControler : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Collider2D coll;
    public LayerMask ground;

    public Transform groundCheck;
    public AudioSource jumpAudio;

    public GameObject GarbagePrefab;  //用于掉落垃圾

    private int trashNum=0;
    private int[] trash=new int[3];


    public float speed;
    public float jumpForce=8;
    public int blood = 100;

    public int cherryNum=0;
    public Text cherryNumText;


    public bool jumpPressed=false;
    public int jumpCount=1;


    public bool isJump,isGround;
    private bool isHurt = false;

    public float FlashCoolDown;  //瞬移的冷却时间
    private float FlashCount;  //记录瞬移冷却时间结束否

    public float FlashTime;  //瞬移持续时间
    private float FlashTimeLeft;  //记录瞬移持续时间结束否
    public float FlashSpeed;  //瞬移速度
    private bool IsFlashing;  //判断是否正在瞬移
    private int BoxPos;  //正在选择的物品栏位置
    private bool IsInBox;  //判断当前是否正在物品栏执行操作

    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        FlashCount = FlashCoolDown;
        BoxPos = 0;
    }

    void Update(){
        if(Input.GetButtonDown("Jump")&&jumpCount>0){
            jumpPressed=true;
        }
        if(Input.GetButtonDown("Attack")&&!isHurt){
            Attack();
        }
        if (Input.GetKey(KeyCode.T))
        {
            Throw(0);
        }
        if(Input.GetKey(KeyCode.L))
        {
            if(FlashCount >= FlashCoolDown)
            {
                ReadyToFlash();               
            }

        }

        if(Input.GetKeyDown(KeyCode.O))  //进行扔物品操作
        {
            IsInBox = !IsInBox;
        }
        if(IsInBox)
        {
            ChangeBox();
        }

        FlashCount+=Time.deltaTime;
    }


    void FixedUpdate()
    {

        Flash();
        if (IsFlashing)
            return;
        isGround = Physics2D.OverlapCircle(groundCheck.position,0.2f,ground);
        if(!isHurt){
            Move();            
        }

        SwitchAnim();
        Jump();
    }

    void Move(){
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("running",Mathf.Abs(horizontalMove));

        //rb.velocity.x=horizontalMove * speed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime,rb.velocity.y);             

        if(horizontalMove != 0){
            transform.localScale = new Vector3(horizontalMove,1,1);
        }
    }

    void Attack(){
        anim.SetBool("attack",true);
    }

    void QuitAttack(){
        anim.SetBool("attack",false);
        SwitchAnim();
    }

    void SwitchAnim(){
        anim.SetBool("idle",false);

        if(rb.velocity.y < 0.1f&&!isGround){
            anim.SetBool("falling",true);
        }
        if(anim.GetBool("jumping")){
  
            if(rb.velocity.y<0){
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        else if(isHurt){
            anim.SetBool("hurt",true);
            if(Mathf.Abs(rb.velocity.x)<0.1f){
                isHurt = false;
                anim.SetBool("hurt",false);
                anim.SetBool("idle",true);
            }
        }
        else if(isGround){
            anim.SetBool("falling",false);
            anim.SetBool("idle",true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag=="Collection"){
            //Destroy(collision.gameObject);
            //cherryNum += 1;
            //cherryNumText.text = cherryNum.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy"){

            Frog frog = other.gameObject.GetComponent<Frog>();

            if(anim.GetBool("attack")){
                frog.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x , jumpForce * Time.deltaTime);
                anim.SetBool("jumping",true);
            }
            else{
                GetHurt(); 
                if(transform.position.x < other.gameObject.transform.position.x)   {
                    rb.velocity = new Vector2(-5 , rb.velocity.y);                    
                }
                else{
                    rb.velocity = new Vector2(5 , rb.velocity.y);                    
                }            
            }
        }
    }

    void GetHurt(){
        isHurt = true;        
        blood-=10;        
    }


    void Jump(){
        if(isGround){
            jumpCount = 1;
            isJump = false;
        }
        if(jumpPressed && isGround){
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            anim.SetBool("jumping",true);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount > 0 && isJump){
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            jumpCount--;       
            jumpPressed = false;     
        }
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Garbage")
        {
            //collision.GetComponent<Garbage>().Type();
            if (Input.GetKey(KeyCode.P))
            {
                if (trashNum>=3)
                {
                    //提示已满
                }
                else
                {
                    trash[trashNum] = collision.GetComponent<Garbage>().Type();
                    trashNum += 1;
                    UICtrler.UIinstence.GetGarbageInfo(collision.GetComponent<Garbage>().GetSprite(),collision.name);
                    Destroy(collision.gameObject);
                }
            }
        }
    }


    bool Throw(int index)
    {
        Sprite DropSprite = UICtrler.UIinstence.GropGarbage(index);
        if (DropSprite != null)
        {
            GarbagePrefab.GetComponent<SpriteRenderer>().sprite = DropSprite;
            GameObject MyGarbage = Instantiate(GarbagePrefab);
            MyGarbage.name = UICtrler.UIinstence.ReturnName(index);
            Vector3 pos = new Vector3(0, -0.5f, 0);
            MyGarbage.transform.position = this.transform.position + pos ;
            trashNum -= 1;
            return true;
        }
        else
            return false;
    }

void Rush()
    {

    }
    void ReadyToFlash()
    {
        IsFlashing = true;
        FlashTimeLeft = FlashTime;
        FlashCount = 0;
    }
    void Flash()
    {
        if (IsFlashing)
        {
            if (FlashTimeLeft > 0)
            {
                rb.velocity = new Vector2(FlashSpeed * this.transform.localScale.x, rb.velocity.y);
                FlashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFromPool();
            }
            if (FlashTimeLeft <= 0)
            {
                IsFlashing = false;
            }
        }
    }

    void ChangeBox()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            BoxPos = (BoxPos + 2) % 3;
        }   
        if(Input.GetKeyDown(KeyCode.E))
        {
            BoxPos = (BoxPos + 1) % 3;
        }
    }
}

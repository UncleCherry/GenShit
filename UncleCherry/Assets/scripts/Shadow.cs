using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("时间管理参数")]
    public float activeTime;
    public float activeStart;

    [Header("不透明度管理参数")]
    private float alpha;
    public float alphaSet;
    public float alphaMultiplier;

    public void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        thisSprite = this.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        thisSprite.sprite = playerSprite.sprite;

        this.transform.position = player.position;
        this.transform.localScale = player.localScale;
        this.transform.rotation = player.rotation;

        activeStart = Time.time;
    }
    void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(0.5f, 0.5f, 1, alpha);
        thisSprite.color = color;
        if (Time.time >= activeStart + activeTime)
        {
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}

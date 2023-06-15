using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrler : MonoBehaviour
{
    public static UICtrler UIinstence;
    private int GarbageNum;
    Image[] father;
    Sprite ReturnSprite;
    // Start is called before the first frame update
    void Start()
    {
        UIinstence = this;
        GarbageNum = 0;
        father = GetComponentsInChildren<Image>();
        
    }
    public void GetGarbageInfo(Sprite sprite,string name)
    {
        foreach (var child in father)
        {
            if(child.sprite==null)
            {
                child.sprite = sprite;
                child.name = name;
                break;
            }
        }
    }

    public Sprite GropGarbage(int index)
    {
        ReturnSprite = father[index].sprite;
        if (ReturnSprite != null)
        {
            father[index].sprite=null;
            return ReturnSprite;
        }
        else
        {
            return null;
        }
    }

    public string ReturnName(int index)
    {
        return father[index].name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{

    private Sprite sprite;
    private int type=0;
    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>().sprite;
        if (this.name == "Straw" || this.name == "LunchBox" || this.name == "Glass")
        {
            type = 0;
        }
        else if (this.name == "Paper" || this.name == "Can" || this.name == "Basketball")
        {
            type = 1;
        }
        else if (this.name == "BananaPeel" || this.name == "Eggshell" || this.name == "Leaf")
        {
            type = 2;
        }
    }

    public int Type()
    {
        return type;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

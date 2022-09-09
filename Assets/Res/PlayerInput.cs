using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("===== Key settings=====")]
    public string KeyUp = "w";    
    public string KeyDown = "s";   
    public string KeyLeft = "a";
    public string KeyRight = "d";

    public string KeyA = "left shift";
    public string KeyB = "c";
    public string KeyC = "space";
    public string KeyD;

    [Header("=====Output signals=====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    //1.pressing signal
    public bool run;
    //2.trigger signal
    public bool roll;
    public bool jump;
    private bool lastJump;
    //3.double sognal

    [Header("=====Others=====")]
    public bool SWITCH = true;   

    public float targetDup;
    public float targetDright;
    public float velocityDup;
    public float velocityDright;

    // Update is called once per frame
    void Update()
    {
        //将玩家输入做一个转换
        targetDup = (Input.GetKey(KeyUp)?1.0f:0) - (Input.GetKey(KeyDown)?1.0f:0);
        targetDright = (Input.GetKey(KeyRight)?1.0f:0) - (Input.GetKey(KeyLeft)?1.0f:0);
        //开关
        if(SWITCH == false){
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp (Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp (Dright, targetDright, ref velocityDright, 0.1f);
        //解决斜向走路的时候会出现的加速问题
        Vector2 temp = SquareToCricle(new Vector2(Dright, Dup));

        float Dright2 = temp.x;
        float Dup2 = temp.y;

        Dmag = Mathf.Sqrt((Dup2*Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
        //跑步
        run = Input.GetKey(KeyA);
        //跳跃
        bool newjump = Input.GetKey(KeyB);
        if (newjump != lastJump && newjump == true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = newjump;
        //翻滚
        roll = Input.GetKey(KeyC);
    }

    private Vector2 SquareToCricle(Vector2 input){
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject model;
    public PlayerInput pi;

    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;
    public float rollVelocity = 0f;
    Vector3 rollVelocity1 = new Vector3(0,0,1);

    [SerializeField]
    private Vector3 movingVec;
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planerVec;
    private Vector3 thrustVec;

    private bool lockPlaner = false;

    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //设置动画混合树
        //使用lerp函数做插值，使切换不那么生硬
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? runSpeed : 1.0f), 0.1f));
        //设置翻滚信号
        if (rigid.velocity.magnitude > 10.0f)
        {
        //   anim.SetTrigger("roll");
        }

        if (pi.roll)
        {
            anim.SetTrigger("roll");
        }
        //设跳跃信号
        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }
        
        //设置人物朝向
        if(pi.Dmag > 0.1f){
            model.transform.forward = Vector3.Slerp (model.transform.forward, pi.Dvec, 0.2f);
        }

        if(lockPlaner == false)
        {
            //planerVec = walkSpeed * pi.Dmag * model.transform.forward * ((pi.run) ? runSpeed : 1.0f);
            planerVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runSpeed : 1.0f);
        }
        //movingVec = walkSpeed * pi.Dmag * model.transform.forward * ((pi.run) ? runSpeed : 1.0f);
    }

    private void FixedUpdate()
    {
        //rigid.position += new Vector3( 0,0,1.0f) * Time.fixedDeltaTime;
        //print(rigid.position);
        //rigid.position = rigid.position + planerVec * Time.fixedDeltaTime + thrustVec;
        rigid.velocity = new Vector3(planerVec.x, rigid.velocity.y, planerVec.z) + rollVelocity1 * rollVelocity + thrustVec;
        thrustVec = Vector3.zero;
    }
    /**
     * 
     * =====================以下为接受msg===========================================
     */

    public void JumpEnter()
    {
        //pi.SWITCH = false;
        //lockPlaner = true;
        thrustVec = new Vector3(0, 4f, 0);
        //print("JumpEnter");
    }
    /*
    public void JumpExit()
    {
        pi.SWITCH = true;
        lockPlaner = false;
        //print("JumpExit");
    }*/
    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.SWITCH = true;
        lockPlaner = false;
    }

    public void OnGroundExit()
    {
        pi.SWITCH = false;
        lockPlaner = true;
    }

    public void OnRollEnter()
    {
        rollVelocity1 = new Vector3(0, 0, 1f);
        rollVelocity = 10f;
        pi.SWITCH = false;
        lockPlaner = true;
        //thrustVec = new Vector3(0, 0, rollVelocity);
    }

    public void OnRollExit()
    {
        rollVelocity = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("점프 설정")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;                          //중력 속도 추가
    public float landingDuration = 0.3f;                    //착지 후 착지 지속 시간


    [Header("공격 설정")]
    public float attackDuration = 0.8f;                     //공격 지속 시간
    public bool canMoveWhileAttacking = false;              //공격중 이동 가능 여부

    [Header("컨포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //현재상태
    private float currentSpeed;
    private bool isAttacking = false;                      //공격중인지 체크
    private bool isLanding = false;                        //착지 중인지 확인
    private float landingTimer;                            //착지 타이머

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;                             //이전 프레임에 땅이였는지
    private float attackTimer;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimator();
    }

    void CheckGrounded()
    {
        //이전 상태를 저장
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;                         //캐릭터 컨트롤러에서 받아온다.
        
        if(!isGrounded && wasGrounded)                              //땅에서 떨어졌을 때 (지금 프레임은 땅이 아니고, 이전 프레임은 땅
        {
            Debug.Log("떨어지기 시작");
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            //착지 모션 트리거 및 착지 상태 시작
            if(!wasGrounded && animator != null)
            {
                //animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("착지");
            }
        }
    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;                    //랜딩 타이머 시간만큼 못움직임
            if(landingTimer <= 0)
            {
                isLanding = false;                    //착지 완료
            }
        }
    }
    
    void HandleAttack()
    {
        if(isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0)
            {
                isAttacking = false;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;

            if (animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }

    void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if(animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }

        if(!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()                                 //이동 함수 제작
    {
        if((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0f;
            return;
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || Vertical != 0)
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * Vertical + cameraRight * horizontal;

            if(Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;
        }
    }

    void UpdateAnimator()
    {
        //전체 최대 속도(runSpeed)기준으로 0 ~ 1 계산

        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("speed", animatorSpeed);
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;                         //캐릭터의 Y축 속도가 음수로 넘어가면 떨어지고 있다고 판단
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);

    }
}

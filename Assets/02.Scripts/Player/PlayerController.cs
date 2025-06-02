using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour, IHandleMovement, IHandleCrouch, IHandleJump, IHandleLook
{
    private CharacterController controller;
    private FPSCamNoiseController noiseController;
    private PlayerInteraction interaction;

    [Header("Pressed Keys")]
    public bool sprintPressed = false;
    public bool crouchPressed = false;
    public bool jumpPressed = false;

    [Header("Move")]
    private float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    private Vector2 moveInput;

    [Header("Jump")]
    public float jumpForce;
    private float verticalVelocity;
    private float gravity = -9.81f;

    [Header("Crouch")]
    public Transform crouchHead;
    public Transform standHead;
    public float crouchLerpSpeed;
    public float standHeight;
    public float crouchHeight;
    public float crouchCenterY;
    public LayerMask overheadObstacleLayer;

    [Header("Look")]
    [Range(0f, 0.5f)] public float mouseSensitivity;
    public Transform camContainer;
    private Transform cam;
    private Vector2 mouseInput;
    private float camXRot;

    private void Awake()
    {
        //예외 처리
        if (!TryGetComponent<CharacterController>(out controller))
        {
            Debug.Log("controller is null");
        }
        if (!TryGetComponent<FPSCamNoiseController>(out noiseController))
        {
            Debug.Log("noiseController is null");
        }
        if (!TryGetComponent<PlayerInteraction>(out interaction))
        {
            Debug.Log("interaction is null");
        }
        if (camContainer == null)
        {
            Debug.LogError("camContainer is null");
            return;
        }
        if (camContainer.GetComponentInChildren<CinemachineVirtualCamera>() == null)
        {
            Debug.LogError("cam is null");
            return;
        }
        cam = camContainer.GetComponentInChildren<CinemachineVirtualCamera>().transform;

        Cursor.lockState = CursorLockMode.Locked;   //마우스 잠금
        standHead.position = cam.transform.position;
    }

    void Update()
    {
        HandleMovement();
        HandleCrouch();
        HandleJump();
    }

    void LateUpdate()
    {
        HandleLook();
    }

    //--------------이동 메서드--------------//
    public void HandleMovement()
    {
        //입력되는 키에 따라 걷는 속도 변경
        currentSpeed = crouchPressed ? crouchSpeed : (sprintPressed ? sprintSpeed : walkSpeed);

        Vector3 moveDir = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        moveDir *= currentSpeed;
        moveDir.y = verticalVelocity;
        controller.Move(moveDir * Time.deltaTime);
    }

    //--------------앉기 메서드--------------//
    public void HandleCrouch()
    {
        if (crouchPressed)
        {
            controller.height = crouchHeight;
            controller.center = new Vector3(0f, crouchCenterY, 0f);
            cam.position = Vector3.Lerp(cam.position, crouchHead.position, Time.deltaTime * crouchLerpSpeed);
        }
        else
        {
            if (!DetectOverheadObstacle())
            {
                crouchPressed = false;
                controller.height = standHeight;
                controller.center = new Vector3(0f, 0f, 0f);
                cam.position = Vector3.Lerp(cam.position, standHead.position, Time.deltaTime * crouchLerpSpeed);
            }
            else
            {
                //머리 위에 장애물 감지 시, 강제로 앉기 입력
                crouchPressed = true;
            }
        }
    }

    //--------------점프 메서드--------------//
    public void HandleJump()
    {
        if (jumpPressed)
        {
            verticalVelocity = jumpForce;
            jumpPressed = false;
        }

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // 지면에 닿을 때 중력 리셋
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    //--------------시점 메서드--------------//
    public void HandleLook()
    {
        if (camContainer == null)
        {
            Debug.LogError("CamContainer is null");
            return;
        }

        camXRot += mouseInput.y * mouseSensitivity;
        camXRot = Mathf.Clamp(camXRot, -90f, 90f);
        camContainer.localEulerAngles = new Vector3(-camXRot, 0f, 0f);
        transform.eulerAngles += new Vector3(0f, mouseInput.x * mouseSensitivity, 0f);
    }

    //--------------머리 위 장애물 감지 메서드--------------//
    public bool DetectOverheadObstacle()
    {
        Ray[] ray = new Ray[4]
        {
            new Ray(crouchHead.position + (crouchHead.forward * 0.2f) + (crouchHead.right * 0.2f), Vector3.up),
            new Ray(crouchHead.position + (crouchHead.forward * 0.2f) + (-crouchHead.right * 0.2f), Vector3.up),
            new Ray(crouchHead.position + (-crouchHead.forward * 0.2f) + (crouchHead.right * 0.2f), Vector3.up),
            new Ray(crouchHead.position + (-crouchHead.forward * 0.2f) + (-crouchHead.right * 0.2f), Vector3.up),
        };

        RaycastHit hit;
        for (int i = 0; i < ray.Length; i++)
        {
            Debug.DrawRay(ray[i].origin, ray[i].direction);
            if (Physics.Raycast(ray[i], out hit, 0.5f, overheadObstacleLayer))
            {
                return true;
            }
        }
        return false;
    }

    // ===== InputSystem 연동 메서드 =====
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        mouseInput = value.Get<Vector2>();
    }

    private void OnSprint(InputValue value)
    {
        if (controller.isGrounded && !crouchPressed)
        {
            sprintPressed = value.isPressed;
        }
    }

    private void OnJump(InputValue value)
    {
        if (controller.isGrounded && !crouchPressed)
        {
            jumpPressed = value.isPressed;
        }
    }

    private void OnInteract(InputValue value)
    {
        interaction.InteractObject();
    }

    private void OnCrouch(InputValue value)
    {
        if (controller.isGrounded && !sprintPressed)
        {
            crouchPressed = !crouchPressed;
        }
    }
}

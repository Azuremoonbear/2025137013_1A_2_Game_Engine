using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("움직임 설정")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    [Header("카메라 설정")]
    public CinemachineVirtualCamera virtualCam;
    public float walkFOV = 40f;         // 걸을 때의 카메라 FOV
    public float sprintFOV = 55f;       // 달릴 때의 카메라 FOV
    public float fovChangeSpeed = 8f;   // FOV가 변하는 속도

    [Header("핵심 구성요소")]
    private CharacterController controller;
    public CinemachineSwitcher cameraSwitcher;

    private CinemachinePOV pov;
    private Vector3 velocity;
    public bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
        // 시작할 때 카메라의 FOV를 걷기 상태의 FOV로 설정
        virtualCam.m_Lens.FieldOfView = walkFOV;
    }

    void Update()
    {
        if (cameraSwitcher != null && cameraSwitcher.usingFreeLook)
        {
            return; // Update 함수를 즉시 종료하여 아래의 모든 로직(이동, 점프, 회전 등)을 실행하지 않음
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = GetCameraRelativeMovement(z, x);

        float currentSpeed;
        float targetFOV; // --- 목표 FOV를 저장할 변수 추가 ---

        if (Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0) // 달리기 조건 추가: 움직일 때만
        {
            currentSpeed = sprintSpeed;
            targetFOV = sprintFOV; // 달릴 때는 sprintFOV를 목표로 설정
        }
        else
        {
            currentSpeed = walkSpeed;
            targetFOV = walkFOV;   // 걸을 때는 walkFOV를 목표로 설정
        }

        controller.Move(move * currentSpeed * Time.deltaTime);

        // --- ✨ 카메라 FOV를 부드럽게 변경하는 로직 추가 ---
        virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, targetFOV, fovChangeSpeed * Time.deltaTime);
        // --------------------------------------------------

        RotateToCameraDirection();
        ApplyGravityAndJump();
    }

    // --- 가독성을 위해 로직을 함수로 분리 ---

    private Vector3 GetCameraRelativeMovement(float forwardInput, float rightInput)
    {
        Vector3 camForward = virtualCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        return (camForward * forwardInput + camRight * rightInput).normalized;
    }

    private void RotateToCameraDirection()
    {
        float cameraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    private void ApplyGravityAndJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
using Cinemachine;
using UnityEngine;

public enum eState
{
    Idle,
    Walk,
    Sprint,
    Jump
}

public class FPSCamNoiseController : MonoBehaviour
{
    //해당 오브젝트를 플레이어의 Input Action에 넣어주세요
    [Header("Gain Settings")]
    public eState curState;  //현재 상태

    [Header("Idle")]    //기본 상태의 흔들림 폭과 주기
    public float amplitudeOnIdle = 0.2f;
    public float frequencyOnIdle = 0.005f;

    [Header("Walk")]    //걷는 상태의 흔들림 폭과 주기
    public float amplitudeOnWalk = 0.2f;
    public float frequencyOnWalk = 0.02f;

    [Header("Run")]     //달리는 상태의 흔들림 폭과 주기
    public float amplitudeOnRun = 0.3f;
    public float frequencyOnRun = 0.04f;

    [Header("Jump")]     //점프 상태의 흔들림 폭과 주기
    public float amplitudeOnJump = 0f;
    public float frequencyOnJump = 0f;

    [Header("Components")]
    public NoiseSettings noiseSetting;
    public CinemachineVirtualCamera FPS_cam;   //1인칭 시네머신 컴포넌트
    private CinemachineBasicMultiChannelPerlin noise;   //1인칭 시네머신의 노이즈

    private PlayerController playerController;
    private CharacterController characterController;


    private void Awake()
    {
        noise = FPS_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        noise.m_NoiseProfile = noiseSetting;
        StateSwitch(eState.Idle);
    }

    private void Update()
    {
        OnMoveInput();
    }

    //--------------상태 변경 메서드--------------//
    public void OnMoveInput()
    {
        if (!characterController.isGrounded)
        {
            StateSwitch(eState.Jump);
        }
        else if (playerController.sprintPressed && characterController.velocity.magnitude > 0.1f)
        {
            StateSwitch(eState.Sprint);
        }
        else if (characterController.velocity.magnitude > 0.1f)
        {
            StateSwitch(eState.Walk);
        }
        else
        {
            StateSwitch(eState.Idle);
        }
    }

    //--------------상태를 받아 진폭&주기 변경 메서드를 호출--------------//
    public void StateSwitch(eState _state)
    {
        curState = _state;
        switch (curState)
        {
            case eState.Idle:
                NoiseHandler(amplitudeOnIdle, frequencyOnIdle);
                break;
            case eState.Walk:
                NoiseHandler(amplitudeOnWalk, frequencyOnWalk);
                break;
            case eState.Sprint:
                NoiseHandler(amplitudeOnRun, frequencyOnRun);
                break;
            case eState.Jump:
                NoiseHandler(amplitudeOnJump, frequencyOnJump);
                break;

        }
    }

    //--------------진폭과 주기를 받아 변경함--------------//
    public void NoiseHandler(float _amplitude, float _frequency)
    {
        noise.m_AmplitudeGain = _amplitude;
        noise.m_FrequencyGain = _frequency;
    }
}

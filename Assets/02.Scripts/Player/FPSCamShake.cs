using Cinemachine;
using UnityEngine;

public class FPSCamShake : MonoBehaviour
{
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
    private CinemachineBasicMultiChannelPerlin perlin;   //1인칭 시네머신의 노이즈

    private void Awake()
    {
        perlin = FPS_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        perlin.m_NoiseProfile = noiseSetting;
        ShakeSwitch(eState.Idle);
    }

    //--------------상태를 받아 진폭&주기 변경 메서드를 호출--------------//
    public void ShakeSwitch(eState _state)
    {
        switch (_state)
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
        perlin.m_AmplitudeGain = _amplitude;
        perlin.m_FrequencyGain = _frequency;
    }
}

using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoScene : MonoBehaviour
{
    private Image logoImage;  //로고 이미지  
    public AudioClip audioClip;
    private AudioSource audioSource;


    public float blackDuration; // 처음 검은 화면 시간
    public float logoFadeRate;  // 페이드 인, 아웃 시간
    public float logoDuration;  // 켜져있는 시간
    public float soundVolume;   // 최대 사운드 볼륨

    public SceneAsset nextScene;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        logoImage = GetComponentInChildren<Image>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(LogoOn());
    }

    private IEnumerator LogoOn()
    {
        float time = 0f;
        logoImage.color = new Color(1f, 1f, 1f, time);

        yield return new WaitForSeconds(blackDuration);    //3초 대기

        audioSource.clip = audioClip;
        audioSource.volume = 0f;
        audioSource.Play();     //사운드 플레이

        // Fade In
        while (time < logoFadeRate)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Clamp01(time / logoFadeRate);
            audioSource.volume = alpha * soundVolume;       //사운드 페이드 인
            logoImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(logoDuration);

        // Fade Out
        time = 0f;
        while (time < logoFadeRate)
        {
            
            time += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(time / logoFadeRate);
            audioSource.volume = alpha * soundVolume;       //사운드 페이드 아웃
            logoImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        SceneManager.LoadScene(nextScene.name);
    }
}

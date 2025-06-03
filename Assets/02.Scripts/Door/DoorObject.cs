using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorObject : MonoBehaviour, IInteractable
{
    public Sprite crosshairIcon;
    public AudioClip DoorClip;
    [Range(0f, 1f)] public float clipVolume;
    private AudioSource audioSource;
    private bool isOpen = false;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            Debug.Log("audioSource is null");
        }
        audioSource.volume = clipVolume;
    }

    public string PromptUI()
    {
        string promptText = string.Empty;
        return promptText;
    }

    //--------------크로스헤어 이미지 반환 메서드--------------//
    public Sprite GetCrosshairIcon()
    {
        return crosshairIcon;
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {
        isOpen = !isOpen;
        switch (isOpen)
        {
            case true:
                OpenDoor();
                break;
            case false:
                CloseDoor();
                break;
        }
        audioSource.PlayOneShot(DoorClip);
    }

    //--------------문을 열고 닫는 메서드--------------//
    private void OpenDoor()
    {
        Debug.Log("Open");
    }

    private void CloseDoor()
    {
        Debug.Log("Close");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    private AudioSource audioSource;
    public AudioClip itemPickUpClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PickUpClipPlay()
    {
        audioSource.PlayOneShot(itemPickUpClip);
    }
}

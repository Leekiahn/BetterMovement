using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public Sprite crosshairIcon;
    public AudioClip pickupClip;
    [Range(0f, 1f)] public float clipVolume;
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private new Collider collider;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        if(!TryGetComponent<AudioSource>(out audioSource))
        {
            Debug.Log("audioSource is null");
        }
        if (!TryGetComponent<MeshRenderer>(out meshRenderer))
        {
            Debug.Log("meshRenderer is null");
        }
        if (!TryGetComponent<Collider>(out collider))
        {
            Debug.Log("collider is null");
        }
        if (!TryGetComponent<Rigidbody>(out rigidbody))
        {
            Debug.Log("rigidbody is null");
        }
    }

    //--------------프롬프트 텍스트 반환 메서드--------------//
    public string PromptUI()
    {
        string promptText = $"{itemData.itemName}";
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
        //픽업 클립이 실행되고 종료되면 Destroy
        audioSource.volume = clipVolume;
        audioSource.PlayOneShot(pickupClip);
        DisableItem();
        Destroy(gameObject, pickupClip.length);
    }

    //--------------아이템을 상호작용 불가능하게 하는 메서드--------------//
    private void DisableItem()
    {
        meshRenderer.enabled = false;
        collider.enabled = false;
        rigidbody.useGravity = false;
    }
}

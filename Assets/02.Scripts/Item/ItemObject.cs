using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public Sprite crosshairIcon;

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
        //픽업 효과음 재생
        SFXManager.Instance.PickUpClipPlay();
        Destroy(gameObject);
    }
}

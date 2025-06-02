using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    //--------------프롬프트 텍스트 반환 메서드--------------//
    public string PromptUI()
    {
        string promptText = $"{itemData.itemName}";
        return promptText;
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {
        Destroy(gameObject);
    }
}

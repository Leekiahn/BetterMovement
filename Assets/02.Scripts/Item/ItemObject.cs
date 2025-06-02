using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    //--------------������Ʈ �ؽ�Ʈ ��ȯ �޼���--------------//
    public string PromptUI()
    {
        string promptText = $"{itemData.itemName}";
        return promptText;
    }

    //--------------��ȣ�ۿ� �޼���--------------//
    public void Interact()
    {
        Destroy(gameObject);
    }
}

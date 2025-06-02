using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상호작용 가능한 오브젝트 스크립트에 상속
public interface IInteractable
{
    public string PromptUI();
    public void Interact();
    public Sprite GetCrosshairIcon();
}

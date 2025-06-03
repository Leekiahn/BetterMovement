using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact settings")]
    public float detectDistance;
    public Transform cam;
    public TextMeshProUGUI promptUI;
    [SerializeField] private GameObject curDetectObject;

    public IInteractable interactableObject;
    public LayerMask interactableLayer;

    [Header("references")]
    public Image crosshair;
    public GameObject keyUI;
    public Sprite itemInteractIcon;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        DetectObject();
    }

    //--------------오브젝트 감지 메서드--------------//
    private void DetectObject()
    {
        Ray ray = new Ray(cam.position, cam.forward * detectDistance);
        RaycastHit hit;

        //상호작용 가능한 레이어 오브젝트 감지
        if (Physics.Raycast(ray, out hit, detectDistance, interactableLayer))
        {
            curDetectObject = hit.collider.gameObject;

            //상호작용 가능한 오브젝트일 때
            if (curDetectObject.TryGetComponent<IInteractable>(out interactableObject))
            {
                keyUI.SetActive(true);
                promptUI.text = interactableObject.PromptUI();
                crosshair.sprite = interactableObject.GetCrosshairIcon();
                crosshair.color = Color.white;
            }
        }
        else
        {
            // 오브젝트 감지가 끝남
            curDetectObject = null;
            interactableObject = null;
            crosshair.sprite = null;
            crosshair.color = new Color(0, 0, 0, 0);
            keyUI.SetActive(false);
            promptUI.text = string.Empty;
        }
    }

    //--------------오브젝트의 특정 컴포넌트를 반환하는 메서드--------------//
    public T DetectSpecificComponent<T>() where T : Component
    {
        if (curDetectObject != null)
        {
            return curDetectObject.GetComponent<T>();
        }
        return null;
    }

    //--------------오브젝트 상호작용 메서드--------------//
    public void InteractObject()
    {
        if (curDetectObject != null)
        {
            //상호작용 가능한 오브젝트를 탐지했을 때
            if (interactableObject != null)
            {
                interactableObject.Interact();
                interactableObject = null;
            }
        }
    }
}

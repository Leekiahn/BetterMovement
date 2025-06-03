using UnityEngine;

public class FootStepSFX : MonoBehaviour
{
    [Header("Settings")]
    public Transform foot;

    [Header("Walk")]
    public float walkPeriod = 0.8f;

    [Header("Sprint")]
    public float sprintPeriod = 0.4f;

    [Header("Clips")]
    public AudioClip[] dirtClips;
    public AudioClip[] grassClips;
    public AudioClip[] woodClips;
    //추가

    private AudioClip[] footStepClips;
    private AudioClip curClip;

    private AudioSource audioSource;
    private PlayerController playerController;
    private CharacterController characterController;

    private float time = 0f;

    void Awake()
    {
        audioSource = foot.gameObject.GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //걷기 및 달리기 발자국 SFX
        if (characterController.isGrounded)
        {
            DetectSurface();

            time += Time.deltaTime;
            if (characterController.velocity.magnitude > 0.1f && !playerController.crouchPressed)
            {
                float period = playerController.sprintPressed ? sprintPeriod : walkPeriod;

                if (time > period)
                {
                    audioSource.PlayOneShot(curClip);
                    time = 0;
                }
            }
        }
    }

    //--------------표면 감지 메서드--------------//
    private void DetectSurface()
    {
        Ray ray = new Ray(foot.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.2f))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();

            if (terrain != null)
            {
                //터레인 텍스처의 이름을 클립 변경 메서드로 보냄
                int index = GetDominantTerrainTextureIndex(hit.point, terrain);
                string textureName = terrain.terrainData.terrainLayers[index].diffuseTexture.name;

                footStepClips = FootStepClipSwitch(textureName);
            }
            else
            {
                //머터리얼의 이름을 클립 변경 메서드로 보냄
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    string materialName = material.name;

                    footStepClips = FootStepClipSwitch(materialName);
                }
            }

            if (footStepClips != null)
            {
                curClip = footStepClips[Random.Range(0, footStepClips.Length)];
            }
        }
    }

    //--------------터레인 텍스처 감지 메서드--------------//
    int GetDominantTerrainTextureIndex(Vector3 hitPoint, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPos = hitPoint - terrain.transform.position;

        int mapX = Mathf.FloorToInt(terrainPos.x / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.FloorToInt(terrainPos.z / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] alphaMap = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int maxIndex = 0;
        float maxMix = 0;

        for (int i = 0; i < alphaMap.GetLength(2); i++)
        {
            if (alphaMap[0, 0, i] > maxMix)
            {
                maxMix = alphaMap[0, 0, i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }

    //--------------텍스처를 받아 클립을 변경해주는 메서드--------------//
    private AudioClip[] FootStepClipSwitch(string _surfaceName)
    {
        string[] _textureNames = _surfaceName.Split('_'); //텍스처 이름을 '_'로 분리하여 배열로 만듭니다.

        //ex) m_dirt_01 -> dirt
        switch (_textureNames[1])
        {
            //추가 및 텍스트 수정 필요
            case "dirt":
                return dirtClips;
            case "grass":
                return grassClips;
            case "wood":
                return woodClips;
            default:
                return null;
        }
    }
}

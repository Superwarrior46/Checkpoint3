using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeshTrail : MonoBehaviour
{
    private InputAction SpeedUp;

    private float meshRefreshRate = 0.1f;
    private float meshDestroyDelay = 2f;
    [SerializeField]private Transform spawnPosition;

    [Header("Shader Settings")]
    [SerializeField]private Material shaderMat;
    [SerializeField]private string shaderVarRef;
    [SerializeField]private float shaderVarRate = 0.1f;
    [SerializeField]private float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    [SerializeField]private float activeTime = 2f;
    void Awake()
    {
        SpeedUp = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (SpeedUp.WasPressedThisFrame())
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null) skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for(int i = 0; i<skinnedMeshRenderers.Length; i++)
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.SetPositionAndRotation(spawnPosition.position, spawnPosition.rotation);

                MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
                MeshFilter mf = gameObject.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = shaderMat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gameObject, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat(Material shaderMat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = shaderMat.GetFloat(shaderVarRef);
        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            shaderMat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}

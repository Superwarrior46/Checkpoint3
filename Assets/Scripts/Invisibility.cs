using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Invisibility : MonoBehaviour
{
    private InputAction invisibility;
    private CharacterMove characterMove;
    private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Material[] shaderMat;
    private Material[] originalMat;
    void Awake()
    {
        invisibility = InputSystem.actions.FindAction("Invisibility");
        characterMove = GetComponentInParent<CharacterMove>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originalMat = meshRenderer.materials;
    }

    void Update()
    {
        if (invisibility.WasPressedThisFrame())
        {
            meshRenderer.materials = shaderMat;
            StartCoroutine(ResetMat());
        }
    }
    IEnumerator ResetMat()
    {
        yield return new WaitForSeconds(10);
        meshRenderer.materials = originalMat;
    }
}

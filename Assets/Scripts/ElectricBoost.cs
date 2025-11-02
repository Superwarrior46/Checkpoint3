using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricBoost : MonoBehaviour
{
    private InputAction electricBoost;
    private CharacterMove characterMove;
    private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Material shaderMat;
    private Material originalMat;
    void Awake()
    {
        electricBoost = InputSystem.actions.FindAction("Interact");
        characterMove = GetComponentInParent<CharacterMove>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originalMat = meshRenderer.material;
    }

    void Update()
    {
        if(electricBoost.WasPressedThisFrame())
        {
            meshRenderer.material = shaderMat;
            StartCoroutine(ResetMat());
        }
    }
    IEnumerator ResetMat()
    {
        yield return new WaitForSeconds(10);
        meshRenderer.material = originalMat;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class LevelUp : MonoBehaviour
{
    private InputAction button;
    private CharacterMove characterMove;
    [SerializeField]private VisualEffect levelUp;
    private SkinnedMeshRenderer meshRenderer;
    [SerializeField]private Material shaderMat;
    private Material originalMat;
    private bool levelingUp;

    void Awake()
    {
        button = InputSystem.actions.FindAction("Crouch");
        characterMove = GetComponentInParent<CharacterMove>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originalMat = meshRenderer.material;
    }

    void Update()
    {
         if (button.WasPressedThisFrame() && !levelingUp)
         {
            characterMove.animator.SetTrigger("LevelUp");

            if(levelUp != null)levelUp.Play();

            levelingUp = true;
            StartCoroutine(ResetBool(levelingUp, 0.5f));

            meshRenderer.material = shaderMat;
            StartCoroutine(ResetMat());

            characterMove.walkSpeed = 0f;
            characterMove.runSpeed = 0f;
        }
    }

    IEnumerator ResetBool(bool boolToReset, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        levelingUp = false;
    }

    IEnumerator ResetMat()
    {
        yield return new WaitForSeconds(2.3f);
        meshRenderer.material = originalMat;
    }
}

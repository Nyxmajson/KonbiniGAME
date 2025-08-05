using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private PlayerControls controls;

    private bool interactPressed;     // pour la touche A / F
    private bool interactCompPressed; // pour la touche Y / X

    [Header("Interactor References")]
    [SerializeField] private Transform _interactionPoint;
    public float _interactionPointRadius = 0.5f; 
    [SerializeField] private float _capsuleHeight = 1.8f;
    [SerializeField] private float _capsuleRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] public InteractionPromptUI _interactionPromptUI;
    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private IInteractable _interactable;

    private void Awake()
    {
        controls = new PlayerControls();

        // Bind : Interact normal
        controls.Gameplay.Interact.performed += ctx => interactPressed = true;
        controls.Gameplay.Interact.canceled += ctx => interactPressed = false;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();
    }

    private void Update()
    {
        Vector3 halfHeight = Vector3.up * (_capsuleHeight / 2f - _capsuleRadius);
        Vector3 point1 = _interactionPoint.position + halfHeight;
        Vector3 point2 = _interactionPoint.position - halfHeight; 
        _numFound = Physics.OverlapCapsuleNonAlloc(point1, point2, _capsuleRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null)
            {
                // Prompt normal
                if (!_interactionPromptUI.IsDisplayed)
                {
                    _interactionPromptUI.SetUp(_interactable.InteractionPrompt);
                }

                if (interactPressed)
                {
                    _interactable.Interact(this);
                    interactPressed = false; // reset pour ?viter spam
                }
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;

        Vector3 halfHeight = Vector3.up * (_capsuleHeight / 2f - _capsuleRadius);
        Vector3 point1 = _interactionPoint.position + halfHeight;
        Vector3 point2 = _interactionPoint.position - halfHeight;

        Gizmos.DrawWireSphere(point1, _capsuleRadius);
        Gizmos.DrawWireSphere(point2, _capsuleRadius);
        Gizmos.DrawLine(point1 + Vector3.forward * _capsuleRadius, point2 + Vector3.forward * _capsuleRadius);
        Gizmos.DrawLine(point1 - Vector3.forward * _capsuleRadius, point2 - Vector3.forward * _capsuleRadius);
        Gizmos.DrawLine(point1 + Vector3.right * _capsuleRadius, point2 + Vector3.right * _capsuleRadius);
        Gizmos.DrawLine(point1 - Vector3.right * _capsuleRadius, point2 - Vector3.right * _capsuleRadius);
#endif
    }
}



using Derevo.UI;
using UnityEngine;

public sealed class UIButtonTester : MonoBehaviour
{
    [SerializeField] private UIButton Target;

    private void Awake()
    {
        Target.OnPointerDownEvent += OnPointerDown;
        Target.OnPointerUpEvent += OnPointerUp;
        Target.OnPointerEnterEvent += OnPointerEnter;
        Target.OnPointerExitEvent += OnPointerExit;
        Target.ActivationEvent += Activation;
        Target.DeactivationEvent += Deactivation;
    }
    private void OnPointerDown()
    {
        Debug.Log("Down");
    }
    private void OnPointerUp()
    {
        Debug.Log("Up");
    }
    private void OnPointerEnter()
    {
        Debug.Log("Enter");
    }
    private void OnPointerExit()
    {
        Debug.Log("Exit");
    }
    private void Activation()
    {
        Debug.Log("Activation");
    }
    private void Deactivation()
    {
        Debug.Log("Deactivation");
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        Target.Activate();
    }
    [ContextMenu("Deactivate")]
    public void Deactivate()
    {
        Target.Deactivate();
    }
}
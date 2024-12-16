


using Derevo.UI;
using UnityEngine;

public sealed class RadialSelectorTestet : MonoBehaviour
{
    [SerializeField] private RadialSelector Selector;
    [SerializeField] private int DirectionIndex;

    private void Awake()
    {
        Selector.ChangeValueEvent += ChangeValue;
    }
    private void ChangeValue(float value,int directionIndex)
    {
        Debug.Log("Value/directionIndex: " + value + "/" + directionIndex);
    }
    [ContextMenu("SetDirection")]
    public void SetDirection()
    {
        Selector.ChangeValueByIndex(DirectionIndex);
    }
}
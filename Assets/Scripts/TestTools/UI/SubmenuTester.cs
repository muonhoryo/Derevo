

using Derevo.UI;
using UnityEngine;

public sealed class SubmenuTester : MonoBehaviour
{
    [SerializeField] private Submenu Target;
    [ContextMenu("Show")]
    public void Show()
    {
        Target.Show();
    }
    [ContextMenu("Hide")]
    public void Hide()
    {
        Target.Hide();
    }
}
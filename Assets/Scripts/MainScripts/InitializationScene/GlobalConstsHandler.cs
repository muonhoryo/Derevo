

using Derevo;
using UnityEngine;

public sealed class GlobalConstsHandler : MonoBehaviour
{
    public static GlobalConsts Instance_ { get; private set; }

    [SerializeField]
    private GlobalConsts Consts;

    private void Awake()
    {
        if (Instance_ != null)
            throw new System.Exception("Have more than one GlobalConstsHandler");

        Instance_ = Consts;
        Destroy(this);
    }
}
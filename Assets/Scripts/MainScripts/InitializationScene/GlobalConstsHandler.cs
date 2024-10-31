

using Derevo;
using UnityEngine;

public sealed class GlobalConstsHandler : MonoBehaviour
{
    public static GlobalConsts Instance_ { get; private set; }

    [SerializeField]
    private GlobalConsts Consts;

    private void Awake()
    {
#if UNITY_EDITOR
        if (Instance_ != null)
            throw new System.Exception("Have more than one GlobalConstsHandler");
        else
            Instance_= Consts;
#else
                GlobalConsts.Instance = Consts;
#endif
        Destroy(this);
    }
}


using Derevo.Level;
using UnityEngine;

namespace Derevo.GUI
{
    public sealed class CellDirectionShower : MonoBehaviour 
    {
        private ValuableCell Target;
        [SerializeField] private GameObject DiffusionDirectionObj;
        [SerializeField] private GameObject ExtendDirectionObj;
        private void Awake()
        {
            LevelManager.ChangeCellDiffDirectionEvent += ChangeDiffusionDirectionAction;
        }
        private void OnDestroy()
        {
            Destroy(DiffusionDirectionObj);
            Destroy(ExtendDirectionObj);
            LevelManager.ChangeCellDiffDirectionEvent -= ChangeDiffusionDirectionAction;
        }
        private void ChangeDiffusionDirectionAction(LevelManager.ChangeCellDiffDIrectionEventInfo info)
        {
            if (info.ChangedCell == Target)
            {
                if (info.NewDirection != 0)
                {
                    if (!DiffusionDirectionObj.activeSelf)
                        DiffusionDirectionObj.SetActive(true);
                    RotateDirectionObj(DiffusionDirectionObj, ValuableCell.GetRotationValueFromDirection(info.NewDirection));
                }
                else
                {
                    if (DiffusionDirectionObj.activeSelf)
                        DiffusionDirectionObj.SetActive(false);
                }
            }
        }
        public void Initialize(ValuableCell Target)
        {
            if(Target is not ExtenderCell exCell)
            {
                Destroy(ExtendDirectionObj);
            }
            else
            {
                RotateDirectionObj(ExtendDirectionObj, ValuableCell.GetRotationValueFromDirection(exCell.ExtendDirection_));
            }
            this.Target = Target;
            DiffusionDirectionObj.SetActive(false);
        }

        private void RotateDirectionObj(GameObject directionObj,float rotation)
        {
            directionObj.transform.eulerAngles =
                new Vector3(directionObj.transform.eulerAngles.x,
                directionObj.transform.eulerAngles.y,
                rotation);
        }
    }
}


using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;

namespace Derevo.GUI
{
    public sealed class LevelGUIGenerator : MonoBehaviour
    {
        public static LevelGUIGenerator Instance_ { get; private set; }

        [SerializeField] private GameObject CellButtonPrefab;
        [SerializeField] private GameObject ButtonsCanvas;
        [SerializeField] private float PositionModifier;

        private void Awake()
        {
            Instance_ = this;
            GameSceneLevelInitialization.LevelLoadingDoneEvent += OnLoadingLevelAction;
        }
        private void OnDestroy()
        {
            GameSceneLevelInitialization.LevelLoadingDoneEvent -= OnLoadingLevelAction;
        }
        private void OnLoadingLevelAction()
        {
            GenerateLevel();
        }

        public void GenerateLevelTest()
        {
            GenerateLevel();
        }
        public static void GenerateLevel()
        {
            if (LevelManager.Width_ <= 0)
                throw new System.Exception("Level is not initialized");
            LevelCell cell;
            for(int i=0;i<LevelManager.Width_ ;i++)
            {
                for(int j=0;j<LevelManager.MaxHeight_;j++)
                {
                    cell=LevelManager.GetCell(i,j);
                    if (cell != null)
                        InstantiateLevelButton(cell, new Vector2Int(i, j));
                }
            }
        }
        private static void InstantiateLevelButton(LevelCell cell,Vector2Int cellPos)
        {
            var button=Instantiate(Instance_.CellButtonPrefab, Instance_.ButtonsCanvas.transform);
            bool needOffset = LevelManager.IsFirstCellBottom_ == (cellPos.x % 2 == 1);
            button.transform.position = new Vector3(cellPos.x* Instance_.PositionModifier, (needOffset?cellPos.y+0.5f:cellPos.y)*Instance_.PositionModifier, button.transform.position.z);
            if (cell is BlockCell)
            {
                Destroy(button.GetComponent<UnfixedCell>());
                Destroy(button.GetComponent<CellDirectionShower>());
                Destroy(button.GetComponent<CellParticleCounter>());
            }
            else if (cell is ValuableCell parCell)
            {
                UnfixedCell unfxdCell = button.GetComponent<UnfixedCell>();
                Destroy(unfxdCell.BlockCellVisual);
                button.GetComponent<CellDirectionShower>().Initialize(parCell);
                button.GetComponent<CellParticleCounter>().Initialize(parCell);
                if(!parCell.IsUnfixed_)
                {
                    Destroy(unfxdCell);
                }
                else
                {
                    unfxdCell.Initialize(cellPos);
                }
            }
            else
                throw new System.Exception("Unknown type of cell: " + cell.GetType().ToString());
        }
    }
}
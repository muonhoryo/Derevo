


using Derevo.GUI;
using Derevo.Level;
using Derevo.PlayerControl;
using Derevo.Visual;
using UnityEngine;

namespace Derevo.UI
{
    public sealed class CellsVisualManager : MonoBehaviour
    {
        private static CellsVisualManager Instance;

        private static UnfixedCell[][] LevelMap;

        public static UnfixedCell GetCell(int column,int row)
        {
            if (column < 0 || column >= LevelMap.Length || row >= LevelMap[column].Length)
                return null;

            return LevelMap[column][row];
        }

        public static float PhysicContainersRowHeight;

        [SerializeField] private GameObject CellButtonPrefab;
        [SerializeField] private GameObject ButtonsParent;
        [SerializeField] private float PositionModifier;

        private void Awake()
        {
            Instance = this;
            GameSceneLevelInitialization.LevelLoadingDoneEvent += OnLoadingLevelAction;
        }
        private void OnDestroy()
        {
            GameSceneLevelInitialization.LevelLoadingDoneEvent -= OnLoadingLevelAction;
        }
        private void OnLoadingLevelAction()
        {
            PhysicContainersRowHeight = GlobalConstsHandler.Instance_.ParticlesFixingRadius*Mathf.Sqrt(3);
            GenerateLevel();
        }
        public static void GenerateLevel()
        {
            if (LevelManager.Width_ <= 0)
                throw new System.Exception("Level is not initialized");

            LevelMap = new UnfixedCell[LevelManager.Width_][];
            for (int i = 0; i < LevelMap.Length; i++)
            {
                LevelMap[i] = new UnfixedCell[LevelManager.GetColumnLength(i)];
            }
            LevelCell cell;
            for (int i = 0; i < LevelMap.Length; i++)
            {
                for (int j = 0; j < LevelMap[i].Length; j++)
                {
                    cell = LevelManager.GetCell(i, j);
                    if (cell != null)
                        LevelMap[i][j] = InstantiateLevelButton(cell, new Vector2Int(i, j));
                }
            }
        }
        private static UnfixedCell InstantiateLevelButton(LevelCell cell, Vector2Int cellPos)
        {
            var button = Instantiate(Instance.CellButtonPrefab, Instance.ButtonsParent.transform);
            bool needOffset = LevelManager.IsFirstCellBottom_ == (cellPos.x % 2 == 1);
            button.transform.position = new Vector3(cellPos.x * Instance.PositionModifier, (needOffset ? cellPos.y + 0.5f : cellPos.y) * Instance.PositionModifier, button.transform.position.z);
            if (cell is BlockCell)
            {
                Destroy(button.GetComponent<UnfixedCell>());
                Destroy(button.GetComponent<CellDirectionShower>());
                Destroy(button.GetComponent<CellParticleCounter>());
                Destroy(button.GetComponent<ParticlesCellContainer>());
                return null;
            }
            else if (cell is ValuableCell parCell)
            {
                UnfixedCell unfxdCell = button.GetComponent<UnfixedCell>();
                ParticlesCellContainer parsContainer = button.GetComponent<ParticlesCellContainer>();
                Destroy(unfxdCell.BlockCellVisual);
                button.GetComponent<CellDirectionShower>().Initialize(parCell);
                button.GetComponent<CellParticleCounter>().Initialize(parCell);
                parsContainer.Initialize(cellPos);
                if (parCell.Value_ > 0) //Spawn diffusion particles as upload position
                {
                    DiffusionParticle[] particles = ParticlesSpawner.SpawnParticles(parsContainer.UploadPosition_, parCell.Value_);
                    parsContainer.UploadParticles(particles);
                }
                if (!parCell.IsUnfixed_)
                {
                    Destroy(unfxdCell);
                }
                else
                {
                    unfxdCell.Initialize(cellPos);
                }
                return unfxdCell;
            }
            else
                throw new System.Exception("Unknown type of cell: " + cell.GetType().ToString());
        }
    }
}
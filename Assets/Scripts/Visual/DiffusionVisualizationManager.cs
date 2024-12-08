


using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;
using Derevo.GUI;

namespace Derevo.Visual
{
    public sealed class DiffusionVisualizationManager : MonoBehaviour
    {
        public static DiffusionVisualizationManager Instance_ { get; private set; }

        [SerializeField] private GameObject DiffusionParticlePrefab;
        [SerializeField] private GameObject CellButtonPrefab;
        [SerializeField] private GameObject ButtonsParent;
        [SerializeField] private float PositionModifier;

        private static UnfixedCell[][] LevelMap;

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
        public static void GenerateLevel()
        {
            if (LevelManager.Width_ <= 0)
                throw new System.Exception("Level is not initialized");

            LevelMap=new UnfixedCell[LevelManager.Width_][];
            for(int i = 0; i < LevelMap.Length; i++)
            {
                LevelMap[i] = new UnfixedCell[LevelManager.GetColumnLength(i)];
            }
            LevelCell cell;
            for (int i = 0; i < LevelMap.Length; i++)
            {
                for (int j = 0; j <LevelMap[i].Length; j++)
                {
                    cell = LevelManager.GetCell(i, j);
                    if (cell != null)
                        LevelMap[i][j]= InstantiateLevelButton(cell, new Vector2Int(i, j));
                }
            }
        }
        private static UnfixedCell InstantiateLevelButton(LevelCell cell, Vector2Int cellPos)
        {
            var button = Instantiate(Instance_.CellButtonPrefab, Instance_.ButtonsParent.transform);
            bool needOffset = LevelManager.IsFirstCellBottom_ == (cellPos.x % 2 == 1);
            button.transform.position = new Vector3(cellPos.x * Instance_.PositionModifier, (needOffset ? cellPos.y + 0.5f : cellPos.y) * Instance_.PositionModifier, button.transform.position.z);
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
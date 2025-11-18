using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;
using UnityEngine.Serialization;

using DG.Tweening;


public class PBlockShapeSpawner : PSingleton<PBlockShapeSpawner>
{
    [Tooltip(
        "Setting this true means placing a block will add new block instantly, false means new shape blocks will be added only once all three are placed on the board.")]
    public bool keepFilledAlways = false;

    [FormerlySerializedAs("shapeBlockList")] [SerializeField] PShapeBlockList pShapeBlockList;

    [FormerlySerializedAs("shapeBlockList_Plus")] [SerializeField] PShapeBlockList pShapeBlockListPlus;

    [FormerlySerializedAs("ActiveShapeBlockModule")] [HideInInspector] public PShapeBlockList activePShapeBlockModule;

    [SerializeField] Transform[] ShapeContainers;

    List<int> shapeBlockProbabilityPool;

    int shapeBlockPoolCount = 1;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        if (PGameController.gameMode == GameMode.ADVANCE || PGameController.gameMode == GameMode.CHALLENGE)
        {
            activePShapeBlockModule = pShapeBlockListPlus;
        }
        else
        {
            activePShapeBlockModule = pShapeBlockList;
        }
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        #region blast mode

        if (PGameController.gameMode == GameMode.BLAST || PGameController.gameMode == GameMode.CHALLENGE)
        {
            keepFilledAlways = true;
        }

        #endregion

        Invoke("SetupPreviousSessionShapes", 0.2F);
        Invoke("CreateShapeBlockProbabilityList", 0.5F);
        Invoke("FillShapeContainer", 0.5F);
    }

    /// <summary>
    /// Setups the previous session shapes.
    /// </summary>
    void SetupPreviousSessionShapes()
    {
        if (PGameBoardGenerator.Instance.previousSessionData != null)
        {
            List<int> shapes = PGameBoardGenerator.Instance.previousSessionData.shapeInfo.Split(',').Select(Int32.Parse)
                .ToList();

            int shapeIndex = 0;
            foreach (int shapeID in shapes)
            {
                if (shapeID >= 0)
                {
                    CreateShapeWithID(ShapeContainers[shapeIndex], shapeID);
                }

                shapeIndex += 1;
            }
        }
    }

    /// <summary>
    /// Creates the shape block probability list.
    /// </summary>
    void CreateShapeBlockProbabilityList()
    {
        shapeBlockProbabilityPool = new List<int>();
        if (activePShapeBlockModule != null)
        {
            foreach (ShapeBlockSpawn shapeBlock in activePShapeBlockModule.ShapeBlocks)
            {
                AddShapeInProbabilityPool(shapeBlock.BlockID, shapeBlock.spawnProbability);
            }
        }

        shapeBlockProbabilityPool.Shuffle();
    }

    /// <summary>
    /// Adds the shape in probability pool.
    /// </summary>
    /// <param name="blockID">Block I.</param>
    /// <param name="probability">Probability.</param>
    void AddShapeInProbabilityPool(int blockID, int probability)
    {
        int probabiltyTimesToAdd = shapeBlockPoolCount * probability;

        for (int index = 0; index < probabiltyTimesToAdd; index++)
        {
            shapeBlockProbabilityPool.Add(blockID);
        }
    }

    /// <summary>
    /// Fills the shape container.
    /// </summary>
    public void FillShapeContainer()
    {
        ReorderShapes();

        if (!keepFilledAlways)
        {
            bool isAllEmpty = true;
            foreach (Transform shapeContainer in ShapeContainers)
            {
                if (shapeContainer.childCount > 0)
                {
                    isAllEmpty = false;
                }
            }

            if (isAllEmpty)
            {
                foreach (Transform shapeContainer in ShapeContainers)
                {
                    AddRandomShapeToContainer(shapeContainer);
                }
            }
        }
        else
        {
            foreach (Transform shapeContainer in ShapeContainers)
            {
                if (shapeContainer.childCount <= 0)
                {
                    AddRandomShapeToContainer(shapeContainer);
                }
            }
        }

        Invoke("CheckOnBoardShapeStatus", 0.2F);
    }

    public void ResetShapeContainer()
    {
        foreach (var shapeContainer in ShapeContainers)
        {
            DestroyAllChildren(shapeContainer);
        }
        
        StartCoroutine(CoAddRandomShapeToContainer());
    }

    IEnumerator CoAddRandomShapeToContainer()
    {
        yield return null;
        foreach (Transform shapeContainer in ShapeContainers)
        {
            AddRandomShapeToContainer(shapeContainer);
        }
        Invoke("CheckOnBoardShapeStatus", 0.2F);
    }

    /// <summary>
    /// Adds the random shape to container.
    /// </summary>
    /// <param name="shapeContainer">Shape container.</param>
    public void AddRandomShapeToContainer(Transform shapeContainer)
    {
        if (shapeBlockProbabilityPool == null || shapeBlockProbabilityPool.Count <= 0)
        {
            CreateShapeBlockProbabilityList();
        }

        int RandomShape = shapeBlockProbabilityPool[0];
        shapeBlockProbabilityPool.RemoveAt(0);

        GameObject newShapeBlock = activePShapeBlockModule.ShapeBlocks.Find(o => o.BlockID == RandomShape).shapeBlock;
        GameObject spawningShapeBlock = (GameObject)(Instantiate(newShapeBlock)) as GameObject;
        spawningShapeBlock.transform.SetParent(shapeContainer);
        spawningShapeBlock.transform.localScale = Vector3.one * 0.6F;
        spawningShapeBlock.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(800F, 0, 0);
        spawningShapeBlock.transform.DOLocalMove(Vector3.zero, 0.3F);
        spawningShapeBlock.transform.DOLocalMove(Vector3.zero, 0.3F);
    }

    /// <summary>
    /// Creates the shape with I.
    /// </summary>
    /// <param name="shapeContainer">Shape container.</param>
    /// <param name="shapeID">Shape I.</param>
    void CreateShapeWithID(Transform shapeContainer, int shapeID)
    {
        GameObject newShapeBlock = activePShapeBlockModule.ShapeBlocks.Find(o => o.BlockID == shapeID).shapeBlock;
        GameObject spawningShapeBlock = (GameObject)(Instantiate(newShapeBlock)) as GameObject;
        spawningShapeBlock.transform.SetParent(shapeContainer);
        spawningShapeBlock.transform.localScale = Vector3.one * 0.6F;
        spawningShapeBlock.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(800F, 0, 0);
        spawningShapeBlock.transform.DOLocalMove(Vector3.zero, 0.3F);

    }

    /// <summary>
    /// Checks the on board shape status.
    /// </summary>
    public void CheckOnBoardShapeStatus()
    {
        List<PShapeInfo> OnBoardBlockShapes = new List<PShapeInfo>();
        foreach (Transform shapeContainer in ShapeContainers)
        {
            if (shapeContainer.childCount > 0)
            {
                OnBoardBlockShapes.Add(shapeContainer.GetChild(0).GetComponent<PShapeInfo>());
            }
        }

        bool canExistingBlocksPlaced = PGamePlay.Instance.CanExistingBlocksPlaced(OnBoardBlockShapes);

        if (canExistingBlocksPlaced == false)
        {
            PGamePlay.Instance.OnUnableToPlaceShape();
        }
    }

    /// <summary>
    /// Reorders the shapes.
    /// </summary>
    void ReorderShapes()
    {
        List<Transform> EmptyShapes = new List<Transform>();

        foreach (Transform shapeContainer in ShapeContainers)
        {
            if (shapeContainer.childCount == 0)
            {
                EmptyShapes.Add(shapeContainer);
            }
            else
            {
                if (EmptyShapes.Count > 0)
                {
                    Transform emptyContainer = EmptyShapes[0];
                    shapeContainer.GetChild(0).SetParent(emptyContainer);
                    EmptyShapes.RemoveAt(0);

                    emptyContainer.GetChild(0).DOLocalMove(Vector3.zero, 0.3F);

                    EmptyShapes.Add(shapeContainer);
                }
            }
        }
    }

    /// <summary>
    /// Gets all on board shape names.
    /// </summary>
    /// <returns>The all on board shape names.</returns>
    public string GetAllOnBoardShapeNames()
    {
        string shapeNames = "";
        foreach (Transform shapeContainer in ShapeContainers)
        {
            if (shapeContainer.childCount > 0)
            {
                shapeNames = shapeNames + shapeContainer.GetChild(0).GetComponent<PShapeInfo>().ShapeID + ",";
            }
            else
            {
                shapeNames = shapeNames + "-1,";
            }
        }

        shapeNames = shapeNames.Remove(shapeNames.Length - 1);
        return shapeNames;
    }

    public void DestroyAllChildren(Transform parent)
    {
        // Loop through all child objects in reverse order
        // (Destroying objects from last to first is generally safer)
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
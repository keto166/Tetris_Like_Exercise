using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPooler : MonoBehaviour {

    public static BlockPooler instance = null;
    public int iniSize = 500;
    public GameObject blockGO;


    private BlockProperties myBlock;
    private List<BlockProperties> blockList;

    void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        blockList = new List<BlockProperties>(iniSize);

        for (int i = 0; i < iniSize; i++)
        {
            NewBlock();
        }
    }

    private BlockProperties NewBlock()
    {
        myBlock = Instantiate(blockGO).GetComponent<BlockProperties>();
        blockList.Add(myBlock);
        myBlock.ResetBlock();
        myBlock.transform.SetParent(transform);
        

        return myBlock;
    }

    public BlockProperties GetBlock()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].BlockType == BlockTypes.blank)
            {
                blockList[i].ResetBlock();
                blockList[i].BlockType = BlockTypes.sticky;
                
                return blockList[i];
            }
        }

        return NewBlock();

    }

}

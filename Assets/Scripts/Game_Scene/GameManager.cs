using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject boardManager;
    public bool gravity = true;
    

    private InputController inputController;
    
    private bool activePieces = false;

    private Vector3 tempPos = new Vector3();
    private Vector3 startPosition;

    public List<BlockProperties> activeBlocks;

    private Vector3 activeOrigin;
    private Vector3 relativeActiveOrigin;

    private List<BlockTemplate> blockTemplateList;
    

    public class BlockTemplate
    {
        public Vector3[] positions;
        public BlockTypes type;
        public Vector3 rotationOrigin;
        public BlockTemplate()
        {
            positions = new Vector3[4];
            type = BlockTypes.sticky;

        }
    }

    public void PrintThing(string s)
    {
        print(s);
    }

    void Start () {
		if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Instantiate(boardManager);

        activeBlocks = new List<BlockProperties>(4);
        for (int i = 0; i < 4; i++) {activeBlocks.Add(null);}
        Invoke("LaunchNewPieces", .25f);
        inputController = new InputController();

        SetupTemplates();
        //startPosition = new Vector3((BoardManager.instance.maxCols / 2), (BoardManager.instance.maxRows / 2), 0f);
        Invoke("Test", 0f);
        


    }

    private void Test()
    {
        startPosition = new Vector3((BoardManager.instance.maxCols / 2), (BoardManager.instance.maxRows / 2), 0f);
    }

    private void SetupTemplates()
    {
        
        blockTemplateList = new List<BlockTemplate>(7);
        BlockTemplate tempBT;



        //1
        tempBT = new BlockTemplate();
        tempBT.rotationOrigin = new Vector3(0.5f, 1.5f);
        tempBT.positions[0] = new Vector3(0f, 0f);
        tempBT.positions[1] = new Vector3(0f, 1f);
        tempBT.positions[2] = new Vector3(0f, 2f);
        tempBT.positions[3] = new Vector3(0f, 3f);
        blockTemplateList.Add(tempBT);

        //2
        tempBT = new BlockTemplate();
        tempBT.rotationOrigin = new Vector3(1f, 1f);
        tempBT.positions[0] = new Vector3(0f, 0f);
        tempBT.positions[1] = new Vector3(1f, 0f);
        tempBT.positions[2] = new Vector3(1f, 1f);
        tempBT.positions[3] = new Vector3(1f, 2f);
        blockTemplateList.Add(tempBT);
        //3
        tempBT = new BlockTemplate();
        tempBT.rotationOrigin = new Vector3(0f, 1f);
        tempBT.positions[0] = new Vector3(0f, 0f);
        tempBT.positions[1] = new Vector3(1f, 0f);
        tempBT.positions[2] = new Vector3(0f, 1f);
        tempBT.positions[3] = new Vector3(0f, 2f);
        blockTemplateList.Add(tempBT);
        //4
        tempBT = new BlockTemplate();
        tempBT.rotationOrigin = new Vector3(0.5f, 0.5f);
        tempBT.positions[0] = new Vector3(0f, 0f);
        tempBT.positions[1] = new Vector3(1f, 0f);
        tempBT.positions[2] = new Vector3(0f, 1f);
        tempBT.positions[3] = new Vector3(1f, 1f);
        blockTemplateList.Add(tempBT);
        //5
        tempBT = new BlockTemplate();
        tempBT.rotationOrigin = new Vector3(1f, 0f);
        tempBT.positions[0] = new Vector3(0f, 0f);
        tempBT.positions[1] = new Vector3(1f, 0f);
        tempBT.positions[2] = new Vector3(1f, 1f);
        tempBT.positions[3] = new Vector3(2f, 1f);
        blockTemplateList.Add(tempBT);
        //6
        tempBT = new BlockTemplate();
        tempBT.rotationOrigin = new Vector3(0f, 0f);
        tempBT.positions[0] = new Vector3(0f, 0f);
        tempBT.positions[1] = new Vector3(1f, 0f);
        tempBT.positions[2] = new Vector3(-1f, 0f);
        tempBT.positions[3] = new Vector3(0f, 1f);
        blockTemplateList.Add(tempBT);
        //7
        tempBT = new BlockTemplate();
        tempBT.rotationOrigin = new Vector3(0f, 0f);
        tempBT.positions[0] = new Vector3(0f, 0f);
        tempBT.positions[1] = new Vector3(1f, 0f);
        tempBT.positions[2] = new Vector3(0f, 1f);
        tempBT.positions[3] = new Vector3(-1f, 1f);
        blockTemplateList.Add(tempBT);


    }

    public void AttemptMove(int xdir, int ydir)
    {
        if (CanMove(xdir, ydir))
        { MovePieces(xdir, ydir); }
    }

    public void AttemptRotate(int rdir)
    {

        activeOrigin = activeBlocks[0].transform.rotation * relativeActiveOrigin + activeBlocks[0].transform.position;
        //print(activeOrigin.x.ToString() + "x, " + activeOrigin.y.ToString() + "y, " + activeOrigin.z.ToString() + "z. ");
        foreach (BlockProperties myThing in activeBlocks)
        {
            myThing.transform.RotateAround(activeOrigin, Vector3.forward, (float)rdir);
        }
    }
	
	void Update () {
        if (activePieces) { inputController.GetInput(); }
    }
    

    private bool CanMove(int xdir, int ydir)
    {
        BlockTypes notUsed;
        return CanMove(xdir, ydir, out notUsed);
    }
    private bool CanMove(int xdir, int ydir, out BlockTypes hitEdge)
    {
        bool pathClear = true;
        hitEdge = BlockTypes.blank;
        int x = 0;
        int y = 0;

        for (int i = 0; i < 4; i++)
        {
            x = (int)Math.Round(activeBlocks[i].transform.position.x) + xdir;
            y = (int)Math.Round(activeBlocks[i].transform.position.y) + ydir;
            
            if (BoardManager.instance.boardMap[x,y] != null)
            {
                pathClear = false;
                if (BoardManager.instance.boardMap[x, y].BlockType == BlockTypes.edge)
                { hitEdge = BlockTypes.edge; }

                if (hitEdge != BlockTypes.edge && BoardManager.instance.boardMap[x, y].BlockType != BlockTypes.blank)
                { hitEdge = BlockTypes.sticky; }
            }
        }
        return pathClear;
    }
    private void MovePieces(int xdir, int ydir)
    {
        for (int i = 0; i < 4; i++)
        {
            activeBlocks[i].transform.Translate(xdir, ydir, 0,Camera.main.transform);
        }
    }

    private void LaunchNewPieces()
    {
        
        gravity = true;
        int blockTemplateType = Random.Range(0, 7);
        for (int i = 0; i < 4; i++)
        {
            activeBlocks[i] = BlockPooler.instance.GetBlock();
            activeBlocks[i].BlockType = blockTemplateList[blockTemplateType].type;
            activeBlocks[i].transform.position = startPosition + blockTemplateList[blockTemplateType].positions[i];
        }
        relativeActiveOrigin = blockTemplateList[blockTemplateType].rotationOrigin;
        activePieces = true;
        InvokeRepeating("GravityMove", 1.0f, 0.5f);
    }

    private void GravityMove()
    {
        if (!activePieces) { return; }
        BlockTypes hitEdge;
        if (CanMove(0, gravity ? -1 : 1,out hitEdge))
        { MovePieces(0, gravity ? -1 : 1); }
        else if (hitEdge == BlockTypes.edge)
        {
            for (int i = 0; i < 4; i++)
            {
                activeBlocks[i].ResetBlock();
                activeBlocks[i] = null;
                
            }
            activePieces = false;
            Invoke("LaunchNewPieces", .25f);
            CancelInvoke("GravityMove");
        }
        else if (hitEdge == BlockTypes.sticky || hitEdge == BlockTypes.post)
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 4; i++)
            {

                x = (int)Math.Round(activeBlocks[i].transform.position.x);
                y = (int)Math.Round(activeBlocks[i].transform.position.y);
                BoardManager.instance.boardMap[x, y] = activeBlocks[i];
                activeBlocks[i] = null;

            }
            activePieces = false;
            Invoke("LaunchNewPieces", .25f);
            CancelInvoke("GravityMove");
        }

    }
}

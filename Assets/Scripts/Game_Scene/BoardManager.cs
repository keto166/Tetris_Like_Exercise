using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public static BoardManager instance = null;

    public GameObject blockPooler;

    public int maxCols = 50;
    public int maxRows = 25;

    public BlockProperties[,] boardMap;
    private Vector3 tempPos = new Vector3(0f,0f,0f);
    private List<BlockData> posts;
    private List<BlockData> edges;

    public struct BlockData {
        public int x;
        public int y;
        public BlockTypes type;
        public BlockData(int _x, int _y, BlockTypes _type) {
            x = _x;
            y = _y;
            type = _type;
        }
    }



    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        boardMap = new BlockProperties[maxCols, maxRows];

        GeneratePosts();
        GenerateEdges();

        SetupBoard();

    }

    private void GeneratePosts() {
        posts = new List<BlockData>(6);

        BlockData myBlockData;
        int y = 3;
        int x;
        for (x = 3; x < 6; x++)
        {

            myBlockData = new BlockData(x, y, BlockTypes.post);
            posts.Add(myBlockData);

        }

        x = maxCols - 3;
        for (y = maxRows - 3; y > maxRows - 6; y--)
        {

            myBlockData = new BlockData(x, y, BlockTypes.post);
            posts.Add(myBlockData);
        }


    }

    private void GenerateEdges()
    {
        edges = new List<BlockData>(2*(maxCols + maxRows) - 4);

        BlockData myBlockData;

        int x = 0;
        int y = 0;
        for (x = 0; x < maxCols; x++)
        {
            for (y = 0; y <= maxRows; y += maxRows - 1)
            {
                myBlockData = new BlockData(x, y, BlockTypes.edge);
                edges.Add(myBlockData);
            }
        }
        for (y = 1; y < maxRows - 1; y++)
        {
            for (x = 0; x <= maxCols; x += maxCols - 1)
            {
                myBlockData = new BlockData(x, y, BlockTypes.edge);
                edges.Add(myBlockData);
            }
        }
    }

    public void SetupBoard()
    {
        

        Camera.main.orthographicSize =  maxCols * (float)Screen.height / (float)Screen.width * 0.5f;
        Camera.main.transform.position = new Vector3(maxCols / 2, maxRows / 2, -10f);

        posts.ForEach(PrintFromBlockData);
        edges.ForEach(PrintFromBlockData);



    }

    private void PrintFromBlockData(BlockData myBlockData)
    {
        if (boardMap[myBlockData.x, myBlockData.y] == null)
        {
            boardMap[myBlockData.x, myBlockData.y] = BlockPooler.instance.GetBlock();
            tempPos.x = myBlockData.x;
            tempPos.y = myBlockData.y;
            tempPos.z = 0.0f;
            boardMap[myBlockData.x, myBlockData.y].transform.position = tempPos;
            boardMap[myBlockData.x, myBlockData.y].BlockType = myBlockData.type;
        } else
        {
            print("Error");
        }
    }


}

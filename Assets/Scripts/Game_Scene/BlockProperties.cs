using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockTypes
{
    sticky = 0,
    edge = 1,
    post = 2,
    blank = 3
}



public class BlockProperties : MonoBehaviour
{


    public static Vector3 stagingPos = new Vector3(-1, -1, 0);


    [SerializeField] private BlockTypes blockType;
    public BlockTypes BlockType
    {
        get
        {
            return blockType;
        }
        set
        {
            if (BlockTypes.IsDefined(typeof(BlockTypes), value)) {
                SpriteRenderer mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                
                switch (value)
                {
                    case BlockTypes.sticky:
                        mySpriteRenderer.color = Color.blue;
                        break;
                    case BlockTypes.edge:
                        mySpriteRenderer.color = Color.red;
                        break;
                    case BlockTypes.post:
                        mySpriteRenderer.color = Color.gray;
                        break;
                    case BlockTypes.blank:
                        mySpriteRenderer.color = Color.clear;
                        break;
                }
                blockType = value;
            }
        }
    }

    public void ResetBlock()
    {
        BlockType = BlockTypes.blank;
        gameObject.transform.position = stagingPos;
        gameObject.transform.rotation = Quaternion.identity;

    }


}

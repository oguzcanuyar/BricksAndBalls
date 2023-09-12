using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChild : MonoBehaviour
{
    public Block block;
    private void OnCollisionExit2D(Collision2D other)
    {
        block.HP--;
        if (block.HP == 0)
        {
            block._GM.Blocks.Remove(block);
            transform.parent.gameObject.SetActive(false);
            block.gameObject.SetActive(false);
            block._GM.pool.Enqueue(block);
        }
    }
}

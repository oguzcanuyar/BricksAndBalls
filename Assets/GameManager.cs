using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int level = 1;

    public float xStart, yStart, yEnd;
    public float xDiff, yDiff;
    public Block prefab;
    [HideInInspector]
    public List<Block> Blocks;

    public Queue<Block> pool;
    private void OnEnable()
    {
        pool = new Queue<Block>();
        Blocks = new List<Block>();
        for (int i = 0; i < 100; i++)
        {
            Block a = Instantiate(prefab.gameObject).GetComponent<Block>();
            a._GM = this;
            a.transform.parent=transform;
            a.gameObject.SetActive(false);
            pool.Enqueue(a);
        }
    }

    private void CreateLine()
    {
        int[] list = { 0, 1, 2, 3, 4, 5, 6 };

        shuffle(list);

        int amount = UnityEngine.Random.Range(1, 7);
        for (int i = 0; i < amount; i++)
        {
            float xPos = xStart + list[i] * xDiff;
            Block a = pool.Dequeue();
            Blocks.Add(a);
            a.transform.position = new Vector3(xPos, yStart+yDiff);
            a.HP = level;
            a.enable(UnityEngine.Random.Range(0,100)<20?1:0,level);
            a.gameObject.SetActive(true);
            StartCoroutine(smoothMove(a));
        }
        level++;
    }

    public void NextTurn()
    {
        StartCoroutine(PreventCreationForSeconds());
        foreach (var item in Blocks)
        {
            //item.transform.position += Vector3.down;
            StartCoroutine(smoothMove(item));
        }

        CreateLine();

    }
    public bool canMove=true;

    IEnumerator PreventCreationForSeconds(){
        float t=0;
        canMove=false;
        while(t<movementTime+.1f){
            t+=Time.deltaTime;
            yield return null;
        }
        canMove=true;
    }
    private float movementTime = 0.5f;
    IEnumerator smoothMove(Block o){
         Vector3 startingPos  = o.transform.position;
         //Vector3 finalPos = transform.position + (transform.forward * 5);
         Vector3 finalPos = o.transform.position + Vector3.down*yDiff;
         float elapsedTime = 0;
        
         while (elapsedTime < movementTime)
         {
             o.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / movementTime));
             elapsedTime += Time.deltaTime;
             yield return null;
         }
        o.transform.position=finalPos;
            //Block item = Blocks[i];
            if(o.transform.position.y < yEnd)
            {
            /* Blocks.Remove(o);
             o.gameObject.SetActive(false);
             pool.Enqueue(o);*/
                SceneManager.LoadScene(0);
                
            }
    }
    private void shuffle(int[] list)
    {
        for (int i = list.Length - 1; i > 0; i--)
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}

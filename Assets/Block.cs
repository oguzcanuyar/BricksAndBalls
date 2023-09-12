using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int HP;
    public GameManager _GM;
    public TextMeshProUGUI slevelText, tlevelText;
    public SpriteRenderer render;

    public List<Color> Colors;
    private void Update()
    {
        slevelText.text = HP + "";
        tlevelText.text = HP + "";

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        HP--;
        if (HP == 0)
        {
            _GM.Blocks.Remove(this);
            gameObject.SetActive(false);
            _GM.pool.Enqueue(this);
        }
    }
    public SpriteRenderer s, t;

    public void enable(int x, int c)
    {
        if (x == 0)
        {
            s.color = Colors[c % 5];
            s.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            t.color = Colors[c % 5];
            t.transform.parent.gameObject.SetActive(true);

        }
    }

}

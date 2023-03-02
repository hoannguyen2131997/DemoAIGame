using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 posCell;
    public int idCell;
    public int type; // 0 - empty, 1 - obstacle, 2 - player, 3 - enemy 

    public void SetCandySprite(Sprite candySprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = candySprite;
    }

    public void SetTypeCell(int type)
    {
        gameObject.GetComponent<Cell>().type = type;
    }
}

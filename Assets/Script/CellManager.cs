using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public Vector2 pos;
    public int idCell;
    
    public void AssignData(int id, Vector2 posCell)
    {
        idCell = id;
        pos = posCell;
    }
}
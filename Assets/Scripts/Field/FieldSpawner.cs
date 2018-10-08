using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class FieldSpawner : MonoBehaviour
{
    private const string FieldObjectName = "Field";

    [SerializeField]
    private Cell[] sampleCells;
    [SerializeField]
    private Vector2Int fieldLength;

    private void Start()
    {
        SpawnField();
        
        foreach (var cell in sampleCells)
        {
            Destroy(cell.gameObject);
        }

        Destroy(this.gameObject);
    }

    private void SpawnField()
    {
        var fieldObject = CreateFieldObject();
        var includedCells = new List<List<Cell>>();
        int pointedIndex = 0;

        for (int xPositionInField = 0; xPositionInField < fieldLength.x; ++xPositionInField)
        {
            var rowCells = new List<Cell>();
            for (int yPositionInField = 0; yPositionInField < fieldLength.y; ++yPositionInField)
            {
                var createdCell = Instantiate(sampleCells[pointedIndex], fieldObject.transform);
                
                createdCell.transform.position = CalculateCellPosition(fieldObject.transform.position, xPositionInField, yPositionInField);
                createdCell.gameObject.SetActive(true);

                rowCells.Add(createdCell);
                pointedIndex = (pointedIndex + 1) % sampleCells.Length;
            }

            includedCells.Add(rowCells);
        }

        fieldObject.SetIncludedCells(includedCells.Select(rowCells => rowCells as IEnumerable<Cell>));
    }

    private Field CreateFieldObject()
    {
        var fieldObject = new GameObject().AddComponent<Field>();
        fieldObject.name = FieldObjectName;
        fieldObject.transform.position = this.transform.position;

        return fieldObject;
    }

    private Vector3 CalculateCellPosition(Vector3 origin, int xPositionInField, int yPositionInField)
    {
        return new Vector3(origin.x + xPositionInField * sampleCells.First().Size.x,
                           origin.y,
                           origin.z + yPositionInField * sampleCells.First().Size.y);
    }
}

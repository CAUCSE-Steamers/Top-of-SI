using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    private IList<IList<Cell>> cellsInSquare;
    private Vector2Int size;

    public Cell GetCell(int x, int y)
    {
        try
        {
            return cellsInSquare[x][y];
        }
        catch (IndexOutOfRangeException)
        {
            DebugLogger.LogErrorFormat("Field::GetCell => Field 범위를 벗어난 Cell을 가져오려고 합니다. ({0}, {1}) ", x, y);
            throw new IndexOutOfRangeException("Field 범위를 벗어난 Cell을 가져오려고 합니다.");
        }
    }

    public void SetIncludedCells(IEnumerable<IEnumerable<Cell>> cellsInSquare)
    {
        if (cellsInSquare == null)
        {
            throw new NullReferenceException("세팅되는 Cell은 Null일 수 없습니다.");
        }

        if (cellsInSquare.Count() == 0)
        {
            throw new ArgumentException("주어진 Cell 리스트가 비어있습니다.");
        }

        if (cellsInSquare.First().Count() == 0)
        {
            throw new ArgumentException("첫 Cell Row는 비어있을 수 없습니다.");
        }
        
        SetSizeOfField(cellsInSquare);

        this.cellsInSquare = cellsInSquare.Select(rowCells => rowCells.ToList() as IList<Cell>)
                                          .ToList();

        RefreshCellPositions();
    }

    private void SetSizeOfField(IEnumerable<IEnumerable<Cell>> cellsInSquare)
    {
        size.x = cellsInSquare.Count();
        size.y = cellsInSquare.First().Count();
    }

    private void RefreshCellPositions()
    {
        for (int x = 0; x < size.x; ++x)
        {
            for (int y = 0; y < size.y; ++y)
            {
                cellsInSquare[x][y].PositionInField = new Vector2Int(x, y);
            }
        }
    }

    public IEnumerable<Cell> FetchObjectContainingCells()
    {
        return cellsInSquare.SelectMany(rowCells => rowCells)
                            .Where(cell => cell.HasObjectOnCell())
                            .Select(cell => cell);
    }

    public Vector2Int VectorToIndices(Vector3 position)
    {
        var cellSize = GetCell(0, 0).Size;
        double xRelatedPosition = (position.x - transform.position.x + 0.5 * cellSize.y);
        double yRelatedPosition = (position.z - transform.position.z + 0.5 * cellSize.x);


        int xIndex = (int) (xRelatedPosition / cellSize.y);
        int yIndex = (int) (yRelatedPosition / cellSize.x);

        if (xRelatedPosition < 0.0 || xIndex >= size.x)
        {
            throw new ArgumentException("주어진 X 좌표가 필드 범위를 벗어났습니다.");
        }

        if (yRelatedPosition < 0.0 || yIndex >= size.y)
        {
            throw new ArgumentException("주어진 Y 좌표가 필드 범위를 벗어났습니다.");
        }

        return new Vector2Int(xIndex, yIndex);
    }

    public Vector3 IndicesTransformToVector(Vector2Int deltaIndices)
    {
        var cellSize = GetCell(0, 0).Size;

        return new Vector3(deltaIndices.y * cellSize.y, 0, deltaIndices.x * cellSize.x);
    }
}

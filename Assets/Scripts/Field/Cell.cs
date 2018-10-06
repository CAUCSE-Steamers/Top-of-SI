using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size;
    [SerializeField]
    private int layerMaskForCheckingObject;

    public Vector2Int Size
    {
        get
        {
            return size;
        }
    }

    public Vector2Int PositionInField
    {
        get; set;
    }

    public bool HasObjectOnCell()
    {
        int programmerOnlyMask = (1 << layerMaskForCheckingObject);

        var detectedProgrammers =
            Physics.BoxCastAll(center: transform.position,
                               halfExtents: new Vector3(Size.x / 2.0f, 3f, Size.y / 2.0f),
                               direction: Vector3.up,
                               maxDistance: 10f,
                               orientation: Quaternion.identity,
                               layermask: programmerOnlyMask);

        return detectedProgrammers.Length > 0;
    }
}

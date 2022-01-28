using UnityEngine;
using UnityEngine.UI;

public class UIGradientScrollbar: UIGradient
{
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        var pivot = rectTransform.pivot;
        var rect = rectTransform.rect;

        var corner1 = new Vector2(-pivot.x * rect.width, -pivot.y * rect.height);
        var corner2 = new Vector2((1 - pivot.x) * rect.width, (1 - pivot.y) * rect.height);

        vh.Clear();
        
        var vert = UIVertex.simpleVert;
        
        AddVertex(ref vert, ref vh, corner1.x, corner1.y,gradient, 1);
        AddVertex(ref vert, ref vh, corner1.x, corner2.y, gradient, 1);
        AddVertex(ref vert, ref vh, corner2.x, corner2.y, gradient, 0);
        AddVertex(ref vert, ref vh, corner2.x, corner1.y,gradient, 0);
        
        vh.AddTriangle(0,1,2);
        vh.AddTriangle(2,3,0);
    }
}
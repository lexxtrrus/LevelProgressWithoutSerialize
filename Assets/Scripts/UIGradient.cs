using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode] [RequireComponent(typeof(CanvasRenderer))]
public class UIGradient : MaskableGraphic
{
    protected enum GradientType
    {
        LeftToRight,
        RightToLeft,
        Upwards,
        TopDown,
    }

    [SerializeField] protected GradientType type;
    [SerializeField] protected Gradient gradient = new Gradient();

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        var pivot = rectTransform.pivot;
        var rect = rectTransform.rect;

        var corner1 = new Vector2(-pivot.x * rect.width, -pivot.y * rect.height);
        var corner2 = new Vector2((1 - pivot.x) * rect.width, (1 - pivot.y) * rect.height);

        vh.Clear();
        
        var vert = UIVertex.simpleVert;
        
        AddVertex(ref vert, ref vh, corner1.x, corner1.y,gradient, 0);
        AddVertex(ref vert, ref vh, corner1.x, corner2.y, gradient, 1);
        AddVertex(ref vert, ref vh, corner2.x, corner2.y, gradient, 1);
        AddVertex(ref vert, ref vh, corner2.x, corner1.y,gradient, 0);
        
        vh.AddTriangle(0,1,2);
        vh.AddTriangle(2,3,0);
    }

    protected virtual void AddVertex(ref UIVertex vertex, ref VertexHelper vertexHelper, 
        float x, float y, Gradient gradient, int key)
    {
        vertex.position = new Vector3(x,y, 0f);
        vertex.color = gradient.colorKeys[key].color;
        vertex.color.a = (byte)(gradient.alphaKeys[key].alpha * 255f);
        vertexHelper.AddVert(vertex);
    }
    
}

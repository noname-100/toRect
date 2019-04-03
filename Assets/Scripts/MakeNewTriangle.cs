using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeNewTriangle : MonoBehaviour
{
    private PolygonCollider2D _polygonCollider2D;
    private void Start()
    {
        // Create Vector2 vertices
        var vertices2D = new Vector2[] {
            new Vector2(0,0),
            new Vector2(3,12),
            new Vector2(7,7)
        };
        gameObject.AddComponent(System.Type.GetType("Drag"));
        _polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);
        _polygonCollider2D.points = vertices2D;

        // Use the triangulator to get indices for creating triangles
        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        // Generate a color for each vertex
        var colors = Enumerable.Range(0, vertices3D.Length)
            .Select(i => Random.ColorHSV())
            .ToArray();

        // Create the mesh
        var mesh = new Mesh
        {
            vertices = vertices3D,
            triangles = indices,
            colors = colors
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set up game object with mesh;
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
    }
}
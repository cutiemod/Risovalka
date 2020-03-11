using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    internal Material mat;

    internal int partNum;

    private LineRenderer line;
    private Vector2 mousePosition;
    [SerializeField] private bool simplifyLine = false;
    [SerializeField] private float simplifyTolerance = 0.02f;

    private void Start()
    {
        line = GetComponent<LineRenderer>();

        Material newMat = new Material(mat.shader); 
        newMat.SetInt("_StencilNum", partNum);
        newMat.renderQueue += partNum - 50;
        newMat.SetColor("_Color", mat.color);
        line.material = newMat;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (simplifyLine)
            {
                line.Simplify(simplifyTolerance);
            }

            BakeIntoMesh(gameObject);

            enabled = false; 
        }
    }

    private void BakeIntoMesh(GameObject lineObj)
    {
        var lineRenderer = lineObj.GetComponent<LineRenderer>();
        var meshFilter = lineObj.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;

        var meshRenderer = lineObj.AddComponent<MeshRenderer>();

        meshRenderer.material = line.material;

        Vector3 newPosition = lineObj.transform.InverseTransformPoint(lineObj.transform.position); 
        newPosition.z = lineObj.transform.position.z;
        meshRenderer.sortingOrder = lineRenderer.sortingOrder;
        lineObj.transform.position = newPosition;

        GameObject.Destroy(lineRenderer);
    }
}
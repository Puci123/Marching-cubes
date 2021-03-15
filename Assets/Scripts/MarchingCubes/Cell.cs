using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vertex[] maiVerecies = new Vertex[8];
    public Vertex[] verticesInresectons = new Vertex[12];
    public Color cellColor = Color.cyan;

    private float isoValue;

    public Cell(Vertex[] vertices, float _isoValue)
    {

        isoValue = _isoValue;


        for (int i = 0; i < vertices.Length; i++)
        {
            maiVerecies[i] = vertices[i];
        }

    }

    public List<Vector3> GetVertxPos()
    {
        List<Vector3> temp = new List<Vector3>();

        foreach (Vertex vert in GetVertsIntersetion())
        {
            if (vert != null)
                temp.Add(vert.WordPos);
            else
                Debug.LogError("Mising vertcies");
        }
        return temp;
    }

    public int GetCombination()
    {
        int temp = 0;

        if (maiVerecies[0].VertexValue < isoValue) temp |= 1;
        if (maiVerecies[1].VertexValue < isoValue) temp |= 2;
        if (maiVerecies[2].VertexValue < isoValue) temp |= 4;
        if (maiVerecies[3].VertexValue < isoValue) temp |= 8;
        if (maiVerecies[4].VertexValue < isoValue) temp |= 16;
        if (maiVerecies[5].VertexValue < isoValue) temp |= 32;
        if (maiVerecies[6].VertexValue < isoValue) temp |= 64;
        if (maiVerecies[7].VertexValue < isoValue) temp |= 128;
        
        return temp;
    }

    public Vertex PointOnEdge(Vertex a, Vertex b)
    {
        Vertex vert = new Vertex(Interpolate(a,b),1,isoValue);
        return vert;
    }

    public Vertex[] GetVertsIntersetion()
    {
 
        Vertex[] temp = new Vertex[12];
        int Combination = GetCombination();

        //Bot sqer
        if ((TriangulationTable.edgeTable[Combination] & 1) != 0)
            temp[0] = (PointOnEdge(maiVerecies[0], maiVerecies[1]));
        if ((TriangulationTable.edgeTable[Combination] & 2) != 0)
            temp[1] = (PointOnEdge(maiVerecies[1], maiVerecies[2]));
        if ((TriangulationTable.edgeTable[Combination] & 4) != 0)
            temp[2] = (PointOnEdge(maiVerecies[2], maiVerecies[3]));
        if ((TriangulationTable.edgeTable[Combination] & 8) != 0)
            temp[3] = (PointOnEdge(maiVerecies[3], maiVerecies[0]));

        //Top sqer
        if ((TriangulationTable.edgeTable[Combination] & 16) != 0)
            temp[4] = (PointOnEdge(maiVerecies[4], maiVerecies[5]));
        if ((TriangulationTable.edgeTable[Combination] & 32) != 0)
            temp[5] = (PointOnEdge(maiVerecies[5], maiVerecies[6]));
        if ((TriangulationTable.edgeTable[Combination] & 64) != 0)
            temp[6] = (PointOnEdge(maiVerecies[6], maiVerecies[7]));
        if ((TriangulationTable.edgeTable[Combination] & 128) != 0)
            temp[7] = (PointOnEdge(maiVerecies[7], maiVerecies[4]));

        //sides
        if ((TriangulationTable.edgeTable[Combination] & 256) != 0)
            temp[8] = (PointOnEdge(maiVerecies[0], maiVerecies[4]));
        if ((TriangulationTable.edgeTable[Combination] & 512) != 0)
            temp[9] = (PointOnEdge(maiVerecies[1], maiVerecies[5]));
        if ((TriangulationTable.edgeTable[Combination] & 1024) != 0)
            temp[10] = (PointOnEdge(maiVerecies[2], maiVerecies[6]));
        if ((TriangulationTable.edgeTable[Combination] & 2048) != 0)
            temp[11] = (PointOnEdge(maiVerecies[3], maiVerecies[7]));



        return temp;
    }

    public List<Triangl> GetTriangle()
    {
        List<Triangl> triangles = new List<Triangl>();
        int combination = GetCombination();
        Vertex[] intersection = GetVertsIntersetion();

        for (int i = 0; TriangulationTable.triTable[combination, i] != -1; i += 3)
        {
            Vertex a = intersection[(TriangulationTable.triTable[combination, i])];
            Vertex b = intersection[(TriangulationTable.triTable[combination, i + 1])];
            Vertex c = intersection[(TriangulationTable.triTable[combination, i + 2])];

            triangles.Add(new Triangl(a.WordPos, b.WordPos, c.WordPos));
        }

        return triangles;
    }
    
    public Vector3 Interpolate(Vertex a, Vertex b)
    {
        if (TereinGenaratorMenager.UseLineralInterplation)
        {

            if (Mathf.Abs(isoValue - a.VertexValue)      < 0.001f) return a.WordPos;
            if (Mathf.Abs(isoValue - b.VertexValue)      < 0.001f) return b.WordPos;
            if (Mathf.Abs(a.VertexValue - b.VertexValue) < 0.001f) return a.WordPos;


            float mu = (isoValue - a.VertexValue) / (b.VertexValue - a.VertexValue);
            Vector3 p = Vector3.zero;

            p.x = a.WordPos.x + mu * (b.WordPos.x - a.WordPos.x);
            p.y = a.WordPos.y + mu * (b.WordPos.y - a.WordPos.y);
            p.z = a.WordPos.z + mu * (b.WordPos.z - a.WordPos.z);


            return p;
        }
        else
        {
            return ((a.WordPos + b.WordPos) / 2);
        }
    }

    public struct Triangl
    {
        private Vector3 a,b,c;

        public Vector3 A {get  { return a; }}
        public Vector3 B { get { return b; } }
        public Vector3 C { get { return c; } }


        public Triangl(Vector3 _a, Vector3 _b,Vector3 _c)
        {
            a = _a;
            b = _b;
            c = _c;
        }
    }

}

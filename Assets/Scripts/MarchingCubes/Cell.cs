using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vertex[] maiVerecies = new Vertex[8];
    public Vertex[] verticesInresectons = new Vertex[12];
    public Color cellColor = Color.cyan;

    private float isoValue;

    public  Cell(Vertex[] vertices,float _isoValue)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            maiVerecies[i] = vertices[i];
        }

        //botom sqer
        verticesInresectons[0] = new Vertex(Interpolate(maiVerecies[0], maiVerecies[1]), isoValue);
        verticesInresectons[1] = new Vertex(Interpolate(maiVerecies[1], maiVerecies[2]), isoValue);
        verticesInresectons[2] = new Vertex(Interpolate(maiVerecies[2], maiVerecies[3]), isoValue);
        verticesInresectons[3] = new Vertex(Interpolate(maiVerecies[3], maiVerecies[0]), isoValue);
        //top sqer
        verticesInresectons[4] = new Vertex(Interpolate(maiVerecies[4], maiVerecies[5]), isoValue);
        verticesInresectons[5] = new Vertex(Interpolate(maiVerecies[5], maiVerecies[6]), isoValue);
        verticesInresectons[6] = new Vertex(Interpolate(maiVerecies[6], maiVerecies[7]), isoValue);
        verticesInresectons[7] = new Vertex(Interpolate(maiVerecies[7], maiVerecies[4]), isoValue);
        //side edge
        verticesInresectons[8] = new Vertex(Interpolate(maiVerecies[0], maiVerecies[4]), isoValue);
        verticesInresectons[9] = new Vertex(Interpolate(maiVerecies[1], maiVerecies[5]), isoValue);
        verticesInresectons[10] = new Vertex(Interpolate(maiVerecies[2], maiVerecies[6]), isoValue);
        verticesInresectons[11] = new Vertex(Interpolate(maiVerecies[3], maiVerecies[7]), isoValue);
    }

    public List<Vector3> GetVertxPos()
    {
        List<Vector3> temp = new List<Vector3>();
    
        foreach (Vertex vert in verticesInresectons)
        {
            temp.Add(vert.WordPos);
        }
        return temp;
    }

    public int GetCombination()
    {
        int temp = 0;

        if (maiVerecies[0].Active) temp |= 1;
        if (maiVerecies[1].Active) temp |= 2;
        if (maiVerecies[2].Active) temp |= 4;
        if (maiVerecies[3].Active) temp |= 8;
        if (maiVerecies[4].Active) temp |= 16;
        if (maiVerecies[5].Active) temp |= 32;
        if (maiVerecies[6].Active) temp |= 64;
        if (maiVerecies[7].Active) temp |= 128;
        
        return temp;
    }

    public List<Vertex> GetVertsIntersetion(int Combination)
    {
 
        List<Vertex> temp = new List<Vertex>();


        if ((TriangulationTable.edgeTable[Combination] & 1) != 0)
            temp.Add(verticesInresectons[0]);
        if ((TriangulationTable.edgeTable[Combination] & 2) != 0)
            temp.Add(verticesInresectons[1]);
        if ((TriangulationTable.edgeTable[Combination] & 4) != 0)
            temp.Add(verticesInresectons[2]);
        if ((TriangulationTable.edgeTable[Combination] & 8) != 0)
            temp.Add(verticesInresectons[3]);
        if ((TriangulationTable.edgeTable[Combination] & 16) != 0)
            temp.Add(verticesInresectons[4]);
        if ((TriangulationTable.edgeTable[Combination] & 32) != 0)
            temp.Add(verticesInresectons[5]);
        if ((TriangulationTable.edgeTable[Combination] & 64) != 0)
            temp.Add(verticesInresectons[6]);
        if ((TriangulationTable.edgeTable[Combination] & 128) != 0)
            temp.Add(verticesInresectons[7]);
        if ((TriangulationTable.edgeTable[Combination] & 256) != 0)
            temp.Add(verticesInresectons[8]);
        if ((TriangulationTable.edgeTable[Combination] & 512) != 0)
            temp.Add(verticesInresectons[9]);
        if ((TriangulationTable.edgeTable[Combination] & 1024) != 0)
            temp.Add(verticesInresectons[10]);
        if ((TriangulationTable.edgeTable[Combination] & 2048) != 0)
            temp.Add(verticesInresectons[11]);


        return temp;
    }
    
    public List<int> GetVertexOrder(int combination)
    {
        List<int> temp = new List<int>();

        for (int i = 0; TriangulationTable.triTable[combination,i] != -1; i += 3)
        {
            temp.Add(TriangulationTable.triTable[combination, i]);
            temp.Add(TriangulationTable.triTable[combination, i + 1]);
            temp.Add(TriangulationTable.triTable[combination, i + 2]);


        }

        return temp;

    }
 

    public Vector3 Interpolate(Vertex a, Vertex b)
    {
       return (a.WordPos + b.WordPos) / 2;
    }



    public Vector3 Center()
    {
        Vector3 temp = Vector3.zero;
       

        foreach (Vertex pos in new List<Vertex>(maiVerecies))
        {
            temp += pos.WordPos;
        }

        return temp / 8;
    }
}

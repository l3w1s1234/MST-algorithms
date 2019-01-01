using System;


public class Vertex<T>
{
    //stores the data of the vertex
    private T m_data;
    private LinkedList<Vertex<T>> m_neighbours;

    public Vertex()
    {

    }

    //adds a node as a connected neighbour
    public void addNeighbour(Vertex<T> vertex)
    {
        m_neighbours.add(vertex);
    }
}

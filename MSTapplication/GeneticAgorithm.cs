using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simpleGraph;

namespace GA
{
    class GeneticAgorithm
    {
        
        private List<Vertex>chromosome;


        public GeneticAgorithm(int k)
        {
            

        }

        public void run(Graph graph)
        {
            
        }

        public void crossover()
        {

        }

        public void mutation()
        {

        }

        public void calcFitness()
        {

        }
    }

    class individual
    {
        private List<Vertex> genes;
        private List<Edge> chromosome;

        private int geneLength;

        private float fitness;


        public individual(List<Vertex> vertices)
        {
            genes = vertices;

            geneLength = genes.Count;

            genChromosome();
        }

        //creates the chromosome by selecting random edges from each vertex
        public void genChromosome()
        {
            foreach(Vertex v in genes)
            {
                //get a random index to select a random edge
                var rnd = new Random();
                var index = rnd.Next(0, v.neighbours.Count);

                chromosome.Add(v.neighbours.ElementAt(index));

            }

        }

        //returns the fitness of inidividual
        public float getFitness()
        {
            return fitness;
        }
            
        //calculate the fitness
        public void calcFitness()
        {
            foreach(Edge e in chromosome)
            {
                
            }
        }
    }
        
    class population
    {

    }
}

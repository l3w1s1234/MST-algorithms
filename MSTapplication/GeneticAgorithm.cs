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

        private Population population;
        public int genCount;

        private Individual fittest;
        private Individual secondFittest;

        public GeneticAgorithm()
        {
            population = new Population();

        }

        //run algorithm
        public Graph run(int k, ref Graph graph, int maxGen)
        {
            population.init(k, ref graph);
            Graph mst = new Graph();
            Random rnd = new Random();

            //perform algorithm while fitness > target weight
            while (genCount < maxGen)
            {
                genCount++;

                //perform selection
                selection();

                //do crossover
                crossover();

            }


            return mst;
        }

        //get two best individuals
        private void selection()
        {
            fittest = population.getFittest();

            secondFittest = population.getSecondFittest();
        }

        //perform crossover
        private void crossover()
        {
            Random rnd = new Random();
            int crossOverPoint = rnd.Next(0, population.individuals.Length);
        }
        
        private void mutation()
        {

        }
        
    }

    class Individual
    {
        private List<Vertex> genes;
        private List<Edge> chromosome;

        private int geneLength;

        private float fitness;


        public Individual(List<Vertex> vertices)
        {
            genes = vertices;

            geneLength = genes.Count;

            //mmake the chromosome
            genChromosome();

            //calculate the fitness
            calcFitness();
        }

        //creates the chromosome by selecting random edges from each vertex
        public void genChromosome()
        {
            foreach(Vertex v in genes)
            {
                //get a random index to select a random edge
                var rnd = new Random();
                var index = rnd.Next(0, v.neighbours.Count);

                //add selected edge to chromosome
                chromosome.Add(v.neighbours.ElementAt(index));

            }

        }

        //returns the fitness of individual
        public float getFitness()
        {
            return fitness;
        }
            
        //calculate the fitness
        public void calcFitness()
        {
            fitness = 0;

            Graph temp = null;

            foreach(Edge e in chromosome)
            {
                //add to temporary graph to check if edges already exist
                if (temp == null)
                {
                    temp.addNode(e.node1);
                    temp.addNode(e.node2);
                }
                if (!temp.hasVertex(e.node1)) temp.addNode(e.node1);
                if (!temp.hasVertex(e.node2)) temp.addNode(e.node2);

                //add edge to temp graph for comparisons
                if (!temp.hasEdge(e)) temp.addEdge(e.weight, e.node1, e.node2, e.data);

                //if the edge already exists hinder graph with fitness
                if (temp.hasEdge(e)) fitness += 1000;

                //add the weight to fitness
                fitness += e.weight;
            }
        }
    }
        
    class Population
    {
        public Individual[] individuals;


        public Population()
        {
           
        }

        public void init(int popSize, ref Graph g)
        {
            individuals = new Individual[popSize];

            //Initialize population
            for (int i = 0; i < individuals.Length; i++)
                individuals[i] = new Individual(g.GetVertices());
        }

        //return the fittest individual
        public Individual getFittest()
        {
            float fittest = 0;
            int index = 0;

            for (int i = 0;i<individuals.Length; i++)
            {
                if (fittest == 0) fittest = individuals[i].getFitness();
                else if (fittest < individuals[i].getFitness())
                {
                    fittest = individuals[i].getFitness();
                    index = i;
                }

            }

            return individuals[index];
        }


        //return the second fittest individual
        public Individual getSecondFittest()
        {
            float fittest = getFittest().getFitness();
            float secondFit = 0;
            int index = 0;

            for (int i = 0; i < individuals.Length; i++)
            {
                if (secondFit == 0 && individuals[i].getFitness() != fittest)
                {
                    secondFit = individuals[i].getFitness();
                }
                else if (secondFit < individuals[i].getFitness() && individuals[i].getFitness() != fittest)
                {
                    secondFit = individuals[i].getFitness();
                    index = i;
                }
                

            }

            return individuals[index];
        }

        //get the least fittest individual
        public Individual getLeastFittest()
        {
            float weakest = 0;
            int index = 0;

            for (int i = 0; i < individuals.Length; i++)
            {
                if (weakest == 0) weakest = individuals[i].getFitness();
                else if (weakest > individuals[i].getFitness())
                {
                    weakest = individuals[i].getFitness();
                    index = i;
                }

            }

            return individuals[index];
        }
    }
}

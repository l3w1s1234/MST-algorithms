using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simpleGraph;

namespace GA
{

   
    class GeneticAlgorithm
    {

        private Population population;
        public int genCount;

        private Individual fittest;
        private Individual strongest;
        private Individual secondFittest;
        private Random rnd = new Random();

        Graph temp;
        public GeneticAlgorithm()
        {
            population = new Population();

        }

        //run algorithm
        public Graph run(int k, ref Graph graph,int iterations)
        {
            temp = graph;
            

            float graphMaxFitness = graph.getGraphWeight();
            population.init(k, ref graph, graphMaxFitness,rnd);
            Graph mst = null;
            
            float minFit = population.getFittest().getFitness();

            //perform algorithm while fitness > target weight
            while (genCount < iterations)
            {
                genCount++;   
                //perform selection
                selection();


                int lestFitI = population.getLeastFittestIndex();
                

                //do crossover
                crossover();


                //5% chance to mutate
                if (rnd.Next(0,100) < 5)
                {
                    mutation();
                }

                

                //add the fittest offspring to population
                addFittestOffspring();


                population.calculateFitness();

                

                
            }

            //build mst
            foreach(Edge e in population.getFittest().chromosome)
            {
                if (mst == null)
                {
                    mst = new Graph();
                    mst.addNode(e.node1);
                    mst.addNode(e.node2);

                    mst.addEdge(e.weight, e.node1, e.node2, e.data);
                }
                else if(!mst.hasVertex(e.node1))
                {
                    mst.addNode(e.node1);
                    if(!mst.hasVertex(e.node2)) mst.addNode(e.node2); 
                    mst.addEdge(e.weight, e.node1, e.node2, e.data);
                }
                else if (!mst.hasVertex(e.node2))
                {
                    mst.addNode(e.node2);
                    if (!mst.hasVertex(e.node1)) mst.addNode(e.node1);
                    mst.addEdge(e.weight, e.node1, e.node2, e.data);
                }

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
          
            int crossOverPoint = rnd.Next(0, population.individuals[0].chromosome.Count);

            //Swap values among parents
            for (int i = 0; i < crossOverPoint; i++)
            {
                Edge temp = fittest.chromosome.ElementAt(i);
                fittest.chromosome[i] = secondFittest.chromosome.ElementAt(i);
                secondFittest.chromosome[i] = temp;

            }
        }
        
        //mutates an individual
        private void mutation()
        {
            Random rnd = new Random();
            


            //Select a random mutation point
            int mutationPoint = rnd.Next(population.individuals[0].chromosome.Count);

            //see if there are more edges to mutate
            if (temp.GetVertex(fittest.chromosome[mutationPoint].node1).neighbours.Count > 1)
            {
                foreach(Edge e in temp.GetVertex(fittest.chromosome[mutationPoint].node1).neighbours)
                {
                    if (fittest.chromosome[mutationPoint].data != e.data) fittest.chromosome[mutationPoint] = e;
                }
                
            }
            else if (temp.GetVertex(fittest.chromosome[mutationPoint].node2).neighbours.Count > 1)
            {
                foreach (Edge e in temp.GetVertex(fittest.chromosome[mutationPoint].node2).neighbours)
                {
                    if (fittest.chromosome[mutationPoint].data != e.data) fittest.chromosome[mutationPoint] = e;
                }
            }

            mutationPoint = rnd.Next(population.individuals[0].chromosome.Count);

            //see if there are more edges to mutate
            if (temp.GetVertex(secondFittest.chromosome[mutationPoint].node1).neighbours.Count > 1)
            {
                foreach (Edge e in temp.GetVertex(secondFittest.chromosome[mutationPoint].node1).neighbours)
                {
                    if (secondFittest.chromosome[mutationPoint].data != e.data) secondFittest.chromosome[mutationPoint] = e;
                }

            }
            else if (temp.GetVertex(fittest.chromosome[mutationPoint].node2).neighbours.Count > 1)
            {
                foreach (Edge e in temp.GetVertex(secondFittest.chromosome[mutationPoint].node2).neighbours)
                {
                    if (secondFittest.chromosome[mutationPoint].data != e.data) secondFittest.chromosome[mutationPoint] = e;
                }
            }
        }

        //Get fittest offspring
        Individual getFittestOffspring()
        {
            if (fittest.getFitness() > secondFittest.getFitness())
            {
                return fittest;
            }
            return secondFittest;
        }


        //Replace least fittest individual from most fittest offspring
        void addFittestOffspring()
        {

            //Update fitness values of offspring
            fittest.calcFitness();
            secondFittest.calcFitness();

            //Get index of least fit individual
            int leastFittestIndex = population.getLeastFittestIndex();

            //Replace least fittest individual from most fittest offspring
            population.individuals[leastFittestIndex] = getFittestOffspring();
        }

    }

    class Individual
    {
        public float MaxFitness = 100;
        public List<Vertex> genes;
        public List<Edge> chromosome = new List<Edge>();
        private int geneLength;

        private float fitness;

        private Random rnd;

        public Individual(List<Vertex> vertices, Random rand)
        {
            rnd = rand;

            genes = vertices;

            geneLength = genes.Count;

            //mmake the chromosome
            genChromosome();

            
        }

        //creates the chromosome by selecting random edges from each vertex
        public void genChromosome()
        {

            foreach (Vertex v in genes)
            {
                //get a random index to select a random edge
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
            bool actualtree = true;
            Graph temp = null;

            foreach(Edge e in chromosome)
            {
                //add to temporary graph to check if edges already exist
                if (temp == null)
                {
                    temp = new Graph();
                    temp.addNode(e.node1);
                    temp.addNode(e.node2);
                }
                if (!temp.hasVertex(e.node1)) temp.addNode(e.node1);
                if (!temp.hasVertex(e.node2)) temp.addNode(e.node2);

                //add edge to temp graph for comparisons
                if (temp.hasEdge(e)) { fitness += MaxFitness; actualtree = false; }
                if (!temp.hasEdge(e)) temp.addEdge(e.weight, e.node1, e.node2, e.data);
                

                //add the weight to fitness
                fitness += e.weight;
            }

            if(actualtree == true)
            {
                fitness -= MaxFitness/2;
            }
        }
    }
        
    class Population
    {
        public Individual[] individuals;

        public Population()
        {
           
        }

        public void init(int popSize, ref Graph g,float maxFitness, Random rand)
        {
            individuals = new Individual[popSize];
            //Initialize population
            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i] = new Individual(g.GetVertices(),rand);
                individuals[i].MaxFitness = maxFitness;
                individuals[i].calcFitness();
                
            }
                
        }

        //return the fittest individual
        public Individual getFittest()
        {
            float fittest = 0;
            int index = 0;

            for (int i = 0;i<individuals.Length; i++)
            {
                if (fittest == 0) fittest = individuals[i].getFitness();
                else if (fittest > individuals[i].getFitness())
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
                else if (secondFit > individuals[i].getFitness() && individuals[i].getFitness() != fittest)
                {
                    secondFit = individuals[i].getFitness();
                    index = i;
                }
                

            }

            return individuals[index];
        }

        //get the least fittest individual
        public int getLeastFittestIndex()
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

            return index;
        }

        //Calculate fitness of each individual
        public void calculateFitness()
        {

            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i].calcFitness();
            }
            
        }

    }
}

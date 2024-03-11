
using System;
using System.Collections.Generic;

namespace Snake_AI_2._0
{
    public class NeuralNetwork
    {
        
        public int[] layers;
        public float[][] neurons;
        public float[][][] weights;
        private float fitness;

        private static Random rand = new Random();



        public NeuralNetwork(int[] layers)
        {
            this.layers = new int[layers.Length];
            for (int i = 0; i < layers.Length; i++)
                this.layers[i] = layers[i];

            InitNeurons();
            InitWeights();

            
        }

        public NeuralNetwork(NeuralNetwork copyNetwork)
        {
            this.layers = new int[copyNetwork.layers.Length];
            for (int i = 0; i < copyNetwork.layers.Length; i++)
            {
                this.layers[i] = copyNetwork.layers[i];
            }

            InitNeurons();
            InitWeights();
            CopyWeights(copyNetwork.weights);
        }

        

        private void CopyWeights(float[][][] copyWeights)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = copyWeights[i][j][k];
                    }
                }
            }
        }

        private void InitNeurons()
        {
            List<float[]> neuronList = new List<float[]>();

            for (int i = 0; i < layers.Length; i++)
            {
                neuronList.Add(new float[layers[i]]);
            }

            neurons = neuronList.ToArray();
        }

        private void InitWeights()
        {
            Random rand = new Random();
            List<float[][]> weightList = new List<float[][]>();

            for (int i = 1; i < layers.Length; i++)
            {
                List<float[]> layerWeightList = new List<float[]>();

                int neuronsInPreviousLayer = layers[i - 1];

                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float[] neuronWeights = new float[neuronsInPreviousLayer];

                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        neuronWeights[k] = (float)(rand.NextDouble() * 2 - 1);
                        
                    }

                    layerWeightList.Add(neuronWeights);
                
                }

                weightList.Add(layerWeightList.ToArray());

            }

            weights = weightList.ToArray();
        }

        public float[] FeedForward(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
                //if (inputs[i] > 1 || inputs[i] < -1)
                    neurons[0][i] = (float)Math.Tanh((double)inputs[i]);
                //else
                  //  neurons[0][i] = inputs[i];
            for (int i = 1; i < layers.Length; i++)
            {

                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float value = 0.25f;

                    for (int k = 0; k < neurons[i - 1].Length; k++)
                    {
                        value += weights[i - 1][j][k] * neurons[i - 1][k];
                    }
                    neurons[i][j] = (float)Math.Tanh(value);
                }
            }

            return neurons[neurons.Length - 1];
        }

        public void Mutate()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        float weight = weights[i][j][k];

                        float mutationChance = (float)(rand.NextDouble()*100);

                        if (mutationChance <= 3f)
                        { //if 1
                          //flip sign of weight
                            weight *= -1f;
                        }
                        else if (mutationChance <= 6f)
                        { //if 2
                          //pick random weight between -1 and 1
                            weight = (float)(rand.NextDouble() * 2 - 1);
                        }
                        else if (mutationChance <= 9f)
                        { //if 3
                          //randomly increase by 0% to 100%
                            float factor = (float)(rand.NextDouble() + 1);
                            weight *= factor;
                        }
                        else if (mutationChance <= 12f)
                        { //if 4
                          //randomly decrease by 0% to 100%
                            float factor = (float)rand.NextDouble();
                            weight *= factor;
                        }

                        weights[i][j][k] = weight;
                    }
                }
            }
        }

        public void AddFitness(float fit)
        {
            fitness += fit;
        }

        public void SetFitness(float fit)
        {
            fitness = fit;
        }

        public float GetFitness()
        {
            return fitness;
        }

        public int CompareTo(NeuralNetwork other)
        {
            if (other == null) return 1;
            if (fitness < other.fitness)
                return 1;
            else if (fitness > other.fitness)
                return -1;
            else
                return 0;
        }
    }
}

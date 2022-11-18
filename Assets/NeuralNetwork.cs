﻿using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    public class NeuralNetwork
    {
        public List<Layer> layers;

        public NeuralNetwork(int[] neuronCounts)
        {
            layers = new List<Layer>(neuronCounts.Length - 1);

            for (int i = 0; i < neuronCounts.Length - 1; i++)
            {
                layers.Add(
                    new Layer(
                        neuronCounts[i],
                        neuronCounts[i + 1]
                    )
                );
            }
        }

        public static List<float> feedForward(List<float> givenInputs, NeuralNetwork network)
        {
            List<float> outputs = Layer.feedForward(
                givenInputs, network.layers[0]
            );

            for (int i = 1; i < network.layers.Count; i++)
            {
                outputs = Layer.feedForward(
                    outputs, network.layers[i]
                );
            }

            return outputs;
        }

        public NeuralNetwork mutate(float amount)
        {
            layers.ForEach(level =>
            {
                for (int i = 0; i < level.biases.Count; i++)
                {
                    level.biases[i] =  Mathf.Lerp(
                        level.biases[i],
                        Random.Range(-1.0f, 1.0f),
                        amount
                    );
                }

                for (int i = 0; i < level.weights.Count; i++)
                {
                    for (int j = 0; j < level.weights[i].Count; j++)
                    {
                        level.weights[i][j] =  Mathf.Lerp(
                            level.weights[i][j],
                            Random.Range(-1.0f, 1.0f),
                            amount
                        );
                    }
                }
            });
            return this;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        public NeuralNetwork DeepCopy()
        {
            return (NeuralNetwork)MemberwiseClone();
        }
    }
}
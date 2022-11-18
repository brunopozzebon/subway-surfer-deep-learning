using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    public class Layer
    {
        public List<float> inputs;
        public List<float> outputs;
        public List<float> biases;
        public List<List<float>> weights;
        public float inputCount;
        public float outputCount;

        
        public Layer(int inputCount, int outputCount)
        {
            inputs = new List<float>(inputCount);
            outputs = new List<float>(outputCount);
            biases = new List<float>(outputCount);
            weights = new List<List<float>>(inputCount);
            this.inputCount = inputCount;
            this.outputCount = outputCount;

            for (int i = 0; i < inputCount; i++)
            {
                inputs.Add(0);
                weights.Add( new List<float>(outputCount));
            }
            
            randomize();
        }

        void randomize()
        {
            for (int i = 0; i < inputCount; i++)
            {
                for (int j = 0; j < outputCount; j++)
                {
                    weights[i].Add( Random.Range(-1.0f, 1.0f));
                }
            }

            for (int i = 0; i < outputCount; i++)
            {
                outputs.Add(0);
                biases.Add( Random.Range(-1.0f, 1.0f));
            }
        }

        public static List<float> feedForward(List<float> givenInputs, Layer layer)
        {
            for (int i = 0; i < layer.inputCount; i++)
            {
                layer.inputs[i] = givenInputs[i];
            }

            for (int i = 0; i < layer.outputCount; i++)
            {
                float sum = 0;

                for (int j = 0; j < layer.inputCount; j++)
                {
                    sum += layer.inputs[j] * layer.weights[j][i];
                }
                
                layer.outputs[i] =  sum > layer.biases[i] ? 1 :0;
            }
            
            

            return layer.outputs;
        }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
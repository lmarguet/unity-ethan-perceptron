﻿using System;
 using System.Collections.Generic;
 using System.IO;
 using System.Linq;
 using TreeEditor;
 using UnityEngine;
 using Random = System.Random;

namespace Assets.Scripts
{
    public class Perceptron: MonoBehaviour
    { 
        public GameObject NPC;

        private List<TrainingSet> trainingSets;
        private double[] weights;
        private double bias;
        private double totalError;

        public void SendInput(double input1, double input2, double output)
        {
            var result = CalculateOutput(input1, input2);

            var trigger = result == 0;
            if (trigger){
                NPC.GetComponent<Animator>().SetTrigger("Crouch");
            }
            
            NPC.GetComponent<Rigidbody>().isKinematic = !trigger;
            
            
            trainingSets.Add(
                new TrainingSet(
                    new[] {input1, input2},
                    output
                 )
             );
            Train();
        }
        
        
        void Start()
        {
           weights = new double[2];
           trainingSets = new List<TrainingSet>();
           InitialiseWeights();
           LoadWeights();
        }

        public void Train()
        {
            for (var j = 0; j < trainingSets.Count; j++)
            {
                UpdateWeights(j);
            }
                
//            Debug.Log(string.Format("Epoch:{0} Total errors:{1}", i, totalError));
//            Debug.Log("##########################################");
            SaveWeights();
        }
        

        void InitialiseWeights()
        {
            for (var i = 0; i < weights.Length; i++)
            {
                weights[i] = RandomRange(-1.0f, 1.0f);
            }

            bias = RandomRange(-1.0f, 1.0f);
        }


        void UpdateWeights(int index)
        {
            var currentSet = trainingSets[index];
            var output = CalculateOutput(currentSet);
            var error = currentSet.Output - output;

            totalError += Math.Abs((float)error);

            for (var i = 0; i < weights.Length; i++)
            {
                weights[i] += error * currentSet.Input[i];
            }

            bias += error;

//            LogInfos(currentSet, output, error);
        }


        private double CalculateOutput(TrainingSet set)
        {
            return Activation(
                DotProductBias(
                    weights, 
                    set.Input
                )
            );
        }
        
        
        public double CalculateOutput(double input1, double input2)
        {
            return Activation(
                DotProductBias(
                    weights,
                    new[] { input1, input2 }
                )
            );
        }
        

        private double Activation(double product)
        {
            return  product > 0 ? 1 : 0;
        }


        private double DotProductBias(double[]weights, double[] inputs)
        {
            if (inputs == null) throw new ArgumentNullException("inputs");
            if((weights == null || inputs == null)
               || (weights.Length != inputs.Length))
            {
                return -1;
            }

            var product = weights.Select((t, i) => t * inputs[i])
                                 .Sum();

            product += bias;

            return product;
        }

        private static double RandomRange(float lower, float upper)
        {
            var random = new Random();
            return random.NextDouble() * (upper - lower) + (lower - 0);
        }

        
        private void LogInfos(TrainingSet currentSet, double output, double error)
        {
            var log = string.Format("Inputs: {0} {1}\t Weights: {2}\t{3}\tBias: {4} \tOutput: {5}\t Desired: {6}\tError: {7}",
                currentSet.Input[0],
                currentSet.Input[1],
                weights[0],
                weights[1],
                bias,
                output,
                currentSet.Output,
                error);
            Debug.Log(log);
        }


        public void ResetTraining()
        {
            InitialiseWeights();
            trainingSets.Clear();
        }

        private void LoadWeights()
        {
            var path = Application.dataPath + "/weights.txt";
            if (File.Exists(path)){
                var content = File.OpenText(path);
                
                var values = content.ReadLine().Split(',');
                weights[0] = Convert.ToDouble(values[0]);
                weights[1] = Convert.ToDouble(values[1]);
                bias = Convert.ToDouble(values[2]);
                
                Debug.Log("Weights loaded");
                content.Close();
            }
        }


        private void SaveWeights()
        {
            var path = Application.dataPath + "/weights.txt";
            
            var txt = File.CreateText(path);
            txt.WriteLine(
                string.Format(
                    "{0},{1},{2}", 
                    weights[0], weights[1], bias
                )
            );
            txt.Close();
        }
    }
}

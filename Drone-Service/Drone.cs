﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace Drone_Service
{
    // 6.1	Create a separate class file to hold the data items of the Drone.
    // Use separate getter and setter methods, ensure the attributes are private and the accessor methods are public.
    // Add a display method that returns a string for Client Name and Service Cost.
    // Add suitable code to the Client Name and Service Problem accessor methods so the data is formatted as Title case or Sentence case.
    // Save the class as “Drone.cs”.
    internal class Drone
    {
        // Attributes
        private string client;
        private string model;
        private string problem;
        private int cost;
        private int tag;

        // getter
        public string GetClient()
        {
            return client;
        }
        
        // setter
        public void SetClient(string newClient)
        {
            client = newClient;
        }

        public string GetModel()
        {
            return model;
        }

        public void SetModel(string newModel)
        {
            model = newModel;
        }

        public string GetProblem()
        {
            return problem;
        }

        public void SetProblem(string newProblem)
        {
            problem = newProblem;
        }

        public int GetCost()
        {
            return cost;
        }

        public void SetCost(int newCost)
        {
            cost = newCost;
        }

        public int GetTag()
        {
            return tag;
        }

        public void SetTag(int newTag)
        {
            tag = newTag;
        }

    }
}

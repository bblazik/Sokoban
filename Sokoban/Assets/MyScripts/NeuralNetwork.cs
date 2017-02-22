using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using Layer = System.Collections.Generic.List<Neuron>;


class Connection
{
    public double weight ;
    public double deltaWeight;
}

public class Neuron
{
    private double m_outputValue, m_gradient;
    private List<Connection> m_outputWeights;
    public int m_myIndex;
    static private double alpha = 0.5, // [[0.0 .. n]   multipler of last weight change (momentum)
                            eta = 0.15;     // [0.0 ... 1.0] overall net training rate

    public Neuron(int numOutputs, int myIndex)
    {
        m_myIndex = myIndex;
        for (var c = 0; c < numOutputs; c++)
        {
            m_outputWeights.Add(new Connection());
            m_outputWeights.Last().weight = randomWeight();
        }
    }

    public void feedForward(ref Layer prevLayer)
    {
        var sum = 0.0;

        for (var n = 0; n < prevLayer.Count(); n++)
        {
            sum += prevLayer[n].getOutputVal()*prevLayer[n].m_outputWeights[m_myIndex].weight;
        }
        m_outputValue = transferFunction(sum);
    }

    static double transferFunction(double x)
    {
        return Math.Tanh(x);
        //sigmoid function return 1 / (1 + Mathf.Exp(-x));
    }

    static double transferDerivedFunction(double x)
    {
        return 1 - x*x; // float y = TransferFunction (x); return (y * (1 - y));
    }

    public void setOutputVal(double val)
    {
        m_outputValue = val;
    }

    public double getOutputVal()
    {
        return m_outputValue;
    }

    static double randomWeight()
    {
        var rand = new System.Random();
        return rand.NextDouble();
    }

    public void calcOutputGradients(double targetVal)
    {
        double delta = targetVal - m_outputValue;
        m_gradient = delta*transferDerivedFunction(m_outputValue);
    }

    public void calcHiddenGradients(ref Layer nextLayer)
    {
        double dow = sumDOW(ref nextLayer);
        m_gradient = dow*transferDerivedFunction(m_outputValue);
    }

    double sumDOW(ref Layer nextLayer)
    {
        double sum = 0.0;
        for (var n = 0; n < nextLayer.Count(); n++)
        {
            sum += m_outputWeights[n].weight*nextLayer[n].m_gradient;
        }

        return sum;
    }

    public void updateInputWeights(ref Layer prevLayer)
    {
        for (var n = 0; n < prevLayer.Count(); n++)
        {
            var neuron = prevLayer[n];
            double oldDeltaWeight = neuron.m_outputWeights[m_myIndex].deltaWeight;

            double newDeltaWeight =
                //Individual input, magnified bby the gradient and trai rate;
                eta
                *neuron.getOutputVal()
                *m_gradient
                + alpha
                *oldDeltaWeight;
            neuron.m_outputWeights[m_myIndex].deltaWeight = newDeltaWeight;
            neuron.m_outputWeights[m_myIndex].weight += newDeltaWeight;
        }
    }
};

class NeuralNetwork
{
    private List<int> Topology;
    //private List<Neuron> Layer;
    public List<Layer> m_layers;
    public double m_error;
    public double m_recentAverageError;
    public double m_recentAverageSmoothingFactor;

    public NeuralNetwork(List<int> topology )
    {
        Topology = topology;
        var numLayers = Topology.Count();
        for (int i = 0; i < numLayers; i++)
        {
            m_layers.Add(new Layer());;
            int numOutputs = i == Topology.Count() - 1 ? 0 : Topology[i + 1];
            //Add bio-neuron to each layer.
            for (var neuronsNum = 0; i <= Topology[i]; neuronsNum++)
            {
                m_layers.Last().Add(new Neuron(numOutputs, neuronsNum));
                Debug.Log("I made neuron");
            }

            m_layers.Last().Last().setOutputVal(1.0);
        }
    }

    void feedForward(List<double> inputValues)
    {
        Assert.AreEqual(inputValues.Count(), m_layers[0].Count() - 1);

        //Assign input values into input neurons
        for (var i = 0; i < inputValues.Count(); i++)
        {
            m_layers[0][i].setOutputVal(inputValues[i]);
        }

        //Forward propagate

        for (var layerNum = 1; layerNum < m_layers.Count(); ++layerNum)
        {
            Layer prevLayer = m_layers[layerNum - 1];
            for (var n = 0; n < m_layers[layerNum].Count() - 1; ++n)
            {
                m_layers[layerNum][n].feedForward(ref prevLayer);
            }
        }
    
}

    void backProp(List<double> targetVals)
    {
        //Calculate overall net error
        Layer outputLayer = m_layers.Last();
        m_error = 0.0;

        for (var n = 0; n < outputLayer.Count() - 1; n++)
        {
            double delta = targetVals[n] - outputLayer[n].getOutputVal();
            m_error += delta*delta;
        }

        m_error /= outputLayer.Count() - 1;
        m_error = Math.Sqrt(m_error);

        //Implement a recent average measurment
        m_recentAverageError = (m_recentAverageError*m_recentAverageSmoothingFactor + m_error)/
                               (m_recentAverageSmoothingFactor + 1.0);

        //Calculate output layer gradients
        for (var n = 0; n < outputLayer.Count(); n++)
        {
            outputLayer[n].calcOutputGradients(targetVals[n]);
        }

        //Calculate gradients on hidden layer
        for (var layerNum = m_layers.Count() - 2; layerNum > 0; --layerNum)
        {
            Layer hiddenLayer = m_layers[layerNum];
            Layer nextLayer = m_layers[layerNum + 1];

            for (var n = 0; n < hiddenLayer.Count(); n++)
            {
                hiddenLayer[n].calcHiddenGradients(ref nextLayer);
            }
        }

        //For all layers from outputs to first hidden layer,
        //update connection weights
        for (var layerNum = m_layers.Count() - 1; layerNum > 0; layerNum--)
        {
            Layer layer = m_layers[layerNum];
            Layer prevLayer = m_layers[layerNum - 1];

            for (var n = 0; n < layer.Count() - 1; n++)
            {
                layer[n].updateInputWeights(ref prevLayer);
            }
        }
    }

    void getResults(ref List<double> resultValues )
    {
        resultValues.Clear();
        for (var n = 0; n < m_layers.Last().Count() - 1; n++)
        {
            resultValues.Add(m_layers.Last()[n].getOutputVal());
        }
    }
};

class Usage : MonoBehaviour
{
    void Start()
    {
        var Net = new NeuralNetwork(new List<int>(new int[]{3,2,1}));
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class Levy
{
    //void Start()
    //{
    //    // Check if same as in python, it is!
    //    //print(CDF.NormInv(0.25f, 0.0f, 1.0f)); 
    //    //print(CDF.NormInv(0.5f, 0.0f, 1.0f));
    //    //print(CDF.NormInv(0.75f, 0.0f, 1.0f));

    //    // Write numbers to file to check in Python if the distribution is correct
    //    //StreamWriter writer = new StreamWriter("Assets/levy.txt", false);
    //    //for (int i = 0; i < 1000; i++)
    //    //{
    //    //    float levyVal = GetLevyCutoff(2f, 1f, 5000f);
    //    //    writer.WriteLine(levyVal);
    //    //}
    //    //writer.Close();
    //    //print("--- DONE WRITING TO FILE ---");
    //}

    public float GetLevy(float mu, float c)
    {
        float U = UnityEngine.Random.value;
        float normal = (float) Math.Pow(NormInv(1f - U), 2f);
        return (c / normal + mu);
    }

    public float GetLevyCutoff(float mu, float c, float cutoff)
    {
        float levyVal = -1f;
        while (levyVal < 0f || levyVal > cutoff)
        {
            levyVal = GetLevy(mu, c);
        }
        return levyVal;
    }

    public float[] GetLevySamples(int nr_of_samples, float mu, float c)
    {
        float[] levySamples = new float[nr_of_samples];
        for (int i = 0; i < nr_of_samples; i++)
        {
            levySamples[i] = GetLevy(mu, c);
        }
        return levySamples;
    }

    public float[] GetLevySamplesCutoff(int nr_of_samples, float mu, float c, float cutoff)
    {
        float[] levySamples = new float[nr_of_samples];
        for (int i = 0; i < nr_of_samples; i++)
        {
            levySamples[i] = GetLevyCutoff(mu, c, cutoff);
        }
        return levySamples;
    }

    // NormInv has two forms. This form allows for an offset normal distribution with a standard deviation different than 1.
    // Three variables are required for this variant: probability, mean and standard deviation (sigma).
    public float NormInv(float probability, float mean, float sigma)
    {
        float x = NormInv(probability);
        return sigma * x + mean;
    }

    // NormInv has two forms. This form allows for an offset normal distribution with a standard deviation different than 1.
    // One variable is required for this varient: only the probability. The mean is assumed to be 0 and the standard deviation is assumed to be 1.
    public float NormInv(float probability)
    {

        // Define variables used in intermediate steps
        float q = 0f;
        float r = 0f;
        float x = 0f;

        // Coefficients in rational approsimations.
        float[] a = new float[] { -3.969683028665376e+01f, 2.209460984245205e+02f, -2.759285104469687e+02f, 1.383577518672690e+02f, -3.066479806614716e+01f, 2.506628277459239e+00f };
        float[] b = new float[] { -5.447609879822406e+01f, 1.615858368580409e+02f, -1.556989798598866e+02f, 6.680131188771972e+01f, -1.328068155288572e+01f };
        float[] c = new float[] { -7.784894002430293e-03f, -3.223964580411365e-01f, -2.400758277161838e+00f, -2.549732539343734e+00f, 4.374664141464968e+00f, 2.938163982698783e+00f };
        float[] d = new float[] { 7.784695709041462e-03f, 3.224671290700398e-01f, 2.445134137142996e+00f, 3.754408661907416e+00f };

        // Define break-points
        float pLow = 0.02425f;
        float pHigh = 1f - pLow;

        // Verify that probability is between 0 and 1 (noninclusinve), and if not, make between 0 and 1

        if (probability <= 0f)
        {
            probability = Mathf.Epsilon;
        }
        else if (probability >= 1f)
        {
            probability = 1f - Mathf.Epsilon;
        }

        // Rational approximation for lower region.
        if (probability < pLow)
        {
            q = Mathf.Sqrt(-2f * Mathf.Log(probability));
            x = (((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) / ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1f);
        }

        // Rational approximation for central region.
        if (pLow <= probability && probability <= pHigh)
        {
            q = probability - 0.5f;
            r = q * q;
            x = (((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * q / (((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1f);
        }

        // Rational approximation for upper region.
        if (pHigh < probability)
        {
            q = Mathf.Sqrt(-2 * Mathf.Log(1f - probability));
            x = -(((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) / ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1f);
        }

        return x;
    }

    //    double norm_cdf(double x, double loc, double scale)
    //    {
    //        return Math.Exp(Math.Pow(-x, 2) / 2) / (Math.Sqrt(2 * Math.PI));
    //    }
}

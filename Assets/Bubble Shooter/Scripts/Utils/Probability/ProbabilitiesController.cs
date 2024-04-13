using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProbabilitiesController
{
    #region Basic Probability check

    /// <summary>
    /// This is the basic Probability function that will take the item chance is 30% and then check if you'll get or not
    /// </summary>
    /// <param name="itemProbability"></param>
    /// <returns></returns>
    public static bool ProbabilityCheck(int itemProbability)
    {
        float rnd = Random.Range(1, 101);
        return rnd <= itemProbability;
    }
    #endregion

    #region Normal items Probability from 100
    private static List<float> _cumulativeProbability;

    /// <summary>
    /// This function is called with the Item probability array and it'll return the index of the item, 
    /// for example the list can look like [10,25,30] so the first item has 10% of showing and next one has 25% and so on
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    public static int GetItemByProbability(List<float> probability) //[50,10,20,20]
    {
        //if your game will use this a lot of time it is best to build the arry just one time
        //and remove this function from here.
        if (!MakeCumulativeProbability(probability))
            return -1; //when it return false then the list excceded 100 in the last index

        float rnd = Random.Range(1, 101); //GetEntity a random number between 0 and 100

        for (int i = 0; i < probability.Count; i++)
        {
            if (rnd <= _cumulativeProbability[i]) //if the probility reach the correct sum
            {
                return i; //return the item index that has been chosen 
            }
        }

        return -1; //return -1 if some error happens
    }

    /// <summary>
    /// This function creates the cumulative list
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    private static bool MakeCumulativeProbability(List<float> probability)
    {
        float probabilitiesSum = 0;

        _cumulativeProbability = new List<float>(); //reset the Array

        for (int i = 0; i < probability.Count; i++)
        {
            probabilitiesSum += probability[i]; //add the probability to the sum
            _cumulativeProbability.Add(probabilitiesSum); //add the new sum to the list

            //All Probabilities need to be under 100% or it'll throw an exception
            if (probabilitiesSum > 100f)
            {
                Debug.LogError("Probabilities exceed 100%");
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Normal items Probability By Rarity
    private static List<float> _cumulativeByRarity;
    /// <summary>
    /// This function is called with the Item probability array and it'll return the index of the item,
    /// for example the list can look like [10,25,30] so the first item has 10% of showing and next one has 25% and so on
    /// </summary>
    /// <param name="probabilityRarity"></param>
    /// <returns></returns>
    public static int GetItemByProbabilityRarity(List<float> probabilityRarity)
    {
        //if your game will use this a lot of time it is best to build the arry just one time
        //and remove this function from here.
        MakeCumulativeByProbabilityRarity(probabilityRarity);

        float rnd = Random.Range(1, 101); //GetEntity a random number between 0 and 100

        for (int i = 0; i < probabilityRarity.Count; i++)
        {
            if (rnd <= _cumulativeByRarity[i]) //if the probility reach the correct sum
            {
                return i; //return the item index that has been chosen 
            }
        }

        return -1; //return -1 if some error happens
    }


    /// <summary>
    /// This function creates the cumulative list
    /// </summary>
    /// <param name="probabilityRarity"></param>
    private static void MakeCumulativeByProbabilityRarity(List<float> probabilityRarity)
    {
        float probabilitiesSum = 0;

        _cumulativeByRarity = new List<float>(); //reset the Array

        float ProbilityModifier = GetprobabilityByRarityModifer(probabilityRarity);

        for (int i = 0; i < probabilityRarity.Count; i++)
        {
            probabilitiesSum += probabilityRarity[i] * ProbilityModifier; //add the probability to the sum
            _cumulativeByRarity.Add(probabilitiesSum); //add the new sum to the list
        }

        //No need to check if it's bigger than 100 because it'll always be 100 max
    }

    /// <summary>
    /// This function is used to get a modifer to be able to get the probabilityRarity List to fit in the _cumulativeByRarity list from 0 to 100
    /// </summary>
    /// <param name="probabilityRarity"></param>
    /// <returns></returns>
    private static float GetprobabilityByRarityModifer(List<float> probabilityRarity) // 5 , 20 , 2
    {
        float itemRaritySum = 0;

        for (int i = 0; i < probabilityRarity.Count; i++)
            itemRaritySum += probabilityRarity[i];

        return 100f / itemRaritySum;
    }

    #endregion

    #region Normal Probability Num In Num
    /// <summary>
    /// This Function will return true if the item is lucky enough to be picked by a small chance of one in something and 100% chance to get the item if its 1 in 1 ... XD
    /// </summary>
    /// <param name="In"></param>
    /// <returns></returns>
    public static bool OneInProbability(int In)
    {
        float rnd = Random.Range(1, In + 1);
        return rnd <= 1;
    }

    /// <summary>
    /// This Function will return true if the item is lucky enough to be picked by a small chance of num in something.
    /// This can be used instead the first one without passing the second variable
    /// </summary>
    /// <param name="In"></param>
    /// <param name="chance"></param>
    /// <returns></returns>
    public static bool ChanceInProbability(int In, int chance = 1)
    {
        float rnd = Random.Range(1, In + 1);
        return rnd <= chance;
    }
    #endregion
}
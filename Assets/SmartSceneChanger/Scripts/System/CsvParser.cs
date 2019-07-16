using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Functions for csv
    /// </summary>
    public class CsvParser
    {

        /// <summary>
        /// Get lines from csv text
        /// </summary>
        /// <param name="csv">csv string</param>
        /// <param name="punctuation">punctuation</param>
        /// <returns>2d string array</returns>
        // ------------------------------------------------------------------------------------------------
        public static List<List<string>> parse(string csv, char punctuation = ',')
        {

            StringBuilder tempSB = new StringBuilder();

            List<List<string>> ret = new List<List<string>>();

            List<string> oneRow = new List<string>();

            int totalQuoteCounter = 0;
            int consecutiveQuoteCounter = 0;

            foreach (char c in csv)
            {

                if (c == '\"')
                {

                    consecutiveQuoteCounter++;

                    if (consecutiveQuoteCounter % 2 == 0)
                    {
                        tempSB.Append(c);
                    }

                    totalQuoteCounter++;

                    continue;

                }

                // ---------------

                consecutiveQuoteCounter = 0;

                // ---------------

                if (c == punctuation && totalQuoteCounter % 2 == 0)
                {
                    oneRow.Add(tempSB.ToString());
                    tempSB.Length = 0;
                }

                else if (c == '\n' && totalQuoteCounter % 2 == 0)
                {
                    oneRow.Add(tempSB.ToString());
                    tempSB.Length = 0;
                    ret.Add(oneRow);
                    oneRow = new List<string>();
                }

                else if(c != '\r')
                {
                    tempSB.Append(c);
                }

            }

            // last
            {
                oneRow.Add(tempSB.ToString());
                ret.Add(oneRow);
            }

            return ret;

        }

        /// <summary>
        /// Convert List to string[,]
        /// </summary>
        /// <param name="target">List</param>
        /// <returns>string 2d array</returns>
        // ------------------------------------------------------------------------------------------------
        public static string[,] convertTo2dArray(List<List<string>> target)
        {

            int rowSize = target.Count;
            int colSize = (rowSize > 0) ? target[0].Count : 0;

            string[,] ret = new string[rowSize, colSize];

            for(int row = 0; row < rowSize; row++)
            {

#if UNITY_EDITOR

                if(colSize != target[row].Count)
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) : Invalid format");
                }

#endif

                colSize = target[row].Count;

                for (int col = 0; col < colSize; col++)
                {
                    ret[row, col] = target[row][col];
                }

            }

            return ret;

        }

    }

}

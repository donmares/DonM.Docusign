﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DonM.Docusign.Library.Interfaces;
using DonM.Docusign.Library.Models;

namespace DonM.Docusign.Library.Services
{
    public class ProximitySearch: IProximitySearch
    {
        private string _FirstKeyword = "";
        private string _SecondKeyword = "";
        private int _Range = 0;
        private string _SearchString = "";
        
        public ProximitySearch(string firstKeyword, string secondKeyword, int range, string searchString)
        {
            _FirstKeyword = ValidateKeyword(firstKeyword);
            _SecondKeyword = ValidateKeyword(secondKeyword);

            if (_FirstKeyword == _SecondKeyword)
                throw new Exception("Error - first and second keywords cannot be the same");
            if (range < 2)
                throw new Exception("Error - range must be at least the number of keywords or 2");

            _Range = range;
            _SearchString = ValidateSearchString(searchString);
        }
        
        /// <summary>
        /// execute proximity search
        /// step 1 - divide search string into all possible ranges
        /// step 2a - loop all ranges getting matches in each range
        /// step 2b - store only new matches where not already stored
        /// step 3 - return count of matches
        /// </summary>
        /// <returns></returns>
        public int ExecuteSearch()
        {
            List<KeywordMatch> matchList = new List<KeywordMatch>();
            List<Dictionary<int, string>> rangeDic = GetRanges();

            foreach (Dictionary<int, string> rangeString in rangeDic)
            {
                List<KeywordMatch> rangeMatchList = GetRangeMatches(rangeString);
                IEnumerable<KeywordMatch> newMatches = rangeMatchList.Where(r => !matchList.Any(m => m.Equals(r)));
                matchList.AddRange(newMatches);
            }

            return matchList.Count;
        }

        /// <summary>
        /// return all possible ranges starting with position 0 and ending in length of range
        /// range consists of dictionary of index position of words and actual words
        /// 
        /// This should be a private method but made public for testing purposes
        /// Most likely able to improve performance using mostly all Linq instead of loops
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<int, string>> GetRanges()
        {
            string[] searchArray = _SearchString.Split(' ');
            List<Dictionary<int, string>> rangeList = new List<Dictionary<int, string>>();  ///key = index of search word, value = search word

            if (searchArray.Length < _Range)
                _Range = searchArray.Length;

            for (int idx = 0; idx <= searchArray.Length - _Range; idx++)
            {
                Dictionary<int, string> rangeDic = new Dictionary<int, string>();
                for (int rangeIdx = 0; rangeIdx < _Range; rangeIdx++)
                {
                    rangeDic.Add(idx + rangeIdx, searchArray[idx + rangeIdx]);
                }
                rangeList.Add(rangeDic);
            }

            return rangeList;
        }

        /// <summary>
        /// return all possible keyword matches in range dictionary
        /// step 1 - store all matching keywords in dictionary
        /// step 2 - add all combinations of 1st and 2nd keywords in matchlist
        /// Most likely able to improve performances using mostly all Linq instead of loops
        /// </summary>
        /// <param name="rangeDic"></param>
        /// <returns></returns>
        private List<KeywordMatch> GetRangeMatches(Dictionary<int, string> rangeDic)
        {
            List<KeywordMatch> matchList = new List<KeywordMatch>();
            Dictionary<int, int> keywordDic = new Dictionary<int, int>();   //key = index, value = 1 or 2 designating first or second keyword

            //step 1 - store all matching keywords in dictionary
            for (int idx=rangeDic.Keys.Min(); idx <= rangeDic.Keys.Max(); idx++)
            {
                if (rangeDic[idx].Trim().ToLower() == _FirstKeyword)
                {
                    keywordDic.Add(idx, 1);
                }
                else if (rangeDic[idx].Replace("\r", "").Replace("\n", "").Trim().ToLower() == _SecondKeyword)
                {
                    keywordDic.Add(idx, 2);
                }
            }

            //step 2 - add all combinations of 1st and 2nd keywords in matchList
            if (keywordDic.ContainsValue(1) && keywordDic.ContainsValue(2))
            {
                IEnumerable<KeyValuePair<int, int>> firstKeywordList = keywordDic.Where(k => k.Value == 1);
                IEnumerable<KeyValuePair<int, int>> secondKeywordList = keywordDic.Where(k => k.Value == 2);
                foreach (KeyValuePair<int, int> firstKvp in firstKeywordList)
                {
                    foreach (KeyValuePair<int, int> secondKvp in secondKeywordList)
                    {
                        KeywordMatch keywordMatch = new KeywordMatch() { FirstPosition = firstKvp.Key, SecondPosition = secondKvp.Key };
                        matchList.Add(keywordMatch);
                    }
                }
            }

            return matchList;
        }

        /// <summary>
        /// removes excess blank characters and line feed characters.
        /// Consider removing punctuation like periods, hyphens and others
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private string ValidateSearchString(string inputString)
        {
            StringBuilder sb = new StringBuilder(inputString.Trim());
            sb.Replace("\r", " ").Replace("\n", " ");
            while (sb.ToString().Contains("  "))
                sb.Replace("  ", " ");
            return sb.ToString();
        }

        /// <summary>
        /// verifies no white spaces in keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private string ValidateKeyword(string keyword)
        {
            Regex regex = new Regex("[\\s]");
            if (regex.IsMatch(keyword))
                throw new Exception("Error - keyword cannot contain white space");

            return keyword.ToLower().Trim();
        }
    }
}

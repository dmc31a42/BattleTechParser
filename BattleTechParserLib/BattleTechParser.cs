using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BattleTechParser.Lib
{
    public class Parser
    {
        public const string SpecialUS = "\u001F";

        private Dictionary<string, string> keyValue = new Dictionary<string, string>();

        public int Count {
            get
            {
                return this.keyValue.Count;
            }
        }

        public Parser() { }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Parser parser = (Parser)obj;
                if (this.keyValue.Count != parser.keyValue.Count) return false;
                IEqualityComparer<string> valueComparer = EqualityComparer<string>.Default;
                Dictionary<string, string> dict1 = this.keyValue;
                Dictionary<string, string> dict2 = parser.keyValue;
                return dict1.Count == dict2.Count &&
                        dict1.Keys.All(key => dict2.ContainsKey(key) && valueComparer.Equals(dict1[key], dict2[key]));
            }
        }

        public static Parser operator -(Parser a, Parser b)
        {
            Parser parser = new Parser();
            foreach (KeyValuePair<string, string> keyValuePair in a.keyValue)
            {
                if (!b.keyValue.ContainsKey(keyValuePair.Key))
                {
                    parser.keyValue.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return parser;
        }

        public static Parser operator +(Parser a, Parser b)
        {
            Parser parser = new Parser();
            foreach(KeyValuePair<string, string> keyValuePair in a.keyValue)
            {
                parser.keyValue[keyValuePair.Key] = keyValuePair.Value;
            }
            foreach (KeyValuePair<string, string> keyValuePair in b.keyValue)
            {
                parser.keyValue[keyValuePair.Key] = keyValuePair.Value;
            }
            return parser;
        }

        public bool AddCSVFromFilePath(string CSVFilePath)
        {
            try
            {
                string[] csvStrs = System.IO.File.ReadAllLines(CSVFilePath);
                foreach(string csvStr in csvStrs)
                {
                    string[] splited = SplitLine(csvStr);
                    if(splited != null)
                    {
                        keyValue.Add(splited[0], splited[1]);
                    }
                }
                if(keyValue.Count != 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            } catch (Exception)
            {
                return false;
            }
        }

        public bool AddJsonFromFolderPath(string FolderPath)
        {
            
            throw new NotImplementedException();
        }

        public static string[] SplitLine(string v)
        {
            string[] splitedString = v.Split(',');

            if (splitedString.Length != 2 || splitedString[0] == "")
            {
                return null;
            }
            else
            {
                splitedString[1] = splitedString[1].Replace((char)0x1F, ',');
                return splitedString;
            }
        }

        public override int GetHashCode()
        {
            return -1415560723 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(keyValue);
        }

        public static string GetKeyFromString(string v)
        {
            string temp = Regex.Replace(v, @"(?:\.|,|~|@|\'|\""| |\t|\f|\r)", ""); // - 이 명확하지 않음
            temp = Regex.Replace(temp, @"(\r\n|\n)", "newline");
            return temp.ToLower();
        }
    }
}

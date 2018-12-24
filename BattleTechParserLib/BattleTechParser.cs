using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

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
            string csvStrs = "";
            try
            {
                csvStrs = System.IO.File.ReadAllText(CSVFilePath); 
            } catch (Exception)
            {
                return false;
            }
            return this.AddCSVFromString(csvStrs);
        }

        public bool AddCSVFromString(string str)
        {
            string[] csvStrs = str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string csvStr in csvStrs)
            {
                string[] splited = SplitLine(csvStr);
                if (splited != null)
                {
                    keyValue[splited[0]] = Parser.GetValueFromCSVString(splited[1]);
                }
            }
            if (keyValue.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddJsonFromFolderPath(string FolderPath)
        {
            
            string[] fileNames = System.IO.Directory.GetFiles(FolderPath, "*.json");
            foreach(string fileName in fileNames)
            {
                try
                {
                    string tempJsonStr = System.IO.File.ReadAllText(fileName);
                    Dictionary<string, string> tempKeyValues = ReadKeyValuesFromJObject(JObject.Parse(tempJsonStr));
                    foreach(KeyValuePair<string, string> tempKeyValuePair in tempKeyValues)
                    {
                        keyValue[tempKeyValuePair.Key] = tempKeyValuePair.Value;
                    }
                }
                catch (Exception)
                {

                }
            }
            string[] folderNames = System.IO.Directory.GetDirectories(FolderPath);
            foreach(string folderName in folderNames)
            {
                AddJsonFromFolderPath(folderName);
            }
            if(keyValue.Count != 0)
            {
                return true;
            } else
            {
                return false;
            }
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

        public static string GetKeyFromJsonString(string v)
        {
            //v = Regex.Replace(v, @"(?:\.|,|~|@|\'|\""| |\t|\f|\r)", ""); // - 이 명확하지 않음
            v = Regex.Replace(v, ",", "^", RegexOptions.Compiled);
            v = Regex.Replace(v, @"\.", "*", RegexOptions.Compiled);
            v = Regex.Replace(v, @"\n", "newline", RegexOptions.Compiled);
            v = Regex.Replace(v, @"\\n", "newline", RegexOptions.Compiled);
            v = Regex.Replace(v, @"\s+", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, @"(\[\[[\w\.\[\]]+,)", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, @"\]\]", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, "<.+?>", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, "…", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, "—", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, "\"", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, @"\'", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, @"\\\d\d\d\\\d\d\d\\\d\d\d", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, @"\\u\d\d\d\d", string.Empty, RegexOptions.Compiled);
            v = Regex.Replace(v, @"\\", string.Empty, RegexOptions.Compiled);
            //v = Regex.Replace(v, @"(\r\n|\n)", "newline");
            return v.ToLower();
        }

        public static string GetValueFromCSVString(string v)
        {
            v = v.Replace("\u001f", ",");
            v = v.Replace("\\r\\n", "\r\n");
            v = v.Replace("\\n", "\n");
            v = v.Replace("\\r", "\r");
            v = v.Replace("\\t", "\t");
            v = v.Replace("\\f", "\f");
            v = Regex.Replace(v, @"(.*)\\""(.*)\\""(.*)", @"$1“$2”$3");
            return v;
        }

        public static string GetPOTStringFromKeyValue(string v)
        {
            v = v.Replace("\"", "\\\"");
            v = v.Replace("\r\n", "\\r\\n");
            v = v.Replace("\n", "\\n");
            v = v.Replace("\r", "\\r");
            v = v.Replace("\t", "\\t");
            v = v.Replace("\f", "\\f");
            return v;
        }

        public static string GetKeyValueFromPOTString(string v)
        {
            v = v.Replace("\\r\\n", "\r\n");
            v = v.Replace("\\n", "\n");
            v = v.Replace("\\r", "\r");
            v = v.Replace("\\t", "\t");
            v = v.Replace("\\f", "\f");
            v = v.Replace("\\\"", "\"");
            return v;
        }

        public static Tuple<string, string> GetKeyValueFromJObject(JToken jObject)
        {
            if(jObject.Type == JTokenType.String) {
                return new Tuple<string, string>(GetKeyFromJsonString(jObject.Value<string>()), jObject.Value<string>());
            } else if(jObject.Type == JTokenType.Object)
            {
                throw new Exception(jObject.ToString() + " is not JProperty");
            } else
            {
                return null;
            }
        }

        public static Dictionary<string, string> ReadKeyValuesFromJObject(JToken jObject)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            foreach (JToken jToken in jObject.Children())
            {
                if(jToken.HasValues)
                {
                    Dictionary<string, string> tempKeyValues = ReadKeyValuesFromJObject(jToken);
                    foreach (KeyValuePair<string, string> tempKeyValurPair in tempKeyValues)
                    {
                        keyValues[tempKeyValurPair.Key] = tempKeyValurPair.Value;
                    }
                } else
                {
                    Tuple<string, string> tuple = GetKeyValueFromJObject(jToken);
                    if(tuple != null)
                    {
                        keyValues[tuple.Item1] = tuple.Item2;

                    }
                    
                }
            }
            return keyValues;
        }

        public Parser Merge(Parser battleTechParserMergeFrom)
        {
            Parser parser = new Parser();
            foreach(KeyValuePair<string, string> keyValuePair in this.keyValue)
            {
                if(battleTechParserMergeFrom.keyValue.ContainsKey(keyValuePair.Key))
                {
                    parser.keyValue[keyValuePair.Key] = battleTechParserMergeFrom.keyValue[keyValuePair.Key];
                } else
                {
                    parser.keyValue[keyValuePair.Key] = keyValuePair.Value;
                }
            }
            return parser;
        }

        public string ToCSVFormat()
        {
            string str = "";
            foreach(KeyValuePair<string, string> keyValuePair in this.keyValue)
            {
                str += keyValuePair.Key + "," + Parser.ToCSVStringFormat(keyValuePair.Value) + "\n";
            }
            return str;
        }

        public static string ToCSVStringFormat(string v)
        {
            v = v.Replace("\r\n", "\\r\\n");
            v = v.Replace("\n", "\\n");
            v = v.Replace("\r", "\\r");
            v = v.Replace("\t", "\\t");
            v = v.Replace("\f", "\\f");
            v = v.Replace ("\"\"", "\"");
            v = v.Replace("\"", "\"\"");
            v = v.Replace(",", "\u001f");
            return v;
        }

        public string ToPotFormat()
        {
            string str = "";
            foreach(KeyValuePair<string, string> keyValuePair in this.keyValue)
            {
                str += "msgctxt \"" + GetPOTStringFromKeyValue(keyValuePair.Key) + "\"\nmsgid \"" + GetPOTStringFromKeyValue(keyValuePair.Value) + "\"\nmsgstr \"\"\n\n";
            }
            return str;
        }

        public bool FromPotString(string potStr)
        {
            potStr = Regex.Replace(potStr, "\"(\\r\\n|\\n|\\r)\"", "");
            MatchCollection matchCollection = Regex.Matches(potStr, "msgctxt \"(.*)\"\\nmsgid \"(.*)\"\\nmsgstr \"(.*)\"");
            foreach(Match match in matchCollection)
            {
                if(match.Groups[2].Value == "")
                {
                    this.keyValue[GetKeyValueFromPOTString(match.Groups[0].Value)] = GetKeyValueFromPOTString(match.Groups[1].Value);
                } else
                {
                    this.keyValue[GetKeyValueFromPOTString(match.Groups[0].Value)] = GetKeyValueFromPOTString(match.Groups[2].Value);
                }
                
            }
            if(this.keyValue.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddGameTipsFromFilePath(string filePath)
        {
            string txtStrs = "";
            try
            {
                txtStrs = System.IO.File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                return false;
            }
            return this.AddGameTipsFromString(txtStrs);
        }

        public bool AddGameTipsFromString(string str)
        {
            string[] txtStrs = str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string txtStr in txtStrs)
            {
                if(txtStr != "")
                {
                    keyValue[Parser.GetKeyFromJsonString(txtStr)] = txtStr;
                }
            }
            if (keyValue.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddSQLFromString(string str)
        {
            MatchCollection matchCollection = Regex.Matches(str, @"'.{2,}?[^']'(?!')");
            foreach(Match match in matchCollection)
            {
                KeyValuePair<string, string> keyValuePair = GetKeyValueFromSQLLine(match.Groups[0].Value);
                keyValue[keyValuePair.Key] = keyValuePair.Value;
            }
            if (keyValue.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool AddSQLFromFilePath(string filePath)
        {
            string txtStrs = "";
            try
            {
                txtStrs = System.IO.File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                return false;
            }
            return this.AddSQLFromString(txtStrs);
        }

        public static KeyValuePair<string, string> GetKeyValueFromSQLLine(string v)
        {
            v = v.Replace("''", "'");
            return new KeyValuePair<string, string>(GetKeyFromJsonString(v), v);
        }


    }
}

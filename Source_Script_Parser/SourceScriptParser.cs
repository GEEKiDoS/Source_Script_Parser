using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SSP
{
    public class SourceScriptParser
    {
        private int rootlevel=0;
        private string _path;
        private struct SectionPair
        {
            public string Section;
            public string Key;
        }
        private readonly Dictionary<SectionPair, string> keyPair = new Dictionary<SectionPair, string>();
        private Dictionary<string, string> uproot = new Dictionary<string, string>();
        
        public void Parser(string path)
        {
            _path = path;
            if (File.Exists(path))
            {
                TextReader reader = new StreamReader(path);
                string strLine = null;
                string currentRoot = null;
                string[] keyPair = null;
                string upLine = null;
                bool isc = false;
                bool isr = false;
                try
                {
                    for (strLine = reader.ReadLine(); strLine != null; strLine = reader.ReadLine())
                    {
                        strLine = strLine.Trim();
                        if (strLine.StartsWith("//"))
                        {
                            continue;
                        }
                        if (strLine != "")
                        {
                            if (rootlevel == 0)
                            {
                                if (strLine.StartsWith("\"") && strLine.EndsWith("\""))
                                {
                                    rootlevel = 1;
                                    currentRoot = strLine.Substring(1, strLine.Length - 2);
                                    isr = true;
                                    continue;
                                }
                            }
                            else
                            {
                                if (strLine.Contains("//"))
                                {
                                    strLine = strLine.Substring(0, strLine.IndexOf("//") - 1);
                                }
                                if (strLine.StartsWith("{") && !isr)
                                {
                                    rootlevel = rootlevel + 1;
                                    uproot.Add(currentRoot + "|" + upLine, currentRoot);
                                    currentRoot = currentRoot+"|"+upLine;
                                    continue;
                                }
                                if (strLine.StartsWith("{") && isr)
                                {
                                    isr = false;
                                    continue;
                                }
                                if (strLine.StartsWith("}"))
                                {
                                    rootlevel = rootlevel-1;
                                    if(rootlevel!=0)
                                    {
                                            currentRoot = uproot[currentRoot];
                                    }
                                    continue;
                                }
                                    strLine = strLine.Replace("	", "");
                                    strLine = strLine.Replace("\"\"", "=");
                                    strLine = strLine.Replace("\"", "");
                                keyPair = strLine.Split(new char[] { '=' }, 2);
                                SectionPair pair;
                                string value = null;

                                if (currentRoot == null)
                                    currentRoot = "ROOT";

                                pair.Section = currentRoot;
                                pair.Key = keyPair[0];

                                if (keyPair.Length > 1)
                                    value = keyPair[1];

                                try
                                {
                                    this.keyPair.Add(pair, value);
                                    upLine = strLine;
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
            else
            {
                throw new FileNotFoundException("Unable to locate " + path);
            }

        }

        public string GetSetting(string sectionName, string settingName)
        {
            try
            {
                SectionPair sectionPair;
                sectionPair.Section = sectionName;
                sectionPair.Key = settingName;
                string setting = keyPair[sectionPair];
                return setting;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void AddSetting(string sectionName, string settingName, string settingValue)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;

            if (keyPair.ContainsKey(sectionPair))
                keyPair.Remove(sectionPair);

            keyPair.Add(sectionPair, settingValue);
        }

        public void AddSetting(string sectionName, string settingName)
        {
            AddSetting(sectionName, settingName, null);
        }

        public void DeleteSetting(string sectionName, string settingName)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;

            if (keyPair.ContainsKey(sectionPair))
                keyPair.Remove(sectionPair);
        }

        public async Task SaveSettings(string filepath)
        {
            List<string> sections = new List<string>();
            string tmpValue = "";
            string strToSave = "";

            foreach (SectionPair sectionPair in keyPair.Keys)
            {
                if (!sections.Contains(sectionPair.Section))
                    sections.Add(sectionPair.Section);
            }

            foreach (string section in sections)
            {
                strToSave += ("\"" + section + "\"\r\n{\r\n");

                foreach (SectionPair sectionPair in keyPair.Keys)
                {
                    if (sectionPair.Section == section)
                    {
                        tmpValue = keyPair[sectionPair];

                        if (tmpValue != null)
                            tmpValue = "\"		\"" + tmpValue + "\"";

                        strToSave += ("	\"" + sectionPair.Key + tmpValue + "\r\n");
                    }
                }

                strToSave += "}\r\n\r\n";
            }

            try
            {
                TextWriter tw = new StreamWriter(filepath);
                await tw.WriteAsync(strToSave);
                tw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void SaveSetting(string filepath)
        {
            await SaveSettings(filepath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConwayNativityDirectory.PluginApi
{
    public class NativityTextParser
    {
        private NativityTextParser(string input)
        {
            this.input = input;
        }

        string input;
        public string Input => input;

        string[][] output;
        public string[][] Output => output;

        public void Parse()
        {
            List<string[]> nativities = new List<string[]>();

            string id = null;
            string title = null;
            string description = null;
            foreach (string line in input.Split('\n'))
            {
                string[] idAndTitle = ExtractIdAndTitle(line);
                if (idAndTitle != null)
                {
                    if (description != null)
                    {
                        nativities.Add(new string[] { id, title, description.TrimEnd('\n') });
                        description = String.Empty;
                    }

                    id = idAndTitle[0];
                    title = idAndTitle[1];
                }

                else
                {
                    if (description == null)
                    {
                        description = String.Empty;
                    }

                    description += line + "\n";
                }
            }

            if (id != null && title != null && description != null)
                nativities.Add(new string[] { id, title, description });

            output = nativities.ToArray();
        }

        string[] ExtractIdAndTitle(string text)
        {
            int startIdSearch = 0;
            if (text.StartsWith("#"))
                startIdSearch = 1;
            else if (text.ToUpper().StartsWith("NO"))
                startIdSearch = 2;
            else if (text.ToUpper().StartsWith("NO."))
                startIdSearch = 3;
            else if (text.Length > 0 && "0123456789".Contains(text[0].ToString()))
                startIdSearch = 0;
            else
                return null;

            string id = string.Empty;
            string text2 = text.Substring(startIdSearch).TrimStart(' ').TrimStart('\t');
            int startTitleSearch = 0;
            foreach (char c in text2)
            {
                string numbers = "0123456789";
                if (numbers.Contains(c.ToString()))
                    id += c.ToString();
                else
                    break;
                startTitleSearch++;
            }
            
            if (id.Length < 1)
                return null;

            string text3 = text2.Substring(startTitleSearch);
            string title = String.Empty;
            if (text3.StartsWith(".") || text3.StartsWith(")"))
                title = text3.Substring(1);
            else if (text3.StartsWith(".)"))
                title = text3.Substring(2);
            else
                title = text3;
            title = title.Trim(' ').Trim('\t').Trim('\n').Trim('\r');

            return new string[] { id, title };
        }

        public static NativityTextParser Create(string input)
        {
            return new NativityTextParser(input);
        }
    }
}

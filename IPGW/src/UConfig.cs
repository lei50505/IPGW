using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IPGW.src
{
    class UConfig
    {

        private static string path = Directory.GetCurrentDirectory() + "/config.txt";

        public static void add(string key, string value, ref string error)
        {
            try
            {
                string[] lines = new string[0];
                if (!File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
                    sw.Close();
                }
                else
                {
                    lines = File.ReadAllLines(path, Encoding.UTF8);
                }
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Split(':')[0].Equals(key))
                    {
                        lines[i] = lines[i].Split(':')[0] + ':' + value;
                        File.WriteAllLines(path, lines);
                        return;
                    }
                }
                string[] newLines = new string[lines.Length + 1];
                for (int i = 0; i < lines.Length; i++)
                {
                    newLines[i] = lines[i];

                }
                newLines[lines.Length] = key + ':' + value;
                File.WriteAllLines(path, newLines);
            }
            catch (Exception e)
            {
                error = e.Message;
            }
        }

        public static string get(string key, ref string error)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return null;
                }
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Split(':')[0].Equals(key))
                    {
                        return lines[i].Split(':')[1];
                    }
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return null;
        }

    }
}

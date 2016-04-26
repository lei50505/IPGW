using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace IPGW.src
{
    class UWeb
    {

        private static string[] post(string url, string data, ref string error)
        {
            try
            {
                //string url = @"http://ipgw.neu.edu.cn:801/srun_portal_pc.php?ac_id=1&";
                //string data = "action=login&ac_id=1&user_ip=&nas_ip=&user_mac=&url=&username=stu_20124702&password=700920&save_me=0";
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                List<string> lines = new List<string>();
                string str = "";
                while ((str = streamReader.ReadLine()) != null)
                {
                    lines.Add(str);
                    Console.WriteLine(str);
                }
                streamReader.Close();
                return lines.ToArray<string>();
            }
            catch (Exception e)
            {
                error = e.Message;
                return new string[0];
            }
        }

        public static string[] connect(string username, string password, ref string error)
        {
            string url = @"http://ipgw.neu.edu.cn:801/srun_portal_pc.php?ac_id=1&";
            string data = "action=login&ac_id=1&user_ip=&nas_ip=&user_mac=&url=&username=" + username + "&password=" + password + "&save_me=0";
            return post(url, data, ref error);
        }

        public static string[] logout(string username, string password, ref string error)
        {
            string url = @"http://ipgw.neu.edu.cn:801/include/auth_action.php";
            string data = "action=logout&username=" + username + "&password=" + password + "&ajax=1";
            return post(url, data, ref error);
        }

    }
}

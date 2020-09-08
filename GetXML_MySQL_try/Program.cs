using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GetXML_MySQL_try
{
	class Program
	{
		static string dbHost = "127.0.0.1";
		static string dbPort = "3306";
		static string dbUser = "parry1233";
		static string dbPass = "parry1233";
		static string dbName = "ccudb";
		static string conn_info = "server=" + dbHost + ";port=" + dbPort + ";user=" + dbUser + ";password=" + dbPass + ";database=" + dbName;
		static void Main(string[] args)
		{
			using (MySqlConnection conn_logIn = new MySqlConnection(conn_info))
			{
				try
				{
					conn_logIn.Open();
					Console.WriteLine("success");

					string checkAccountCMD = "SELECT * FROM course WHERE C_ID = @ID_in";
					MySqlCommand cmd = new MySqlCommand(checkAccountCMD, conn_logIn);
					cmd.Parameters.AddWithValue("@ID_in", "002");
					MySqlDataReader data = cmd.ExecuteReader();

					string xml = "";

					if (data.HasRows)
					{
						while (data.Read())
						{
							xml += data["C_Tag"];
						}
					}

					XmlDocument xmldoc = new XmlDocument();
					xmldoc.LoadXml(xml);
					XmlNodeList allTags = xmldoc.GetElementsByTagName("tag");
					List<string> tag = new List<string>();
					string output = "課程名稱: "+data["C_Name"]+"\n\n所有標籤:";
					foreach(XmlNode childrenNode in allTags)
					{
						tag.Add(childrenNode.InnerText);
						output += "\n" + childrenNode.InnerText;
					}
					Console.WriteLine(output);
				}
				catch (MySql.Data.MySqlClient.MySqlException ex)
				{
					Console.WriteLine("failed");
					switch (ex.Number)
					{
						case 0:
							Console.WriteLine("Unpredicted incident occured. Fail to connect to database.");
							break;
						case 1042:
							Console.WriteLine("IP error. Please check again.");
							break;
						case 1045:
							Console.WriteLine("User account or password error. Please check again");
							break;
					}
				}

				conn_logIn.Close();

				Console.ReadLine();
			}
		}
	}
}

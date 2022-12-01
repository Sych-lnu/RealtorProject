using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtorProject
{
    internal class Program
    {
        static async Task Main()
        {
            ConnectionStringSettingsCollection settings =
                ConfigurationManager.ConnectionStrings;
            var conn = new NpgsqlConnection(settings["DB"].ConnectionString);
            await conn.OpenAsync();
            while (true)
            {
                Console.WriteLine("Choose option:\n" +
                    "1 - List of clients\n" +
                    "2 - Create client\n" +
                    "3 - Update client info\n" +
                    "4 - List of contracts\n" +
                    "5 - Create contract\n" +
                    "6 - Detele contract\n" +
                    "0 - Quit");
                int option;
                option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        {
                            ListClients(conn);
                            break;
                        }
                    case 2:
                        {
                            CreateClient(conn);
                            break;
                        }
                    case 3:
                        {
                            UpdateClient(conn);
                            break;
                        }
                    case 4:
                        {
                            ListContracts(conn);
                            break;
                        }
                    case 5:
                        {
                            CreateContract(conn);
                            break;
                        }
                    case 6:
                        {
                            DeleteContract(conn);
                            break;
                        }
                    case 0:
                        {
                            return;
                        }
                }
            }
        }

        private static async void DeleteContract(NpgsqlConnection conn)
        {
            ListContracts(conn);
            Console.WriteLine("Enter contract id:");
            int id = int.Parse(Console.ReadLine());
            using (var cmd = new NpgsqlCommand("DELETE FROM contracts WHERE contract_id = ($1)", conn))
            {
                cmd.Parameters.AddWithValue(id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private static async void CreateContract(NpgsqlConnection conn)
        {
            ListDwellings(conn);
            Console.WriteLine("enter dwelling id:");
            int dId = int.Parse(Console.ReadLine());
            ListClients(conn);
            Console.WriteLine("enter client id:");
            int cId = int.Parse(Console.ReadLine());
            ListAgents(conn);
            Console.WriteLine("enter agent id:");
            int aId = int.Parse(Console.ReadLine());
            using (var cmd = new NpgsqlCommand("INSERT INTO contracts (dwelling_id,agent_id,client_id) VALUES ($1,$2,$3)", conn))
            {
                cmd.Parameters.AddWithValue(dId);
                cmd.Parameters.AddWithValue(aId);
                cmd.Parameters.AddWithValue(cId);
                await cmd.ExecuteNonQueryAsync();
            }

        }

        private static async void ListContracts(NpgsqlConnection conn)
        {
            Console.WriteLine("List of contracts:");
            using (var cmd = new NpgsqlCommand("SELECT * FROM contracts", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string msg = "ID: " + reader.GetInt32(0) + "\tDwelling ID: " + reader.GetInt32(1) + "\tAgent ID: " + reader.GetInt32(2) + "\tClient ID: " + reader.GetInt32(3);
                    Console.WriteLine(msg);
                }
            }
        }

        private static async void ListDwellings(NpgsqlConnection conn)
        {
            Console.WriteLine("List of dwellings:");
            using (var cmd = new NpgsqlCommand("SELECT * FROM dwellings", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string msg = "ID: " + reader.GetInt32(0) + "\tAdress: " + reader.GetString(1);
                    Console.WriteLine(msg);
                }
            }

        }
        private static async void ListAgents(NpgsqlConnection conn)
        {
            Console.WriteLine("List of agents:");
            using (var cmd = new NpgsqlCommand("SELECT * FROM agents", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string msg = "ID: " + reader.GetInt32(0) + "\tName: " + reader.GetString(1);
                    Console.WriteLine(msg);
                }
            }

        }

        private static async void UpdateClient(NpgsqlConnection conn)
        {
            ListClients(conn);
            Console.WriteLine("enter id to update:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("enter new client name:");
            string name = Console.ReadLine();
            using (var cmd = new NpgsqlCommand("UPDATE clients SET client_name = ($1) WHERE client_id = ($2)", conn))
            {
                cmd.Parameters.AddWithValue(name);
                cmd.Parameters.AddWithValue(id);
                await cmd.ExecuteNonQueryAsync();
            }

        }

        private static async void CreateClient(NpgsqlConnection conn)
        {
            Console.WriteLine("Enter Client Name: ");
            string client_name = Console.ReadLine();
            using (var cmd = new NpgsqlCommand("INSERT INTO clients (client_name) VALUES ($1)", conn))
            {
                cmd.Parameters.AddWithValue(client_name);
                await cmd.ExecuteNonQueryAsync();
            }

        }

        private static async void ListClients(NpgsqlConnection conn)
        {
            Console.WriteLine("List of clients:");
            using (var cmd = new NpgsqlCommand("SELECT * FROM clients", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string msg = "ID: " + reader.GetInt32(0) + "\tName: " + reader.GetString(1);
                    Console.WriteLine(msg);
                }
            }
        }
    }
}

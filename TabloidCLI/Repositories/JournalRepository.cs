using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI
{
    public class JournalRepository : DatabaseConnector/*, IJournal<Journal>*/
    {
        public JournalRepository(string connectionString) : base(connectionString) { }

        public List<Journal> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT  Title,
                                                CreationDate,
                                                Content
                                        FROM    Journal";

                    List<Journal> journals = new List<Journal>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Journal journal = new Journal()
                        {
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                        };
                        journals.Add(journal);
                    }
                    reader.Close();

                    return journals;


                }
            }
        }
    }
}


 

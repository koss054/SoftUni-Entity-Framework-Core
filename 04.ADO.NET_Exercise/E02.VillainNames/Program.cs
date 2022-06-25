namespace E02.VillainNames
{
    using System;
    using System.Text;
    using System.Data.SqlClient;

    class Program
    {
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection 
                = new SqlConnection(Config.ConnectionString);

            int enteredId
                = int.Parse(Console.ReadLine());

            sqlConnection.Open();


            //string result = GetVillainNamesWithMinionsCount(sqlConnection); --Problem 1
            string result = GetVillainWithMinions(sqlConnection, enteredId);
            Console.WriteLine(result);

            sqlConnection.Close();
        }

        private static string GetVillainNamesWithMinionsCount(SqlConnection sqlConnection)
        {
            StringBuilder output
                = new StringBuilder();

            string query = @"  SELECT [v].[Name]
                                    , COUNT([mv].[MinionId]) AS [MinionsCount]
                                 FROM [MinionsVillains] AS [mv]
                                 JOIN [Villains] AS [v]
                                   ON [v].[Id] = [mv].VillainId
                             GROUP BY [v].[Id], [v].[Name]
                               HAVING COUNT([mv].[MinionId]) > 3
                             ORDER BY COUNT([mv].[MinionId])";

            SqlCommand cmd 
                = new SqlCommand(query, sqlConnection);

            using SqlDataReader reader
                = cmd.ExecuteReader();

            while (reader.Read())
            {
                output
                    .AppendLine($"{reader["Name"]} - {reader["MinionsCount"]}");
            }

            return output
                .ToString().TrimEnd();
        }

        private static string GetVillainWithMinions(SqlConnection sqlConnection, int enteredId)
        {
            StringBuilder output = new StringBuilder();

            string villainNameQuery = @"SELECT [Name]
                                          FROM [Villains]
                                         WHERE [Id] = @VillainId";

            using SqlCommand getVillainNameCmd 
                = new SqlCommand(villainNameQuery, sqlConnection);

            getVillainNameCmd
                .Parameters.AddWithValue("@VillainId", enteredId);

            string villainName = (string)getVillainNameCmd.ExecuteScalar();

            if (villainName == null)
            {
                return $"No villain with ID {enteredId} exists in the database.";
            }

            output.AppendLine($"Villain: {villainName}");

            string minionsQuery = @"   SELECT [m].[Name]
                                            , [m].[Age]
                                         FROM [MinionsVillains] AS [mv]
                                    LEFT JOIN [Minions] AS [m]
                                           ON [m].[Id] = [mv].[MinionId]
	                                    WHERE [mv].VillainId = @VillainId
                                     ORDER BY [m].[Name]";

            using SqlCommand getMinionsCmd
                = new SqlCommand(minionsQuery, sqlConnection);

            getMinionsCmd
                .Parameters.AddWithValue("@VillainId", enteredId);

            using SqlDataReader minionsReader 
                = getMinionsCmd.ExecuteReader();

            if (!minionsReader.HasRows)
            {
                output.AppendLine($"(no minions)");
            }
            else
            {
                int rowNum = 1;

                while (minionsReader.Read())
                {
                    output.AppendLine($"{rowNum}. {minionsReader["Name"]} {minionsReader["Age"]}");
                    rowNum++;
                }
            }

            return output
                .ToString().TrimEnd();
        }
    }
}

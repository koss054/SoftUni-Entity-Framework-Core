--Problem 2
  SELECT [v].[Name]
       , COUNT([mv].[MinionId]) AS [MinionsCount]
    FROM [MinionsVillains] AS [mv]
    JOIN [Villains] AS [v]
      ON [v].[Id] = [mv].VillainId
GROUP BY [v].[Id], [v].[Name]
  HAVING COUNT([mv].[MinionId]) > 3
ORDER BY COUNT([mv].[MinionId])

--Problem 3
SELECT [Name]
  FROM [Villains]
 WHERE [Id] = 1

   SELECT [m].[Name]
        , [m].[Age]
     FROM [MinionsVillains] AS [mv]
LEFT JOIN [Minions] AS [m]
       ON [m].[Id] = [mv].[MinionId]
	WHERE [mv].VillainId = 1
 ORDER BY [m].[Name]
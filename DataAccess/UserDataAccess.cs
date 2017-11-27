using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;
using Common;

namespace DataAccess
{
    public class UserDataAccess
	{
		public static void CreateDatabaseAndTables()
		{
			using (var engine = new SqlCeEngine($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Data\\UserData.sdf\"; Password=\"test_password\""))
			{
				if (!engine.Verify())
				{
					engine.CreateDatabase();
					using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Data\\UserData.sdf\"; Password=\"test_password\""))
					{
						connection.Execute(@"REMOVE TABLE TestUser;");
						connection.Execute(@"CREATE TABLE User(
												[UserId] [INT] IDENTITY(1,1) PRIMARY KEY NOT NULL,
												[FirstName] [NVARCHAR](256) NOT NULL,
												[LastName] [NVARCHAR](256) NOT NULL,
												[EmailAddress] [NVARCHAR](256) NOT NULL,
												[CreatedDate] [DATETIME]  NOT NULL DEFAULT(GETDATE()),
												[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()));");
					}
				}
			}
		}

		public static List<User> GetUsers()
		{
			using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Data\\UserData.sdf\"; Password=\"test_password\""))
			{
				return connection.Query<User>("SELECT * FROM TestUser").ToList();
			}
		}

		public static User GetUser(int userId)
		{
			using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Data\\UserData.sdf\"; Password=\"test_password\""))
			{
				return connection.Query<User>("SELECT * FROM TestUser WHERE TestUserId = @TestUserId", new { TestUserId = userId }).SingleOrDefault();
			}
		}

		public static User AddUpdateUser(User user)
		{
			using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Data\\UserData.sdf\"; Password=\"test_password\""))
			{
				if (user.TestUserId == 0)
				{
					connection.Execute(@"INSERT INTO TestUser
										( 
											[FirstName]
											,[LastName]
											,[EmailAddress] 
										)
										VALUES  
										( 
											@FirstName
											,@LastName
											,@EmailAddress
										)"
						, new { user.TestUserId, user.FirstName, user.LastName, user.EmailAddress });

					user.TestUserId = connection.ExecuteScalar<int>("SELECT MAX([TestUserId]) FROM TestUser");
				}
				else
				{
					connection.Execute(@"UPDATE TestUser
                                        SET [FirstName] = @FirstName
                                        ,[LastName] = @LastName
                                        ,[EmailAddress] = @EmailAddress
                                        ,[ModifiedDate] = GETDATE()
                                        WHERE [TestUserId] = @TestUserId"
						, new { user.TestUserId, user.FirstName, user.LastName, user.EmailAddress });
				}

				return GetUser(user.TestUserId);
			}
		}

		public static int DeleteUser(int userId)
		{
			using (var connection = new SqlCeConnection($"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Data\\UserData.sdf\"; Password=\"test_password\""))
			{
				return connection.Execute("DELETE FROM TestUser WHERE TestUserId = @TestUserId", new { TestUserId = userId });
			}
		}
	}
}

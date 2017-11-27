using System.Collections.Generic;
using Common;
using DataAccess;

namespace BusinessLogic
{
    public class UserBusinessLogic
	{
		public static List<User> GetUsers()
		{
			return UserDataAccess.GetUsers();
		}

		public static User GetUser(int userId)
		{
			return UserDataAccess.GetUser(userId);
		}

		public static User AddUpdateUser(User user)
		{
			return UserDataAccess.AddUpdateUser(user);
		}

		public static int DeleteUser(int userId)
		{
			return UserDataAccess.DeleteUser(userId);
		}

		public static void CreateDatabaseAndTables()
		{
			UserDataAccess.CreateDatabaseAndTables();
		}
	}
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class User
	{
		public int TestUserId { get; set; }
		[DisplayName("First Name")]
		[Required]
		public string FirstName { get; set; }
		[DisplayName("Last Name")]
		[Required]
		public string LastName { get; set; }
		[DisplayName("Email Address")]
		[Required]
		public string EmailAddress { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
	}
}

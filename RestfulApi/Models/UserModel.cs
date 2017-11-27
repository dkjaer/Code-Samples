namespace RestfulApi.Models
{
    public class UserModel
    {
        public int Id { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string EmailAddress { get; internal set; }
    }
}
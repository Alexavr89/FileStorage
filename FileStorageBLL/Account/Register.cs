namespace FileStorageBLL.Account
{
    /// <summary>
    /// Model for user registration
    /// </summary>
    public class Register
    {
        public string Email { get; set; }
        public int Year { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

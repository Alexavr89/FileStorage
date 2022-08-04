namespace FileStorageBLL.Account
{
    /// <summary>
    /// Model for role assignment
    /// </summary>
    public class AssignUserToRoles
    {
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}

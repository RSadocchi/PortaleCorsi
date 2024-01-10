namespace TestIdentity
{
    public class MultilevelPasswordValidationItem
    {
        public string? Role { get; set; }
        public string[]? Claims { get; set; }
        public string? SeverityLevel { get; set; }
        public PasswordValidationOptions ValidationOptions { get; set; }
    }
}

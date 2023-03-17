namespace BookGen.Cli
{
    public sealed class ValidationResult
    {
        private readonly List<string> _issues;

        public ValidationResult()
        {
            _issues = new List<string>();
        }

        public void AddIssue(string msg)
        {
            _issues.Add(msg);
        }

        public void AddIssue(bool condition, string msg)
        {
            if (condition)
                _issues.Add(msg);
        }

        public bool IsOk => _issues.Count == 0;

        public static ValidationResult Ok()
        {
            return new ValidationResult();
        }

        public override string ToString()
        {
            return string.Join('\n', _issues);
        }

        public static ValidationResult Error(string message)
        {
            var result = new ValidationResult();
            result.AddIssue(message);
            return result;
        }
    }
}

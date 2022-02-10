namespace WpLoad
{
    /// <summary>
    /// Represents an authentication info for wordpress site
    /// </summary>
    public record SiteInfo
    {
        /// <summary>
        /// Wordpress host name
        /// </summary>
        public string Host { get; init; }
        /// <summary>
        /// Wordpress user name
        /// </summary>
        public string Username { get; init; }
        /// <summary>
        /// App password
        /// </summary>
        public string Password { get; init; }

        public SiteInfo()
        {
            Host = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}

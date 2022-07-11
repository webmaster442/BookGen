//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace WpLoad.Domain
{
    /// <summary>
    /// Represents an authentication info for wordpress site
    /// </summary>
    public record SiteInfo
    {
        /// <summary>
        /// Wordpress host name
        /// </summary>
        public string ApiEndPoint { get; init; }
        /// <summary>
        /// Wordpress user name
        /// </summary>
        public string Username { get; init; }
        /// <summary>
        /// App password
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Disable https cert validation. NOT recomented
        /// </summary>
        [XmlAttribute]
        public bool DisableCertValidation { get; init; }

        public SiteInfo()
        {
            ApiEndPoint = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            DisableCertValidation = false;
        }
    }
}

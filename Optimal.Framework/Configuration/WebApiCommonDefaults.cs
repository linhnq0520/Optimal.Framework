namespace Optimal.Framework.Configuration
{
    public class WebApiCommonDefaults
    {
        public static string LoginName = "LoginName";

        public static string UserName = "UserName";

        public static string UserCode = "UserCode";

        /// <summary>
        /// Gets user key of http context
        /// </summary>
        public static string UserKey => "ApiUser";

        /// <summary>
        /// Gets Claim type
        /// </summary>
        public static string ClaimTypeName => "UserId";

        /// <summary>
        /// Gets the name of the header to be used for security
        /// </summary>
        public static string SecurityHeaderName => "Authorization";

        /// <summary>
        /// Token lifetime in days
        /// </summary>
        public static int TokenLifeTime => 7;

        /// <summary>
        /// The JWT token signature algorithm
        /// </summary>
        public static string JwtSignatureAlgorithm => "HS256";

        /// <summary>
        /// The minimal length of secret key applied to signature algorithm
        /// <remarks>
        /// For HmacSha256 it may be at least 16 (128 bites)
        /// </remarks>
        /// </summary>
        public static int MinSecretKeyLength => 16;
    }
}

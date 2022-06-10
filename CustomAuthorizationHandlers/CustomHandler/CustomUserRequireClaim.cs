#region using
using Microsoft.AspNetCore.Authorization;
#endregion

namespace CustomHandlers.CustomHandler
{
    /// <summary>
    /// class for claim regarding authurization 
    /// </summary>
    public class CustomUserRequireClaim : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public CustomUserRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using System;
using System.DirectoryServices.AccountManagement;
using Umbraco.Cms.Core.Security;

namespace AIC.Portal.Services
{
    public class ActiveDirectoryAuthenticationService : IBackOfficeUserPasswordChecker
    {
        private readonly string _ADDomain;
        private readonly string _ADContainer;
        private readonly ILogger<ActiveDirectoryAuthenticationService> _logger;
        public ActiveDirectoryAuthenticationService(IConfiguration configuration, ILogger<ActiveDirectoryAuthenticationService> logger)
        {
            _ADDomain = configuration.GetValue<string>("ActiveDirectory:Domain") ?? string.Empty;
            _ADContainer = configuration.GetValue<string>("ActiveDirectory:Container") ?? string.Empty;
            _logger = logger;
        }

        public Task<BackOfficeUserPasswordCheckerResult> CheckPasswordAsync(BackOfficeIdentityUser user, string password)
        {
            bool isSuperUser = user.Id.Equals("-1");
            bool isEmptyADConfiguration = string.IsNullOrEmpty(_ADDomain) || string.IsNullOrEmpty(_ADContainer);
            bool isEmptyUserName = user.UserName is null;
            var result = BackOfficeUserPasswordCheckerResult.FallbackToDefaultChecker;

            if (isEmptyADConfiguration) 
            {
                var nullArgument = string.IsNullOrEmpty(_ADDomain) ? nameof(_ADDomain) : nameof(_ADContainer);
                var exceptionMessage = $"Active Directory {nullArgument} configuration value is not Correctly Set";
                var exception = new ArgumentNullException( nullArgument );
                _logger.LogError(exception, exceptionMessage);
            }

            var useDefaultPasswordChecker = isEmptyUserName || isSuperUser || isEmptyADConfiguration;

            if (OperatingSystem.IsWindows() && !useDefaultPasswordChecker)
            {
                try
                {
                    using var context = new PrincipalContext(
                        ContextType.Domain,
                        _ADDomain,
                        _ADContainer,
                        user.UserName,
                        password);

                    result = context.ValidateCredentials(user.UserName, password) ?
                        BackOfficeUserPasswordCheckerResult.ValidCredentials : 
                        BackOfficeUserPasswordCheckerResult.InvalidCredentials;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Connecting to Active Directory Authentication Service caused an Exception, " +
                        "\n Make sure Active Directory Configurations is Correctly set and Active Directory Server is running");
                    result = BackOfficeUserPasswordCheckerResult.FallbackToDefaultChecker;
                }
            }

            return Task.FromResult(result);
        }
    }
}

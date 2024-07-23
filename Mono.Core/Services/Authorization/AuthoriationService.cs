namespace Mono.Core.Authorization
{
    public class AuthorizationService : IMonoAuthorization
    {
        private readonly IRefitClientBuilder<IAuthorizationService> _authorizationService;

        public AuthorizationService(IRefitClientBuilder<IAuthorizationService> authorizationService)
        {
            _authorizationService = authorizationService;
        }
    }
}

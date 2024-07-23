namespace Mono.Core.Miscellaneous
{
    public class MiscellaneousService : IMonoMiscellaneous
    {
        private readonly IRefitClientBuilder<IMiscellaneousService> _miscellaneousService;

        public MiscellaneousService(IRefitClientBuilder<IMiscellaneousService> miscellaneousService)
        {
            _miscellaneousService = miscellaneousService;
        }
    }
}

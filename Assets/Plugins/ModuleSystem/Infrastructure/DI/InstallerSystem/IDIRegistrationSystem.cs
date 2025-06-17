using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IDIRegistrationSystem : IDIRegistratorAPI
    {
        IEnumerable<Type> GetContracts();
        void StopRegistration();
        void ActivateBindings(Context dIRegistrator, bool useDeepBinding);
    }
}

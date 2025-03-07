using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IDIRegistrationSystem : IDIRegistratorAPI
    {
        HashSet<Type> GetContracts();
        void StopRegistration();
        void ActivateBindings(IDIRegistratorAPI dIRegistrator);
    }
}

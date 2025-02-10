using AccessAppUser.Domain.Entities;
using System.Collections.Generic;

namespace AccessAppUser.Application.Builders
{
    public class ProfileBuilderExternal
    {
        private readonly Profile.ProfileBuilder _builder;

        public ProfileBuilderExternal()
        {
            _builder = Profile.Builder(); 
        }

        public ProfileBuilderExternal WithUser(User user)
        {
           if(user == null)
           {
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
           }
           _builder.WithUser(user); 
            return this;
        }

        public ProfileBuilderExternal WithRole(Role role)
        {
            _builder.WithRole(role); 
            return this;
        }

        public ProfileBuilderExternal WithAreaProfiles(List<AreaProfile> areaProfiles)
        {
            _builder.WithAreaProfiles(areaProfiles); 
            return this;
        }

        public Profile Build() => _builder.Build();
    }
}

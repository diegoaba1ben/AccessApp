using AccessAppUser.Domain.Entities;
using System.Collections.Generic;

namespace AccessAppUser.Application.Builders
{
    public class AreaBuilderExternal
    {
        private readonly Area.AreaBuilder _builder;

        public AreaBuilderExternal()
        {
            _builder = Area.Builder(); 
        }

        public AreaBuilderExternal WithName(string name)
        {
            _builder.SetName(name); 
            return this;
        }

        public AreaBuilderExternal WithDescription(string description)
        {
            _builder.SetDescription(description); 
            return this;
        }

        public AreaBuilderExternal WithRoles(List<Role> roles)
        {
            _builder.AddRoles(roles); 
            return this;
        }

        public AreaBuilderExternal WithProfile(Profile profile)
        {
            _builder.AddProfile(profile); 
            return this;
        }

        public Area Build() => _builder.Build();
    }
}
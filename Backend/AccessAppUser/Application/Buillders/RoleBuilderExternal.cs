using AccessAppUser.Domain.Entities;
using System.Collections.Generic;

namespace AccessAppUser.Application.Builders
{
    public class RoleBuilderExternal
    {
        private readonly Role.RoleBuilder _builder;

        public RoleBuilderExternal()
        {
            _builder = Role.Builder(); 
        }

        public RoleBuilderExternal WithName(string name)
        {
            _builder.SetName(name); 
            return this;
        }

        public RoleBuilderExternal WithDescription(string description)
        {
            _builder.SetDescription(description); 
            return this;
        }

        public RoleBuilderExternal WithAreas(List<Area> areas)
        {
            _builder.AddAreas(areas); 
            return this;
        }

        public Role Build() => _builder.Build();
    }
}



using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Builders
{
    public class PermissionBuilderExternal
    {
        private readonly Permission.PermissionBuilder _builder;

        public PermissionBuilderExternal()
        {
            _builder = Permission.Builder();
        }

        public PermissionBuilderExternal WithName(string name)
        {
            _builder.WithName(name);
            return this;
        }

        public PermissionBuilderExternal WithDescription(string description)
        {
            _builder.WithDescription(description);
            return this;
        }

        public Permission Build() => _builder.Build();
    }
}

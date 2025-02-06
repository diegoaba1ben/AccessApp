using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Builders
{
    public class AreaProfileBuilderExternal
    {
        private readonly AreaProfile.AreaProfileBuilder _builder;

        public AreaProfileBuilderExternal()
        {
            _builder = AreaProfile.Builder(); 
        }

        public AreaProfileBuilderExternal WithArea(Area area)
        {
            _builder.WithArea(area); 
            return this;
        }

        public AreaProfileBuilderExternal WithProfile(Profile profile)
        {
            _builder.WithProfile(profile); 
            return this;
        }

        public AreaProfile Build() => _builder.Build();
    }
}

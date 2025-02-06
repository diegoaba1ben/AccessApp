using AccessAppUser.Domain.Entities;
using System;

namespace AccessAppUser.Application.Builders
{
    public class GesPassBuilderExternal
    {
        private readonly GesPass.GesPassBuilder _builder;

        public GesPassBuilderExternal()
        {
            _builder = GesPass.Builder(); 
        }

        public GesPassBuilderExternal WithUser(User user)
        {
            _builder.WithUser(user); 
            return this;
        }

        public GesPassBuilderExternal WithResetToken(string token, DateTime expiration)
        {
            _builder.WithResetToken(token, expiration); 
            return this;
        }

        public GesPassBuilderExternal MarkAsCompleted()
        {
            _builder.MarkAsCompleted(); 
            return this;
        }

        public GesPass Build() => _builder.Build();
    }
}

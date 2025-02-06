using AccessAppUser.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AccessAppUser.Application.Builders
{
    /// <summary>
    /// Clase que permite construir un usuario externo
    /// </summary>
    public class UserBuilderExternal
    {
        private User _user;

        public UserBuilderExternal()
        {
            _user = User.Builder().Build(); // Inicialización correcta
        }

        public UserBuilderExternal WithName(string name)
        {
            _user = User.Builder()
                        .WithName(name)
                        .WithEmail(_user.Email)
                        .WithPassword(_user.Password)
                        .WithProfile(_user.Profile)
                        .WithRoles(_user.Roles)
                        .Build();
            return this;
        }

        public UserBuilderExternal WithEmail(string email)
        {
            _user = User.Builder()
                        .WithName(_user.Name)
                        .WithEmail(email)
                        .WithPassword(_user.Password)
                        .WithProfile(_user.Profile)
                        .WithRoles(_user.Roles)
                        .Build();
            return this;
        }

        public UserBuilderExternal WithPassword(string password)
        {
            _user = User.Builder()
                        .WithName(_user.Name)
                        .WithEmail(_user.Email)
                        .WithPassword(password)
                        .WithProfile(_user.Profile)
                        .WithRoles(_user.Roles)
                        .Build();
            return this;
        }

        public UserBuilderExternal WithProfile(Profile profile)
        {
            _user = User.Builder()
                        .WithName(_user.Name)
                        .WithEmail(_user.Email)
                        .WithPassword(_user.Password)
                        .WithProfile(profile)
                        .WithRoles(_user.Roles)
                        .Build();
            return this;
        }

        public UserBuilderExternal WithRoles(List<Role> roles)
        {
            _user = User.Builder()
                        .WithName(_user.Name)
                        .WithEmail(_user.Email)
                        .WithPassword(_user.Password)
                        .WithProfile(_user.Profile)
                        .WithRoles(roles)
                        .Build();
            return this;
        }

        public User Build() => _user; //  Usuario correctamente construido
    }
}

﻿using Actio.Common.Exeptions;
using Actio.Services.Identity.Domain.DomainServices;
using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;
        //private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            //_jwtHandler = jwtHandler;
        }

        public async Task RegisterAsync(string name, string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new ActioException("email_in_user", $"Email: '{email}' is already in use.");
            }
            user = new User(name, email);
            user.SetPassword(password, _encrypter);
            await _userRepository.AddAsync(user);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ActioException("invalid_credentials", $"Invalid credentials.");
            }
            if (!user.ValidatePassword(password, _encrypter))
            {
                throw new ActioException("invalid_credentials", $"Invalid credentials.");
            }

           // return _jwtHandler.Create(user.Id);
        }
    }
}

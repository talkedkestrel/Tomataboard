﻿using System;
using System.Linq;
using Thalia.Data;
using Thalia.Data.Entities;
using Thalia.Services.Encryption;

namespace Thalia.Services.AccessTokens
{
    public class AccessTokensRepository : IAccessTokensRepository
    {
        private readonly ThaliaContext _context;
        private readonly IEncryptionService _encryptor;

        public AccessTokensRepository(ThaliaContext context, IEncryptionService encryptor)
        {
            _context = context;
            _encryptor = encryptor;
        }

        public void Add(string service, OauthToken token)
        {
            Delete(service);

            _context.Add(new AccessToken()
            {
                Service = service,
                Token = _encryptor.Encrypt(token.Token),
                Secret = _encryptor.Encrypt(token.Secret),
                Created = DateTime.Now,
                Expires = token.Expires,
                SessionHandle = token.SessionHandle,
            });
            _context.SaveChanges();
        }

        public void Delete(string service)
        {
            var token = _context.AccessTokens.FirstOrDefault(x => x.Service == service);
            if (token == null) return;
            _context.Remove(token);
            _context.SaveChanges();
        }

        public AccessToken Find(string service)
        {
            var token = _context.AccessTokens.FirstOrDefault(x => x.Service == service);
            if (token == null) return null;

            token.Token = _encryptor.Decrypt(token.Token);
            token.Secret = _encryptor.Decrypt(token.Secret);
            return null;
        }
    }
}

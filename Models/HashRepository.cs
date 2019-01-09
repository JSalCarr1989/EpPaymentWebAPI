using System;
using EPWebAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using EPWebAPI.Utilities;

namespace EPWebAPI.Models
{
    public class HashRepository : IHashRepository
    {

        private readonly IConfiguration _config;
        private readonly IDbLoggerRepository _dbLoggerRepository;
        private readonly IDbLoggerErrorRepository _dbLoggerErrorRepository;
        

        public HashRepository(IConfiguration config,
                              IDbLoggerErrorRepository dbLoggerErrorRepository,
                              IDbLoggerRepository dbLoggerRepository)
        {
            _config = config;
            _dbLoggerErrorRepository = dbLoggerErrorRepository;
            _dbLoggerRepository = dbLoggerRepository;

        }

        public Hash CreateRequestHash(HashDTO hash)
        {
            string hashString = string.Empty;

            Hash createdHash = null;

            try
            {
                var environmentMpSk = Environment.GetEnvironmentVariable("MpSk", EnvironmentVariableTarget.Machine);

                var _mpsk = !string.IsNullOrEmpty(environmentMpSk)
                                       ? environmentMpSk
                                       : _config["MpSk"];


                 string rawString =  hash.paymentOrder + hash.paymentReference  + hash.paymentAmount;
                 string MpSk = _mpsk;

                 hashString = StaticRequestEP.ComputeSha256Hash(rawString,MpSk);

                 createdHash = new Hash 
                 {
                    hash = hashString
                 };


                _dbLoggerRepository.LogCreateRequestHash(createdHash, hash);
            }
            
            catch(Exception ex)
            {
                _dbLoggerErrorRepository.LogCreateRequestHashError(ex.ToString());
            }

            return createdHash;
        }


    }
}
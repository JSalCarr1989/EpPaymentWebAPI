using System;
using EPWebAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using EPWebAPI.Utilities;
using System.Security.Cryptography;

namespace EPWebAPI.Models
{
    public class HashRepository : IHashRepository
    {

        private readonly IConfiguration _config;
        private readonly IDbLoggerRepository _dbLoggerRepository;
        private readonly IDbLoggerErrorRepository _dbLoggerErrorRepository;
        private readonly IEnvironmentSettingsRepository _environmentSettingsRepository;
        

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
                //var environmentMpSk = Environment.GetEnvironmentVariable("MpSk", EnvironmentVariableTarget.Machine);

                string environmentMpSk = _environmentSettingsRepository.GetMpSK();

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

        public string GetHashStatus(MultiPagosResponsePaymentDTO multipagosResponse)
        {
            return ValidateMultipagosHash(multipagosResponse);
        }


        private string ValidateMultipagosHash(MultiPagosResponsePaymentDTO multipagosResponse)
        {
            bool result = false;
            string hashStatus = null;
            

            try
            {


                string environmentMpSk = _environmentSettingsRepository.GetMpSK();

                var _mpsk = !string.IsNullOrEmpty(environmentMpSk)
                                       ? environmentMpSk
                                       : _config["MpSk"];
                string MpSk = _mpsk;

                var rawData = multipagosResponse.mp_order + multipagosResponse.mp_reference + multipagosResponse.mp_amount + multipagosResponse.mp_authorization;
                var generatedHash = ComputeSha256Hash(rawData, MpSk);

                result = (generatedHash == multipagosResponse.mp_signature) ? true : false;

                hashStatus = (result) ? StaticResponsePaymentProperties.VALID_HASH
                                        : StaticResponsePaymentProperties.INVALID_HASH;

                _dbLoggerRepository.LogHashValidationToDb(multipagosResponse, rawData, generatedHash, hashStatus);

            }
            catch (Exception ex)
            {
                _dbLoggerErrorRepository.LogValidateMultipagosHashError(ex.ToString(), multipagosResponse.mp_reference, multipagosResponse.mp_order);
            }

            return hashStatus;
        }

        private  string ComputeSha256Hash(string rawData, string secret)
        {
            string computedHash = string.Empty;

            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    secret = secret ?? "";
                    var encoding = new System.Text.ASCIIEncoding();
                    byte[] keyByte = encoding.GetBytes(secret);
                    byte[] messageBytes = encoding.GetBytes(rawData);

                    using (var hmacsha256 = new HMACSHA256(keyByte))
                    {
                        byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                        computedHash = ByteToString(hashmessage);
                    }
                }
            }
            catch (Exception ex)
            {
                _dbLoggerErrorRepository.LogCompute256HashError(ex.ToString());
            }

            return computedHash;
        }

        public  string ByteToString(byte[] buff)
        {
            string sbinary = string.Empty;

            try
            {
                for (int i = 0; i < buff.Length; i++)
                {
                    sbinary += buff[i].ToString("X2"); // hex format
                }
            }
            catch (Exception ex)
            {
                _dbLoggerErrorRepository.LogByteToStringError(ex.ToString());
            }

            return (sbinary).ToLower();
        }
    }
}
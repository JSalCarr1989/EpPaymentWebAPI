using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Dapper;
using EPWebAPI.Utilities;

namespace EPWebAPI.Models
{
    public class HashRepository : IHashRepository
    {

        private readonly IConfiguration _config;
        

        public HashRepository(IConfiguration config)
        {
            _config = config;

        }

        public Hash CreateRequestHash(HashDTO hash)
        {
            string hashString = string.Empty;

            try
            {
                var environmentMpSk = Environment.GetEnvironmentVariable("MpSk", EnvironmentVariableTarget.Machine);

                var _mpsk = !string.IsNullOrEmpty(environmentMpSk)
                                       ? environmentMpSk
                                       : _config["MpSk"];


                string rawString =  hash.paymentOrder + hash.paymentReference  + hash.paymentAmount;
                 string MpSk = _mpsk;

                 hashString = StaticRequestEnterprisePayment.ComputeSha256Hash(rawString,MpSk);

                 Hash createdHash = new Hash 
                 {
                    hash = hashString
                 }; 

                 return createdHash;

            }
            
            catch(Exception ex)
            {
               Console.WriteLine(ex.ToString());
               Hash createdHash = new Hash();
               return createdHash;
            } 
        }


    }
}
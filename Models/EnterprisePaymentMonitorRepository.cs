using System;
using System.Collections.Generic;
using EPWebAPI.Interfaces;
using Dapper;
using System.Data;
using System.Linq;

namespace EPWebAPI.Models
{
    public class EnterprisePaymentMonitorRepository : IEnterprisePaymentMonitorRepository
    {
        private readonly IDbConnectionRepository _dbConnectionRepository;
        private readonly IDbConnection _conn;

        public EnterprisePaymentMonitorRepository(IDbConnectionRepository dbConnectionRepository)
        {
            _dbConnectionRepository = dbConnectionRepository;
            _conn = _dbConnectionRepository.CreateDbConnection();
        }


         IEnumerable<EnterprisePayment> IEnterprisePaymentMonitorRepository.GetAllPayments()
        {

            IEnumerable<EnterprisePaymentDTO> resultList = Enumerable.Empty<EnterprisePaymentDTO>();
            List<EnterprisePayment> resultFilter =   new List<EnterprisePayment>();
            EnterprisePayment viewPayment = null;

            EnterprisePayment dummyPayment = new EnterprisePayment
            {
                CuentaFacturacion = "000000000",
                ServiceRequest = "0-0000000000",
                ReferenciaPago = "00000000000000000",
                Monto = 0,
                PaymentDetails = new List<EnterprisePaymentDetail>()

            };

            resultFilter.Append(dummyPayment);
            

            try
            {
                using (_conn)
                {
                    _conn.Open();
                    resultList = _conn.Query<EnterprisePaymentDTO>("SP_MONITOR_GET_PAYMENT_INFO_BY_SERVICE_REQUEST", new { }, commandType: CommandType.StoredProcedure);

                    foreach( var payment in resultList)
                    {
                        var result = resultFilter.Where(p => p.ServiceRequest == payment.ServiceRequest).Count();


                        if (result == 0)
                        {

                            viewPayment = new EnterprisePayment
                            {
                                CuentaFacturacion = payment.CuentaFacturacion,
                                ServiceRequest = payment.ServiceRequest,
                                ReferenciaPago = payment.PaymentReference,
                                Monto = payment.Monto,
                                PaymentDetails = new List<EnterprisePaymentDetail>()

                            };

                            viewPayment.PaymentDetails.Add(new EnterprisePaymentDetail
                            {
                                ResponsePaymentId = payment.ResponsePaymentId,
                                TipoPago = payment.TipoPago,
                                CodigoRespuesta = payment.CodigoRespuesta,
                                MensajeRespuesta = payment.MensajeRespuesta,
                                AutorizacionBancaria = payment.AutorizacionBancaria,
                                IdVentaMultipagos = payment.IdVentaMultipagos,
                                OrigenRespuesta = payment.OrigenRespuesta,
                                EstatusHash = payment.EstatusHash,
                                UltimosCuatroDigitos = payment.UltimosCuatroDigitos,
                                Bin = payment.Bin,
                                TipoTc = payment.TipoTc,
                                BancoEmisor = payment.BancoEmisor,
                                EstatusEnviadoTibco = payment.EstatusEnviadoTibco
                            });


                            resultFilter.Add(viewPayment);

                        }
                        else
                        {
                             viewPayment = resultFilter.Where(p => p.ServiceRequest == payment.ServiceRequest).FirstOrDefault();


                            viewPayment.PaymentDetails.Add(new EnterprisePaymentDetail
                            {
                                ResponsePaymentId = payment.ResponsePaymentId,
                                TipoPago = payment.TipoPago,
                                CodigoRespuesta = payment.CodigoRespuesta,
                                MensajeRespuesta = payment.MensajeRespuesta,
                                AutorizacionBancaria = payment.AutorizacionBancaria,
                                IdVentaMultipagos = payment.IdVentaMultipagos,
                                OrigenRespuesta = payment.OrigenRespuesta,
                                EstatusHash = payment.EstatusHash,
                                UltimosCuatroDigitos = payment.UltimosCuatroDigitos,
                                Bin = payment.Bin,
                                TipoTc = payment.TipoTc,
                                BancoEmisor = payment.BancoEmisor,
                                EstatusEnviadoTibco = payment.EstatusEnviadoTibco
                            });


                        }








                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return resultFilter;

            
        }
    }
}

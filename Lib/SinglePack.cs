using NMVSReceiver.SinglePackReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using NMVSReceiver.Lib;
using System.Diagnostics;

namespace NMVSReceiver.Lib
{
    public class SinglePack
    {
        public static G110Response G110Verify(object body)
        {

            try
            {
                ConfigManagement gestionXML = new ConfigManagement();
                Encrypt crypt = new Encrypt();
                

                //add using directive NMVS_Sample.SinglePackServices;
                //binding the service address you want to contact
                string EndPoint = Properties.Settings.Default.SinglePackServicesEndPoint;

                //Creates a G110Request object and a G110VerifyRequest object
                G110VerifyRequest request = new G110VerifyRequest();
                G110Request g110Request = new G110Request();


                //Executes the SetHeaderRequest method from the header class to fill the header
                object Header = NMVSReceiver.Lib.Header.SetHeaderRequest("NMVSReceiver.SinglePackReference", false);

                //Appends the header and body to the request object
                g110Request.Header = (RequestHeaderData_Type)Header;
                g110Request.Body = (RequestData_Type)body;
                request.G110Request = g110Request;

                //add using directive System.ServiceModel;
                //Defines a secure binding with certificate authentication
                WSHttpBinding binding = new WSHttpBinding();
                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

                //Creates ServiceClient, attach transport-binding, Endpoint and the loaded certificate
                SinglePackServicesClient service = new SinglePackServicesClient(binding, new EndpointAddress(EndPoint));


                //add using directive System.Security.Cryptography.X509Certificates;
                X509Certificate2 cert = new X509Certificate2(gestionXML.GetCertificado(), gestionXML.GetPrivateKey(), X509KeyStorageFlags.PersistKeySet);
                
                service.ClientCredentials.ClientCertificate.Certificate = cert;
                service.Endpoint.EndpointBehaviors.Add(new CustomMessageInspector());

                G110Response response = service.G110Verify(g110Request);

                return response;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Excepción " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Se encarga de gestionar el comportamiento de la solicitud al webservice
        /// </summary>
        internal class CustomMessageInspector : IEndpointBehavior, IClientMessageInspector
        {
            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                clientRuntime.MessageInspectors.Add(this);
            }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {
            }

            public void Validate(ServiceEndpoint endpoint)
            {
            }

            //add using directive Message = System.ServiceModel.Channels.Message;
            public void AfterReceiveReply(ref Message reply, object correlationState)
            {
                reply.Headers.Clear();
                Console.WriteLine("received Response:");
                Console.WriteLine("{0}\r\n", reply);
            }

            /// <summary>
            /// Shows the sent message with and without SOAP-Header
            /// </summary>
            /// <param name="request"></param>
            /// <param name="channel"></param>
            /// <returns></returns>
            public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                HttpRequestMessageProperty header;
                header = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, header);
                header.Headers["Host"] = "https://ws-single-transactions-int-bp.nmvs.eu";
                Console.WriteLine("original Request:");
                Console.WriteLine("{0}\r\n", request);
                request.Headers.Clear();
                Console.WriteLine();
                return null;
            }


        }
    }
}
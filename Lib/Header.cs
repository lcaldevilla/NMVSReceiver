using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMVSReceiver.Lib
{
    public class Header
    {
        /// <summary>
        /// Generates, from the user data specified in the settings, the header for the individual transactions
        /// </summary>
        /// <param name="serviceName">Specify the service name to be used for the header. For example "SinglePackServices"</param>
        /// <returns></returns>
        /// 
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
 (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal static object SetHeaderRequest(string serviceName, bool TrxId)
        {
            try
            {
                ConfigManagement gestionXML = new ConfigManagement();
                Encrypt crypt = new Encrypt();

                Type typeHeaderData = Type.GetType(serviceName + ".RequestHeaderData_Type");
                Type typeAuthHeaderData = Type.GetType(serviceName + ".RequestAuthHeaderData_Type");
                Type typeUserSoftware = Type.GetType(serviceName + ".UserSoftware_Type");

                Type typeTransactionHeaderData = Type.GetType(serviceName + ".RequestTransactionHeaderData_Type");

                dynamic headerData = Activator.CreateInstance(typeHeaderData);
                dynamic authHeaderData = Activator.CreateInstance(typeAuthHeaderData);
                dynamic userSoftware = Activator.CreateInstance(typeUserSoftware);
                dynamic transactionHeaderData = Activator.CreateInstance(typeTransactionHeaderData);

                authHeaderData.ClientLoginId = gestionXML.GetHeaderLogin();
                authHeaderData.UserId = gestionXML.GetHeaderUserId();
                authHeaderData.Password = gestionXML.GetHeaderPassword();

                log.Info("ID: " + gestionXML.GetHeaderLogin() + " ||UserID: " + gestionXML.GetHeaderUserId() + " ||Pass: " + gestionXML.GetHeaderPassword());

                if (TrxId == true)
                {
                    authHeaderData.SubUserId = "06c8dea069c24446949e7806e028bcaf"; //Alternativ: Properties.Settings.Default.HeaderSubUserID;
                }

                headerData.Auth = authHeaderData;
                headerData.Transaction = transactionHeaderData;
                headerData.UserSoftware = userSoftware;

                userSoftware.name = Properties.Settings.Default.HeaderSoftwareName;
                userSoftware.supplier = Properties.Settings.Default.HeaderSoftwareSupplier;
                userSoftware.version = Properties.Settings.Default.HeaderSoftwareVersion;

                //transactionHeaderData.ClientTrxId = "06c8dea069c24446949e7806e028bcaf";  //Alternativ Guid.NewGuid().ToString().Replace("-", "");
                transactionHeaderData.ClientTrxId = Guid.NewGuid().ToString().Replace("-", "");
                transactionHeaderData.Language = Properties.Settings.Default.HeaderLanguage;
                //  Console.WriteLine(transactionHeaderData);
                return headerData;
            }
            catch
            {
                return null;
            }
        }
    }
}
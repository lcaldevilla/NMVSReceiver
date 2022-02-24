using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace NMVSReceiver.Lib
{
    public class ConfigManagement
    {
        DataBase db = new DataBase();
        XmlDocument doc = new XmlDocument();
        string xmlConfiguracion = Server.MapPath("~/images")"configuracion.xml";

        // log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

 
        public string GetHeaderLogin()
        {
            doc.Load(xmlConfiguracion);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/root/NMVS");

            foreach (XmlNode node in nodes)
            {
                return node.SelectSingleNode("HeaderLogin").InnerText;
            }
            return "error";
        }

        public string GetHeaderUserId()
        {
            doc.Load(xmlConfiguracion);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/root/NMVS");

            foreach (XmlNode node in nodes)
            {
                return node.SelectSingleNode("HeaderUserID").InnerText;
            }
            return "error";
        }

        public string GetHeaderPassword()
        {
            doc.Load(xmlConfiguracion);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/root/NMVS");

            foreach (XmlNode node in nodes)
            {
                return node.SelectSingleNode("HeaderPassword").InnerText;
            }
            return "error";
        }

        public string GetCertificado()
        {
            doc.Load(xmlConfiguracion);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/root/NMVS");

            foreach (XmlNode node in nodes)
            {
                return node.SelectSingleNode("Certificado").InnerText;
            }
            return "error";
        }

        public string GetPrivateKey()
        {
            doc.Load(xmlConfiguracion);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/root/NMVS");

            foreach (XmlNode node in nodes)
            {
                return node.SelectSingleNode("PrivateKey").InnerText;
            }
            return "error";
        }
    }
}
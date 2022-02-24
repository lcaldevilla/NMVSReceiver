using NMVSReceiver.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace NMVSReceiver.Controllers
{
    public class BarcodeController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Barcode> Get()
        {
            Console.WriteLine("get:");
            return new[] { new Barcode { GTIN = "0845255", Serial = "ABCEDEF", Batch = "b1256", Expire = "Ex8545" } };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Barcode barcode)
        {
            Debug.WriteLine(AppDomain.CurrentDomain.RelativeSearchPath);
            SinglePackReference.G110Response response;

            SinglePackReference.RequestPack_Type pack = new SinglePackReference.RequestPack_Type();
            pack.sn = barcode.Serial;
            SinglePackReference.ProductIdentifier_Type productCode = new SinglePackReference.ProductIdentifier_Type();
            productCode.scheme = SinglePackReference.CatalogProductSchema_Type.GTIN;
            productCode.Value = barcode.GTIN;
            SinglePackReference.BaseBatch_Type batch = new SinglePackReference.BaseBatch_Type();
            batch.Id = barcode.Batch;
            batch.ExpDate = barcode.Expire;
            SinglePackReference.RequestProduct_Type product = new SinglePackReference.RequestProduct_Type();
            product.ProductCode = productCode;
            product.Batch = batch;

            // falta cumplir el requisito de enviar el codigo nhrn

            SinglePackReference.RequestData_Type body = new SinglePackReference.RequestData_Type();
            body.Pack = pack;
            body.Product = product;

            response = Lib.SinglePack.G110Verify(body);
       
            Debug.WriteLine("JSON " + barcode.GTIN+ " - "+barcode.Serial+" - "+barcode.Batch+" - "+barcode.Expire);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GO;
using GL;

namespace WebAPI.Controllers
{

 /// <summary>
 /// WebAPI /api/Buchung
 /// </summary>
 [Route("api/[controller]")]
 public class BuchungController : Controller
 {
  public JsonResult Post([FromBody]Buchung buchung)
  {
   using (var bm = new BuchungManager())
   {
    string ergebnis = bm.CreateBuchung(buchung.FlugNr, buchung.PassagierID);
    if (ergebnis == "OK")
    {
     var r = new JsonResult("Gebucht!");
     r.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status201Created ;
     return r;
    }
    else
    {
     var r = new JsonResult(ergebnis);
     r.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status406NotAcceptable;
     return r;
    }
   }
  }
 }
}

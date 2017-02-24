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
 /// WebAPI /api/Passagier
 /// </summary>
 /// 
 [Route("api/[controller]")]
 public class PassagierController : Controller
 {

  [HttpGet("{id}")]
  public Passagier Get(int id)
  {
   using (var bm = new PassagierManager())
   {
    return bm.GetPassagier(id);
   }
  }

  [HttpGet("name/{name}")]
  public List<Passagier> Get(string name)
  {
   using (var bm = new PassagierManager())
   {
    return bm.GetPassagierSet(name);
   }
  }

  // POST /api/passagier
  [HttpPost()]
  public JsonResult PostPassagier([FromBody]List<Passagier> passagiere)
  {
   if (passagiere == null) return new JsonResult("Liste darf nicht leer sein!");
   try
   {
    using (PassagierManager bm = new PassagierManager())
    {
     string statistik;
     bm.SavePassagierSet(passagiere, out statistik);

     var r = new JsonResult(statistik);
     this.Response.Headers.Add("X-Status", statistik);
     this.Response.Headers.Add("Location", new Uri("http://localhost:8887/api/passagier/" + passagiere[0].ID).ToString());
     r.StatusCode = 201; // Created
     return r;
    }
   }
   catch (Exception e)
   {
    return new JsonResult(e.ToString());
   }

  }
 }
}

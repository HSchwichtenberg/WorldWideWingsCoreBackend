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
 /// WebAPI /api/Flughafen
 /// </summary>
 /// 
 [Route("api/[controller]")]
 public class FlughafenController : Controller
 {
  [HttpGet()]
  public List<string> Get()
  {
   using (var bm = new FlugManager())
   {
    return bm.GetFlughaefen();
   }
  }
 }
}

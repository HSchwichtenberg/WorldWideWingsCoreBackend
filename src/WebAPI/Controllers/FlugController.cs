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
 /// WebAPI /api/flug
 /// </summary>
[Route("api/[controller]")]
 public class FlugController : Controller
 {

  /// <summary>
  /// http://localhost:8887/api/flug/1
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpGet("{id}")]
  public Flug Get(int id)
  {
   using (var bm = new FlugManager())
   {
    return bm.GetFlug(id);
   }
  }

  /// <summary>
  /// http://localhost:8887/api/flug?abflugort=Paris&zielort=
  /// </summary>
  /// <param name="abflugOrt"></param>
  /// <param name="zielOrt"></param>
  /// <returns></returns>
  [HttpGet("{abflugOrt}/{zielort}")]
  public List<Flug> Get(string abflugOrt, string zielOrt)
  {
   using (var bm = new FlugManager())
   {
    return bm.GetFlugSet(abflugOrt, zielOrt);
   }
  }

  [HttpPost()]
  public void Post(Flug flug)
  {
   using (var bm = new FlugManager())
   {
    bm.New(flug);
   }
  }

 }
}

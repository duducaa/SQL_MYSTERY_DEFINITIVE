﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.DAL;
using Newtonsoft.Json;
using System.Data;

namespace MyProject.BLL.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        [HttpPost(Name = "SelectQuery")]
        public ActionResult<DataTable> SelectQuery([FromBody] string query)
        {
            try
            {
                UserCmdSelect user = new UserCmdSelect();
                DataTable dt = user.GetTable(query);

                return dt != null ? Ok(JsonConvert.SerializeObject(dt)) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("InsertQuery")]
        public ActionResult<Dictionary<string, string>> InsertQuery([FromBody] string query)
        {
            try
            {
                UserCmdInsert user = new UserCmdInsert();
                Dictionary<string, string> response = user.InsertSolution(query);

                string code;
                if (response.TryGetValue("Code", out code))
                {
                    int intCode = Convert.ToInt32(response["Code"]);
                    if (intCode == 1062 && query.Trim().Split(" ")[2].ToUpper() == "TB_SOLUCAO")
                    {
                        response.Clear();
                        response.Add("Message", "Parabéns, você encontrou o assassino!!! Devido a seu ato de bravura e coragem, " +
                                                "o país agora conhece o nome de James Buggy, um dos melhores detetives. " +
                                                "De agora em diante, fique atento ao seu comunicador, porque, com certeza, mais casos aparecerão. Até breve!!!");
                        response.Add("Code", intCode.ToString());
                    }
                }

                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

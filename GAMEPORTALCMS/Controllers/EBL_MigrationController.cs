﻿using GAMEPORTALCMS.Data;
using GAMEPORTALCMS.Models.DTO;
using GAMEPORTALCMS.Models.Entity;
using GAMEPORTALCMS.Models.Response;
using GAMEPORTALCMS.Repository.Implementation;
using iRely.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Twilio;

namespace GAMEPORTALCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EBL_MigrationController : ControllerBase
    {
        private readonly EBL_MigrationRepo eBL_Migration;

        private readonly MailGenerator _mail;

        public EBL_MigrationController(EBL_MigrationRepo bL_MigrationRepo, AppDBContext dbContext, MailGenerator mail)
        {
            eBL_Migration = bL_MigrationRepo;
            _mail = mail;
        }

        #region ListView
        [HttpGet("MigrationList")]
        public IActionResult GetEBL_MigrationList(string? AccountNo, string? status, DateTime? FromDate, DateTime? Todate, string ProductBranch, string? ProductType, string? CIF)
        {
            try
            {
               // var data = eBL_Migration.GetEBLMigrationData(AccountNo, status, FromDate, Todate, ProductBranch, ProductType, CIF);
                var data = eBL_Migration.GetEBLMigrationData(AccountNo, status, FromDate, Todate, ProductBranch, ProductType, CIF);
                
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpGet("MigrationList")]
        //public async Task<IActionResult> GetEBL_MigrationList(string? DocClass, string? status, DateTime? FromDate, DateTime? Todate)
        //{
        //    try
        //    {
        //        var data = eBL_Migration.GetEBLMigrationData(DocClass, status, FromDate, Todate);
        //        return Ok(data);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpGet("EBLPOCList")]
        public IActionResult GetEBL_POCList(string? AccountNo, string? status, DateTime? FromDate, DateTime? Todate, string ProductBranch, string? ProductType, string? CIF)
        {
            try
            {
                var data = eBL_Migration.GetEblPocData(AccountNo, status, FromDate, Todate, ProductBranch, ProductType, CIF);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region Polulate

        //[HttpGet("EblDataClassPopulate")]
        //public async Task<IActionResult> GetEBL_DataClassPopulateList(string? DepartmentId)
        //{
        //    try
        //    {
        //        var data = await eBL_Migration.GetEblDataClassLoad(DepartmentId);
        //        return Ok(data);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpGet("EblProductTypePopulate")]
        public IActionResult GetEBL_ProductTypePopulateList(string? DepartmentId)
        {
            try
            {
                var data = eBL_Migration.GetEblProductTypeLoadSync(DepartmentId); // Call synchronous method
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("EblAccountNoPopulate")]
        public IActionResult GetEBL_AccountNoPopulateList(string? DepartmentId)
        {
            try
            {
                var data = eBL_Migration.GetEblAccountNoLoadSync(DepartmentId); // Call synchronous method
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("EblBranchCodePopulate")]
        public IActionResult GetEBL_BranchCodePopulateList(string? DepartmentId)
        {
            try
            {
                var data = eBL_Migration.GetEblBrachCodeLoadSync(DepartmentId); // Call synchronous method
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("EblCIFPopulate")]
        public IActionResult GetEBL_CIFPopulateList(string? DepartmentId)
        {
            try
            {
                var data = eBL_Migration.GetEblCIFLoadSync(DepartmentId); // Call synchronous method
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //[HttpGet("EblDataClassPopulate")]
        //public IActionResult GetEBL_DataClassPopulateList(string? DepartmentId)
        //{
        //    try
        //    {
        //        var data = eBL_Migration.GetEblDataClassLoadSync(DepartmentId); // Call synchronous method
        //        return Ok(data);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpGet("EblStatusPopulate")]
        public IActionResult GetEBL_StatusPopulateList(string? DepartmentId)
        {
            try
            {
                var data = eBL_Migration.GetEblStatusLoadSync(DepartmentId); // Call synchronous method
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region Graph
        [HttpGet("PIEChart")]
        public ActionResult<Dictionary<string, int>> _Get_Ebl_Migration_Pie_Data(string Department, string type, DateTime? Fromdate, DateTime? Todate)
        {
            try
            {
                if (Department == "1")
                {
                    var data = eBL_Migration.GetPieListSync(type, Fromdate, Todate);
                    return Ok(data);
                }
                else
                {
                    var data = eBL_Migration.GetPieChart_EBl_POCList_Sync(type, Fromdate, Todate);
                    return Ok(data);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("BarChartForDashBoard")]
        public async Task<IActionResult> GetBarChartData(string Department, string type, DateTime? Fromdate, DateTime? Todate)
        {
            try
            {
                if (Department == "1")
                {
                    var data = eBL_Migration.GetEblMigration_BarChart_List(type, Fromdate, Todate);
                    return Ok(data);
                }
                else
                {
                    var data = eBL_Migration.GetEblPOC_BarChart_List(type, Fromdate, Todate);
                    return Ok(data);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        #endregion

        #region Mail_Generator

        [HttpGet("MailGenerator")]
        public async Task<IActionResult> MailGenerator(MailSendDTO jsonData)
        {
            bool data = await _mail.SendMail(jsonData);
            if (data)
            {
                return new JsonResult(new ResponseModel { Success = true, Message = "Mail sent successfully" });
            }
            else
            {
                return new JsonResult(new ResponseModel { Success = false, Message = "error" });
            }         
        }


        //[HttpPost("send-table-data")]
        //public IActionResult ReceiveTableData(string mailAddress, [FromBody] JObject data)
        //{
        //    // Extract table data from request
        //    JArray tableDataArray = (JArray)data["tableData"];
        //    // Convert JSON array to array of objects
        //    var tableData = tableDataArray.ToObject<object[]>();

        //    // Process the table data (e.g., save to database)
        //    // Here, for demonstration, just return a success message
        //    return Ok(new { message = "Table data received successfully" });
        //}


        [HttpPost("MMailGenerator")]
        public IActionResult MMailGenerator(MailSendDTO jsonData)
        {
            try
            {
                // Parse the JSON data received from the client
                //dynamic data = requestData;
                //string inputValue = data.inputValue;

                return Ok("Data received successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

    }
}

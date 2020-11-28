using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Net5Mvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Mvc.Controllers
{
    /// <summary>
    /// 健康检测Api
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
            _logger.LogInformation("health");
        }
        /// <summary>
        /// 首页1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Index()
        {
            return "HealthIndex";
        }
        /// <summary>
        /// 首页2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Index2()
        {
            //var consulList = ConsulProvider.FindConsulServiceList();
            return "Index2";
        }
        [HttpPost]
        public ResultData GetData()
        {
            ResultData result = new ResultData();
            return result;
        }
    }
}

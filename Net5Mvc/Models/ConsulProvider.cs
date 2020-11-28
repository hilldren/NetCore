using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Mvc.Models
{
    public class ConsulProvider
    {
        public static void RegisterConsul()
        {
            var consulClient = new ConsulClient(p => { p.Address = new Uri($"http://127.0.0.1:8500"); });//请求注册的 Consul 地址
                                                                                                         //这里的这个ip 就是本机的ip，这个端口8500 这个是默认注册服务端口 
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//间隔固定的时间访问一次，https://localhost:44381/api/Health
                HTTP = $"https://localhost:44381/Health/Index",//健康检查地址  44308是visualstudio启动的端口
                Timeout = TimeSpan.FromSeconds(5)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = "Net5Mvc",
                Address = "https://localhost/",
                Port = 44381,

            };

            consulClient.Agent.ServiceRegister(registration).Wait();//注册服务 

            //consulClient.Agent.ServiceDeregister(registration.ID).Wait();//registration.ID是guid
            //当服务停止时需要取消服务注册，不然，下次启动服务时，会再注册一个服务。
            //但是，如果该服务长期不启动，那consul会自动删除这个服务，大约2，3分钟就会删了 

        }

        public static List<ConsulServiceModel> FindConsulServiceList()
        {
            List<ConsulServiceModel> consulServiceModelList = new List<ConsulServiceModel>();
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://127.0.0.1:8500"));//请求注册的 Consul 地址
            var ret = consulClient.Agent.Services();

            var allServer = ret.GetAwaiter().GetResult();
            //这个是个dictionary的返回值，他的key是string类型，就是8500/ui上services的instance的id
            var allServerDic = allServer.Response;

            foreach (var Servicedict in allServerDic)
            {
                ConsulServiceModel consulServiceModel = new ConsulServiceModel()
                {
                    name = Servicedict.Value.Service,
                    ip = Servicedict.Value.Address,
                    port = Servicedict.Value.Port,
                };
                consulServiceModelList.Add(consulServiceModel);
            }
            //var test1 = allServerDic.First();
            //string name = test1.Value.Service;//服务名,就是注册的那个test1
            //string serverAddress = test1.Value.Address;
            //int serverPort = test1.Value.Port;
            return consulServiceModelList;
        }


    }

    public class ConsulServiceModel
    {
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
    }
}

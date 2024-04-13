using Microsoft.ServiceFabric.Services.Remoting;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [ServiceContract]
    public interface IStudent : IService
    {
        [OperationContract]
        Task<bool> CheckStudent(StudentSignInDTO studentSignInDTO);
    }
}

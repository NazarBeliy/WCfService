using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WCFSERVICETOMYAPP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        Task<User[]> GettingAllUsersAsync();

        [OperationContract]
        UserOperationResult AddingNewUser(User user);

        [OperationContract]
        void ChargingSomeChanges(User oldUser, User newUser);
        [OperationContract]
        void DeleteUser(User User);
    }
}

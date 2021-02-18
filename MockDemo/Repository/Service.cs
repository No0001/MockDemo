using Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Service : IService
    {
        private IRepository _repository;
        private IUserRepository _userRepository;

        public Service(IRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public void OutputMessage(string message)
        {
            _userRepository.OutputMessage("ffffffffffff");
            _repository.OutputMessage(message);

            var b = _userRepository.Get(message);
        }
    }
}

using Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository : IRepository
    {
        public void OutputMessage(string message)
        {
            Debug.WriteLine(message);
        }
    }
}

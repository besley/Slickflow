using System;
using System.Collections.Generic;
using System.Linq;

namespace Slickflow.Data
{
    public abstract class ManagerBase
    {
        private IRepository _repository;
        public IRepository Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new Repository();
                }
                return _repository;
            }
        }
    }
}

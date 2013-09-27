using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Gateways
{
    public class ItemManageBase : MicroDAQ.Common.IDataItemManage
    {
        public IList<Common.Item> Items
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

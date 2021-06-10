using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstracts.Services
{
    public interface IInventoryService
    {
        void NotifySaleOcurred(int productId, int quantity);
    }
}

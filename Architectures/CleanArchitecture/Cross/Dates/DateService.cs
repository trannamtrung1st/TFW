using System;
using System.Collections.Generic;
using System.Text;

namespace Cross.Dates
{
    public class DateService : IDateService
    {
        public DateTime GetDate()
        {
            return DateTime.Now.Date;
        }
    }
}

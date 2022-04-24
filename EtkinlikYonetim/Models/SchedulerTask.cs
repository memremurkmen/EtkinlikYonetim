using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtkinlikYonetim.Models
{
    public class SchedulerTask : ISchedulerEvent
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
    }
}

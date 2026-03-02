using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_06_TradingService
{
    public class TradingConfig
    {
        public string InputFolder { get; set; } = string.Empty;
        public string ProcessedFolder { get; set; } = string.Empty;
        public int IntervalSeconds { get; set; } = 30;
    }
}

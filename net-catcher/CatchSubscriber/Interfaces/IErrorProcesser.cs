using CatchSubscriber.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchSubscriber.Interfaces;

internal interface IErrorProcesser
{
    Task ProcessError(string message, LogLevel logLevel, List<CatchAction>? actions = null);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDInlämningsuppgift
{
    public enum Command
    {
        timeline,
        post,
        send_message,
        view_messages,
        follow,
        wall,
        log_out
    }
    public enum ResultStatus
    {
        IsSuccess,
        IsFaild,
    }
}

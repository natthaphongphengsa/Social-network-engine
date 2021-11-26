using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDInlämningsuppgift
{
    public enum Command
    {
        timeLine,
        post,
        send_message,
        view_messages,
        timeline,
        follow,
        wall,
        friendlist
    }
    public enum Result
    {
        IsSuccess,
        IsFaild,
    }
}

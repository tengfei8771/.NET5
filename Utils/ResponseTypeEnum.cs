using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public enum ResponseTypeEnum
    {
        [Description("查询数据成功")]
        GetInfoSucess = 2000,
        [Description("操作成功")]
        OperationSucess = 2001,
        [Description("操作失败")]
        OperationFail = 2002,
        [Description("出现异常!")]
        Exception = -1,
        [Description("令牌验证通过")]
        Success,
        [Description("非读取到令牌")]
        NoToken = 50014,
        [Description("非法令牌")]
        IllegalToken = 50015,
        [Description("令牌已经过期！")]
        Overdue = 50016,
        [Description("未定义的验证类型！")]
        UnkonwType = 50017,
        [Description("登录成功")]
        LoginSucess = 50000,
        [Description("账号不存在！")]
        AccountNotExists = 50007,
        [Description("密码错误")]
        PwdError = 50008,
        [Description("账号已经存在")]
        AccountAlreadyExists = 50009,
        [Description("获取Token成功")]
        TokenSucess = 60000,
    }
}

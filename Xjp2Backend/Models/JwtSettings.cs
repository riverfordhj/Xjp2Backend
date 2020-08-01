using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xjp2Backend.Models
{
    public class JwtSettings
    {
        /// <summary>
        /// token是谁颁发的
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// token可以给那些客户端使用
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 加密的key（SecretKey必须大于16个,是大于，不是大于等于）
        /// </summary>
        public string SecretKey { get; set; }
    }
}

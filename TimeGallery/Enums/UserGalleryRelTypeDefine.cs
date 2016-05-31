using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeGallery.Enums
{
    /// <summary>
    /// 用户和相册的关系枚举定义
    /// </summary>
    [Flags]
    public enum UserGalleryRelTypeDefine
    {
        /// <summary>
        /// 相册关注者
        /// </summary>
        Follower = 0x1,

        /// <summary>
        /// 相册管理员
        /// </summary>
        Manager = 0x2,

        /// <summary>
        /// 相册拥有者，拥有最高权限
        /// </summary>
        Owner = 0x4
    }
}
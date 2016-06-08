﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.Models;

namespace TimeGallery.Interfaces
{
    public interface IContentManager : IDependency
    {
        /// <summary>
        /// 增添内容
        /// </summary>
        /// <param name="openId">上传内容的用户</param>
        /// <param name="contentModel">待添加的内容</param>
        /// <param name="errorMsg">失败的错误提示</param>
        /// <returns></returns>
        bool AddContent(string openId, ref ContentModel contentModel, out string errorMsg);
    }
}

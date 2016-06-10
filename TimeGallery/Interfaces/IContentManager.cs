using System;
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

        /// <summary>
        /// 根据阈值智能获取一定量的内容
        /// </summary>
        /// <param name="galleryId">相册Id</param>
        /// <param name="afterDateTime">获取指定时间后的分组，不包括该时间(如果DateTime为最小值则从最新时间开始)</param>
        /// <returns></returns>
        IEnumerable<ContentGroup> SmartGetContentGroups(long galleryId, DateTime afterDateTime);
    }
}

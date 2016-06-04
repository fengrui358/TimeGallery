using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.Enums;
using TimeGallery.Models;

namespace TimeGallery.Interfaces
{
    public interface IGalleryManager : IManagerDependency
    {
        /// <summary>
        /// 根据相册Id获取相册
        /// </summary>
        /// <param name="galleryId">相册Id</param>
        /// <returns>没找到返回null</returns>
        GalleryModel GetGalleryModel(long galleryId);

        /// <summary>
        /// 根据用户OpenId返回对应的相册
        /// </summary>
        /// <param name="openId">用户openId</param>
        /// <param name="userGalleryRelType">用户与相册的关系</param>
        /// <returns></returns>
        IEnumerable<GalleryModel> GetGalleryModels(string openId,
            UserGalleryRelTypeDefine userGalleryRelType = UserGalleryRelTypeDefine.Follower);

        /// <summary>
        /// 注册一个相册
        /// </summary>
        /// <param name="user">相册的拥有者</param>
        /// <param name="galleryModel">待注册的相册信息</param>
        /// <param name="errorMsg">如果失败的错误提示</param>
        /// <returns></returns>
        bool RegisterGalleryModel(UserModel user, ref GalleryModel galleryModel, out string errorMsg);
    }
}

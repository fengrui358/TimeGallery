﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.FastCrud;
using Newtonsoft.Json;
using NLog;
using TimeGallery.DataBase;
using TimeGallery.DataBase.Entity;
using TimeGallery.Enums;
using TimeGallery.Interfaces;
using TimeGallery.Models;
using WebGrease.Css.Extensions;

namespace TimeGallery.Managers
{
    public class GalleryManager : IGalleryManager
    {
        private IConfigurationManager _configurationManager;
        private IUserManager _userManager;
        private ConcurrentDictionary<long, GalleryModel> _galleryModelsDictionary;

        private int FollowerGalleriesMaxCountPerUser
        {
            get
            {
                int followerGalleriesMaxCountPerUser;
                if (int.TryParse(_configurationManager.GetAppSetting("FollowerGalleriesMaxCountPerUser"),
                    out followerGalleriesMaxCountPerUser))
                {
                    return followerGalleriesMaxCountPerUser;
                }
                else
                {
                    LogManager.GetCurrentClassLogger().Error("FollowerGalleriesMaxCountPerUser配置节点出现错误");
                    return 5;
                }
            }
        }

        /// <summary>
        /// 用户拥有的相册
        /// </summary>
        private ConcurrentDictionary<string, IEnumerable<UserGalleryRelDbEntity>> _userGalleries;

        /// <summary>
        /// 相册对应的用户
        /// </summary>
        private ConcurrentDictionary<long, IEnumerable<UserModel>> _galleryUsers;

        public GalleryManager(IConfigurationManager configurationManager, IUserManager userManager)
        {
            _configurationManager = configurationManager;
            _userManager = userManager;
        }

        public void Init()
        {
            LogManager.GetCurrentClassLogger().Info("初始化相册管理器");
            IEnumerable<GalleryModel> galleryModels;

            using (var con = StorageHelper.GetConnection())
            {
                galleryModels = con.Find<GalleryModel>();
            }

            _galleryModelsDictionary =
                new ConcurrentDictionary<long, GalleryModel>(galleryModels.ToDictionary(s => s.Id));

            IEnumerable<UserGalleryRelDbEntity> userGalleryRelDbEntities;
            using (var con = StorageHelper.GetConnection())
            {
                userGalleryRelDbEntities = con.Find<UserGalleryRelDbEntity>();
            }

            #region 找出用户对应的相册

            var userGroup = from userGalleryRel in userGalleryRelDbEntities
                group userGalleryRel by userGalleryRel.OpenId
                into g
                select new {OpenId = g.Key, Galleries = g.OrderByDescending(s => s.UserGalleryRelType)};

            var userGalleries = userGroup.ToDictionary(s => s.OpenId, s => s.Galleries.SelectMany(g =>
            {
                var tempGalleries = new List<UserGalleryRelDbEntity>();
                if (_galleryModelsDictionary.ContainsKey(g.GalleryId))
                {
                    tempGalleries.Add(g);
                }
                else
                {
                    LogManager.GetCurrentClassLogger().Error($"用户OpenId：{g.OpenId}的相册关系中没有找到对应相册：{g.GalleryId}");
                }

                return tempGalleries;
            }));
            _userGalleries = new ConcurrentDictionary<string, IEnumerable<UserGalleryRelDbEntity>>(userGalleries);

            #endregion

            #region 找出相册对应的用户

            var galleryGroup = from userGalleryRel in userGalleryRelDbEntities
                group userGalleryRel by userGalleryRel.GalleryId
                into g
                select new {GalleryId = g.Key, Users = g.OrderByDescending(s => s.UserGalleryRelType)};

            var galleryUsers = galleryGroup.ToDictionary(s => s.GalleryId, s => s.Users.SelectMany(u =>
            {
                var tempUsers = new List<UserModel>();
                var user = _userManager.GetUser(u.OpenId);
                if (user != null)
                {
                    tempUsers.Add(user);
                }
                else
                {
                    LogManager.GetCurrentClassLogger().Error($"相册Id：{u.GalleryId}中没找到对应的用户：{u.OpenId}");
                }

                return tempUsers;
            }));

            _galleryUsers = new ConcurrentDictionary<long, IEnumerable<UserModel>>(galleryUsers);

            #endregion

            LogManager.GetCurrentClassLogger().Info($"相册管理器初始化完毕，当前相册共{_galleryModelsDictionary.Count}本");
        }

        public GalleryModel GetGalleryModel(long galleryId)
        {
            if (_galleryModelsDictionary.ContainsKey(galleryId))
            {
                return _galleryModelsDictionary[galleryId];
            }

            return null;
        }

        public IEnumerable<GalleryModel> GetGalleryModels(string openId,
            UserGalleryRelTypeDefine userGalleryRelType = UserGalleryRelTypeDefine.Follower)
        {
            if (string.IsNullOrEmpty(openId))
            {
                throw new ArgumentNullException(nameof(openId));
            }

            var result = new List<GalleryModel>();
            if (_userGalleries.ContainsKey(openId))
            {
                var userGalleryRels = _userGalleries[openId];
                if (userGalleryRelType == UserGalleryRelTypeDefine.Owner)
                {
                    var galleryIds =
                        userGalleryRels.Where(s => s.UserGalleryRelType == UserGalleryRelTypeDefine.Owner)
                            .Select(s => new {s.GalleryId}).ToList();

                    //一个用户不能拥有超过一个相册
                    if (galleryIds.Count > 1)
                    {
                        throw new Exception($"用户：{openId}拥有来超过一个相册");
                    }

                    if (galleryIds.Any())
                    {
                        result.Add(GetGalleryModel(galleryIds.First().GalleryId));
                    }                    
                }
                else if (userGalleryRelType == UserGalleryRelTypeDefine.Manager)
                {
                    var galleryIds =
                        userGalleryRels.Where(
                            s =>
                                s.UserGalleryRelType == UserGalleryRelTypeDefine.Owner ||
                                s.UserGalleryRelType == UserGalleryRelTypeDefine.Manager)
                            .Select(s => new {s.GalleryId}).ToList();

                    if (galleryIds.Any())
                    {
                        result.AddRange(galleryIds.Select(galleryId => GetGalleryModel(galleryId.GalleryId)));
                    }
                }
                else if (userGalleryRelType == UserGalleryRelTypeDefine.Owner)
                {
                    var galleryIds =
                        userGalleryRels.Where(
                            s =>
                                s.UserGalleryRelType == UserGalleryRelTypeDefine.Owner ||
                                s.UserGalleryRelType == UserGalleryRelTypeDefine.Manager ||
                                s.UserGalleryRelType == UserGalleryRelTypeDefine.Follower)
                            .Select(s => new {s.GalleryId}).ToList();

                    if (galleryIds.Any())
                    {
                        result.AddRange(galleryIds.Select(galleryId => GetGalleryModel(galleryId.GalleryId)));
                    }
                }
            }

            return result;
        }

        public bool RegisterGalleryModel(UserModel user, ref GalleryModel galleryModel, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (user == null || _userManager.GetUser(user.OpenId) == null)
            {
                errorMsg = "系统错误";
                throw new ArgumentNullException(nameof(user));
            }

            if (galleryModel == null || galleryModel.Id > 0)
            {
                errorMsg = "系统错误";
                throw new ArgumentNullException(nameof(galleryModel));
            }

            //先判断是否已注册相册
            if (GetGalleryModels(user.OpenId, UserGalleryRelTypeDefine.Owner).Any())
            {
                errorMsg = "该用户已拥有相册";
                return false;
            }

            if (GetGalleryModels(user.OpenId).Count() >= FollowerGalleriesMaxCountPerUser)
            {
                errorMsg = $"该用户已关注相册数量{FollowerGalleriesMaxCountPerUser}，达到系统上限";
                return false;
            }

            //添加数据库以及内存
            using (var con = StorageHelper.GetConnection())
            {
                #region 向数据库中添加相册及对应关系

                //插入相册信息
                con.Insert(galleryModel);

                var newUserGalleryRelDbEntity = new UserGalleryRelDbEntity
                {
                    GalleryId = galleryModel.Id,
                    OpenId = user.OpenId,
                    UserGalleryRelType = UserGalleryRelTypeDefine.Owner
                };
                con.Insert(newUserGalleryRelDbEntity);

                #endregion

                #region 向内存中添加对应关系

                //向内存中添加相册信息
                if (_galleryModelsDictionary.ContainsKey(galleryModel.Id) ||
                    !_galleryModelsDictionary.TryAdd(galleryModel.Id, galleryModel))
                {
                    throw new Exception(
                        $"内存中添加相册失败，相册信息：{JsonConvert.SerializeObject(galleryModel)}；用户信息：{JsonConvert.SerializeObject(user)}");
                }

                //添加相册对应的用户信息
                if (!_galleryUsers.ContainsKey(galleryModel.Id))
                {
                    if(!_galleryUsers.TryAdd(galleryModel.Id, new List<UserModel> {user}))
                    {
                        throw new Exception(
                            $"内存中添加相册对应用户失败，相册信息：{JsonConvert.SerializeObject(galleryModel)}；用户信息：{JsonConvert.SerializeObject(user)}");
                    }
                }
                else
                {
                    throw new Exception(
                        $"内存中添加相册对应用户失败，新建相册内存中出现重复主键，相册信息：{JsonConvert.SerializeObject(galleryModel)}；用户信息：{JsonConvert.SerializeObject(user)}");
                }

                //添加用户对应的相册信息
                if (!_userGalleries.ContainsKey(user.OpenId))
                {
                    if(!_userGalleries.TryAdd(user.OpenId, new List<UserGalleryRelDbEntity> {newUserGalleryRelDbEntity}))
                    {
                        throw new Exception(
                            $"内存中添加用户对应相册失败，相册信息：{JsonConvert.SerializeObject(galleryModel)}；用户信息：{JsonConvert.SerializeObject(user)}");
                    }
                }
                else
                {
                    //如果用于已关注部分相册，则取出数据更改，注意排序
                    IEnumerable<UserGalleryRelDbEntity> userGalleryRelDbEntities;
                    if (_userGalleries.TryGetValue(user.OpenId, out userGalleryRelDbEntities))
                    {
                        var userGalleryRelDbEntityList = userGalleryRelDbEntities.ToList();
                        userGalleryRelDbEntityList.Insert(0, newUserGalleryRelDbEntity);

                        if (!_userGalleries.TryUpdate(user.OpenId, userGalleryRelDbEntityList, userGalleryRelDbEntities))
                        {
                            throw new Exception(
                            $"内存中添加用户对应相册失败，相册信息：{JsonConvert.SerializeObject(galleryModel)}；用户信息：{JsonConvert.SerializeObject(user)}");
                        }
                    }
                    else
                    {
                        throw new Exception(
                            $"内存中添加用户对应相册失败，相册信息：{JsonConvert.SerializeObject(galleryModel)}；用户信息：{JsonConvert.SerializeObject(user)}");
                    }
                }

                #endregion
            }

            return true;
        }
    }
}
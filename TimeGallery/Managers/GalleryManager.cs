﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.FastCrud;
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
        private IUserManager _userManager;
        private ConcurrentDictionary<long, GalleryModel> _galleryModelsDictionary;

        /// <summary>
        /// 用户拥有的相册
        /// </summary>
        private ConcurrentDictionary<string, IEnumerable<UserGalleryRelDbEntity>> _userGalleries;

        /// <summary>
        /// 相册对应的用户
        /// </summary>
        private ConcurrentDictionary<long, IEnumerable<UserModel>> _galleryUsers;

        public GalleryManager(IUserManager userManager)
        {
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

        public IEnumerable<GalleryModel> GetGalleryModel(string openId,
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
                    if (galleryIds.Count() > 1)
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
    }
}
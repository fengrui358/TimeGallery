using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using TimeGallery.DataBase.Entity;

namespace TimeGallery.Models
{
    /// <summary>
    /// 按日期分组包装
    /// </summary>
    public class ContentWrapperModel
    {
        public DateTime DateTime { get; private set; }

        public IEnumerable<ContentDbEntity> Contents { get; private set; }

        public ContentWrapperModel(DateTime dateTime, IEnumerable<ContentDbEntity> contents)
        {
            try
            {
                if (contents == null)
                {
                    throw new ArgumentNullException(nameof(contents));
                }

                if (dateTime == DateTime.MinValue)
                {
                    throw new ArgumentNullException(nameof(dateTime));
                }

                DateTime = dateTime;
                Contents = contents.OrderByDescending(s => s.CreateTime).ToList();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }
    }
}
using System;

namespace TimeGallery.DataBase.Attributes
{
    /// <summary>
    /// 标注该列为一个数据库特殊列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpecialKeyAttribute : System.Attribute
    {
    }
}
-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.6.16-log - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win32
-- HeidiSQL 版本:                  9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 timegallery 的数据库结构
CREATE DATABASE IF NOT EXISTS `timegallery` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `timegallery`;


-- 导出  表 timegallery.content 结构
CREATE TABLE IF NOT EXISTS `content` (
  `Id` bigint(11) NOT NULL AUTO_INCREMENT,
  `Type` varchar(20) NOT NULL,
  `Url` varchar(128) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Size` varchar(50) NOT NULL,
  `Description` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CreateTime` (`CreateTime`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='内容表';

-- 数据导出被取消选择。


-- 导出  表 timegallery.user 结构
CREATE TABLE IF NOT EXISTS `user` (
  `Uuid` char(50) NOT NULL COMMENT '当前存储随机Id，未来申请下开放平台可替换为真实的微信Uuid',
  `OpenId` char(50) NOT NULL COMMENT '真对该公众号的OpenId',
  `Name` varchar(50) NOT NULL,
  `IsManager` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否是管理员',
  `IsFollower` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否是关注者',
  PRIMARY KEY (`Uuid`),
  UNIQUE KEY `OpenID` (`OpenId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户表，存储凡是关注过公众号的微信号相关信息';

-- 数据导出被取消选择。
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

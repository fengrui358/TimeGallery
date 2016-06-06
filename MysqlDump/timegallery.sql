-- MySQL dump 10.13  Distrib 5.6.16, for Win32 (x86)
--
-- Host: localhost    Database: timegallery
-- ------------------------------------------------------
-- Server version	5.6.16-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `content`
--

DROP TABLE IF EXISTS `content`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `content` (
  `Id` char(36) NOT NULL COMMENT '主键',
  `ContentGroupId` char(36) NOT NULL COMMENT '内容分组的外键',
  `Type` varchar(20) NOT NULL COMMENT 'mime类型',
  `Url` varchar(128) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Size` varchar(50) NOT NULL,
  `Description` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CreateTime` (`CreateTime`),
  KEY `ContentGroupId` (`ContentGroupId`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='内容表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `content`
--

LOCK TABLES `content` WRITE;
/*!40000 ALTER TABLE `content` DISABLE KEYS */;
INSERT INTO `content` VALUES ('5','0','image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1afjh8qb611089eiano18kiqkn9.jpg','2016-04-05 23:39:48','73060',NULL),('4','0','video/quicktime','http://7xrp60.com1.z0.glb.clouddn.com/o_1afjce9gkqrv1lhj1tos51dcad9.mov','2016-04-05 22:15:30','5273396',NULL),('3','0','image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1af6glmpit2u1e0gfah6hv1j2p9.jpg','2016-03-31 22:19:15','1191451',NULL);
/*!40000 ALTER TABLE `content` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contentgroup`
--

DROP TABLE IF EXISTS `contentgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contentgroup` (
  `Id` char(36) NOT NULL COMMENT '主键',
  `GalleryId` bigint(20) NOT NULL COMMENT '所属相册外键',
  `ImageCount` int(11) NOT NULL DEFAULT '0',
  `VideoCount` int(11) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `GalleryId` (`GalleryId`),
  KEY `Date` (`Date`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='按天分组内容，统计一天内容的基本信息';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contentgroup`
--

LOCK TABLES `contentgroup` WRITE;
/*!40000 ALTER TABLE `contentgroup` DISABLE KEYS */;
/*!40000 ALTER TABLE `contentgroup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gallery`
--

DROP TABLE IF EXISTS `gallery`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gallery` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  `Description` varchar(400) DEFAULT '0',
  `Cover` varchar(50) DEFAULT '0',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='相册';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gallery`
--

LOCK TABLES `gallery` WRITE;
/*!40000 ALTER TABLE `gallery` DISABLE KEYS */;
/*!40000 ALTER TABLE `gallery` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `OpenId` char(28) NOT NULL COMMENT '真对该公众号的OpenId',
  `Uuid` char(28) DEFAULT NULL COMMENT '未来申请下开放平台可替换为真实的微信Uuid',
  `OrderNumber` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户的加入平台的时间序号',
  `Name` varchar(50) NOT NULL COMMENT '微信用户自身的昵称',
  `Sex` tinyint(2) NOT NULL DEFAULT '0' COMMENT '用户的性别，值为1时是男性，值为2时是女性，值为0时是未知',
  `City` varchar(50) DEFAULT NULL COMMENT '用户所在城市',
  `Remark` varchar(50) DEFAULT NULL COMMENT '公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注',
  `IsWeixinFollower` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否关注微信公众号',
  `IsManager` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否是管理员',
  `IsFollower` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否是关注者',
  PRIMARY KEY (`OpenId`),
  UNIQUE KEY `Uuid` (`Uuid`),
  KEY `OrderNumber` (`OrderNumber`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='用户表，存储凡是关注过公众号的微信号相关信息';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('oIKlFw0yLVagA1nNfEegqP_2o6Bs',NULL,1,'free',0,NULL,'0','\0','\0','');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_gallery_rel`
--

DROP TABLE IF EXISTS `user_gallery_rel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_gallery_rel` (
  `OpenId` char(28) NOT NULL COMMENT '用户openId',
  `GalleryId` bigint(20) NOT NULL COMMENT '相册Id',
  `UserGalleryRelType` tinyint(4) NOT NULL COMMENT '关系：1为关注者，2为上传管理员，4为相册管理员',
  PRIMARY KEY (`OpenId`,`GalleryId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户和相册的关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_gallery_rel`
--

LOCK TABLES `user_gallery_rel` WRITE;
/*!40000 ALTER TABLE `user_gallery_rel` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_gallery_rel` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-06-06 16:58:59

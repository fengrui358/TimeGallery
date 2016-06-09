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
-- Table structure for table `gallery`
--

DROP TABLE IF EXISTS `gallery`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gallery` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '整型自增主键',
  `Name` varchar(50) NOT NULL COMMENT '相册名称',
  `Description` varchar(400) DEFAULT NULL COMMENT '相册描述',
  `CoverUrl` varchar(50) DEFAULT NULL COMMENT '封面地址',
  `ContentDbHost` varchar(15) NOT NULL COMMENT '内容表所在数据库的主机地址',
  `TotalImageCount` int(11) NOT NULL DEFAULT '0' COMMENT '相册中的图片数量',
  `TotalVideoCount` int(11) NOT NULL DEFAULT '0' COMMENT '相册中的视屏数量',
  `TotalSize` bigint(20) NOT NULL DEFAULT '0' COMMENT '相册总的体积大小',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后一次更新时间，用以判断相册活跃度',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='相册'
/*!50100 PARTITION BY HASH (`Id`)
PARTITIONS 20 */;
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
  PRIMARY KEY (`OpenId`,`OrderNumber`),
  KEY `OrderNumber` (`OrderNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户表，存储凡是关注过公众号的微信号相关信息'
/*!50100 PARTITION BY HASH (`OrderNumber`)
PARTITIONS 100 */;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户和相册的关系表'
/*!50100 PARTITION BY HASH (`GalleryId`)
PARTITIONS 100 */;
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

-- Dump completed on 2016-06-09  9:48:07

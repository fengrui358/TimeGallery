-- MySQL dump 10.13  Distrib 5.6.16, for Win32 (x86)
--
-- Host: localhost    Database: timegallerycontent
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
  `Id` char(36) NOT NULL COMMENT 'Guid主键',
  `ContentGroupId` char(36) NOT NULL COMMENT '内容分组的外键',
  `GalleryId` bigint(20) NOT NULL COMMENT '所属相册外键',
  `Type` varchar(20) NOT NULL COMMENT 'mime类型',
  `Url` varchar(128) NOT NULL COMMENT '内容的网络地址',
  `Size` bigint(20) NOT NULL DEFAULT '0' COMMENT '尺寸大小',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`Id`,`ContentGroupId`,`GalleryId`),
  KEY `CreateTime` (`CreateTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='内容表'
/*!50100 PARTITION BY HASH (`GalleryId`)
PARTITIONS 800 */;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `content`
--

LOCK TABLES `content` WRITE;
/*!40000 ALTER TABLE `content` DISABLE KEYS */;
/*!40000 ALTER TABLE `content` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contentgroup`
--

DROP TABLE IF EXISTS `contentgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contentgroup` (
  `Id` char(36) NOT NULL COMMENT 'Guid主键',
  `GalleryId` bigint(20) NOT NULL COMMENT '所属相册外键',
  `ImageCount` int(11) NOT NULL DEFAULT '0' COMMENT '该分组的图片数量',
  `VideoCount` int(11) NOT NULL DEFAULT '0' COMMENT '该分组的视频数量',
  `TotalSize` bigint(20) NOT NULL DEFAULT '0' COMMENT '该分组内容总体积大小',
  `Date` datetime NOT NULL COMMENT '该分组是哪一天',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`Id`,`GalleryId`),
  KEY `Date` (`Date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='按天分组内容，统计一天内容的基本信息'
/*!50100 PARTITION BY HASH (`GalleryId`)
PARTITIONS 200 */;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contentgroup`
--

LOCK TABLES `contentgroup` WRITE;
/*!40000 ALTER TABLE `contentgroup` DISABLE KEYS */;
/*!40000 ALTER TABLE `contentgroup` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-06-09  9:48:19

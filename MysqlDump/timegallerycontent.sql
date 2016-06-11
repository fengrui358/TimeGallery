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
  `Url` varchar(1024) NOT NULL COMMENT '内容的网络地址',
  `Size` bigint(20) NOT NULL DEFAULT '0' COMMENT '尺寸大小',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`Id`,`ContentGroupId`,`GalleryId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='内容表'
/*!50100 PARTITION BY HASH (`GalleryId`)
PARTITIONS 800 */;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `content`
--

LOCK TABLES `content` WRITE;
/*!40000 ALTER TABLE `content` DISABLE KEYS */;
INSERT INTO `content` VALUES ('1572034a-5b62-4a7e-81dc-53acd69319f0','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk11k0uud41ii51npi1ns8k.jpg',233332,'2016-06-11 11:15:22'),('1b59e706-0992-4895-b4c6-640fefc3492a','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk11fh8fb318ah1n6p10rkq.jpg',206165,'2016-06-11 11:15:24'),('1bc86ecd-9db0-4ee6-8939-b64866f7290e','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk119gnskr1mmc1kgq1op6l.jpg',142820,'2016-06-11 11:15:22'),('2c0e0117-09a8-446b-84f9-8229b635086e','2c0e0117-09a8-486b-84f9-8229b635086e',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1aks5ng8pvs8ncm7pb1nnnesb9.jpg',108176,'2016-06-10 11:29:46'),('32d360bc-5b4c-4061-84e0-b20d87780ca2','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk1mtj1ibc58khtf14esn.jpg',108176,'2016-06-11 11:15:23'),('3ca214e0-3cab-498b-9a30-6d671a79ddd9','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk11boc6b810o16ar1taso.jpg',75315,'2016-06-11 11:15:24'),('58321c54-1fdd-46b6-9d18-47cb74875e31','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk1mad1qcs1uj87d31ip0s.jpg',740034,'2016-06-11 11:15:26'),('746dea38-126a-42e7-8c1c-feaed2c11220','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk11r3qqn1lautso2kp.jpg',247678,'2016-06-11 11:15:24'),('7fc78540-fae2-4405-a04d-3a5b945952aa','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk11os56lcs181sgu1987r.jpg',174498,'2016-06-11 11:15:25'),('8637c611-1d6d-40c3-a12b-da8fc7f4375f','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk11im91n1vj6o1vo714d9m.jpg',173704,'2016-06-11 11:15:23'),('a11702b4-f5ee-41f7-96bc-938c3a13d4b5','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk0pi91v771iul1m3618uj.jpg',192319,'2016-06-11 11:15:21'),('c57604c5-f941-4f1c-a4d5-8cbcec73f351','5f350a54-88df-4803-8dfe-4629b24ca675',1,'image/jpeg','http://7xrp60.com1.z0.glb.clouddn.com/o_1akun9qk16gf1o2nkuli3k1l46t.jpg',352287,'2016-06-11 11:15:26');
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
  `Date` date NOT NULL COMMENT '该分组是哪一天',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
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
INSERT INTO `contentgroup` VALUES ('2c0e0117-09a8-486b-84f9-8229b635086e',1,1,0,108176,'2016-06-10','2016-06-10 11:24:44'),('5f350a54-88df-4803-8dfe-4629b24ca675',1,11,0,2646328,'2016-06-11','2016-06-11 11:12:51');
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

-- Dump completed on 2016-06-11 22:24:43

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
  `CoverUrl` varchar(1024) DEFAULT NULL COMMENT '封面地址',
  `ContentDbHost` varchar(15) NOT NULL COMMENT '内容表所在数据库的主机地址',
  `TotalImageCount` int(11) NOT NULL DEFAULT '0' COMMENT '相册中的图片数量',
  `TotalVideoCount` int(11) NOT NULL DEFAULT '0' COMMENT '相册中的视屏数量',
  `TotalSize` bigint(20) NOT NULL DEFAULT '0' COMMENT '相册总的体积大小',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后一次上传内容时间，用以判断相册活跃度',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='相册'
/*!50100 PARTITION BY HASH (`Id`)
PARTITIONS 20 */;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gallery`
--

LOCK TABLES `gallery` WRITE;
/*!40000 ALTER TABLE `gallery` DISABLE KEYS */;
INSERT INTO `gallery` VALUES (1,'nihao','dfgdgdsgds','http://7xrp60.com1.z0.glb.clouddn.com/o_1akrqmr84a54v043921q9o11i39.jpg','127.0.0.1',12,0,2754504,'2016-06-11 11:15:26','2016-06-10 08:17:33');
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
  `Uuid` varchar(50) DEFAULT NULL COMMENT '未来申请下开放平台可替换为真实的微信Uuid',
  `OrderNumber` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户的加入平台的时间序号',
  `NickName` varchar(50) NOT NULL COMMENT '微信用户自身的昵称',
  `HeadImgUrl` varchar(1024) DEFAULT NULL COMMENT '用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。',
  `Sex` tinyint(2) NOT NULL DEFAULT '0' COMMENT '用户的性别，值为1时是男性，值为2时是女性，值为0时是未知',
  `City` varchar(50) DEFAULT NULL COMMENT '用户所在城市',
  `Province` varchar(50) DEFAULT NULL COMMENT '用户所在省份',
  `Country` varchar(50) DEFAULT NULL COMMENT '用户所在国家',
  `Language` varchar(50) DEFAULT 'zh-CN' COMMENT '用户的语言，zh-CN 简体，zh-TW 繁体，en 英语，默认为zh-CN',
  `Subscribe` tinyint(1) NOT NULL DEFAULT '0' COMMENT '用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息',
  `SubscribeTime` bigint(20) DEFAULT NULL COMMENT '用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间',
  `Remark` varchar(50) DEFAULT NULL COMMENT '公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注',
  `GroupId` int(11) DEFAULT NULL COMMENT '用户所在的分组ID',
  `RegisteredGallery` tinyint(1) DEFAULT '0' COMMENT '是否曾经注册过相册',
  PRIMARY KEY (`OpenId`,`OrderNumber`),
  KEY `OrderNumber` (`OrderNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='用户表，存储凡是关注过公众号的微信号相关信息'
/*!50100 PARTITION BY HASH (`OrderNumber`)
PARTITIONS 100 */;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('oIKlFw0yLVagA1nNfEegqP_2o6Bs',NULL,2,'free','http://wx.qlogo.cn/mmopen/DYAIOgq83eqvVnwn6hbicakLlooXww3Y17UFhITfQ5SCnufT8Ssl3alribZ1VMfuSulY3eibCC13WPJKLJ4nGej0SYicsrw28DP4/0',1,'成都','四川','中国','zh_CN',1,1465450280,'',0,0);
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
INSERT INTO `user_gallery_rel` VALUES ('oIKlFw0yLVagA1nNfEegqP_2o6Bs',1,4);
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

-- Dump completed on 2016-06-11 18:04:53

# 一套基于Abp(服务器端)+Material Admin(AngularJs客户端)的权限管理系统
运行项目：
* 使用VS2013（带Grunt Launcher插件,还需要添加Microsoft.Net.Compilers的引用,否则C#6.0语法编译不过去）或VS2015（带Web Essentials插件）打开项目
* 确保网络通畅还原package.json bower.json以及nuget引用的包
* 数据库已提供，修改web.config的连接字符串指定对应的数据库实例即可
* 登录名：admin 密码 111111

##项目阶段
* 功能模块：用户管理（CRUD、邮箱激活、密码找回、特殊权限设置）、角色管理（CRUD、权限设置）、基本权限管理（CRUD）、审计日志（基本信息列表、查看详情）
* 后台框架：Abp：http://www.aspnetboilerplate.com
* 前台模板：Material Admin：https://wrapbootstrap.com/theme/material-admin-responsive-angularjs-WB011H985

##项目预览
登录界面：
![image](https://github.com/fzrain/ProjectWithAbp/raw/master/doc/Login.png)
首页：
![image](https://github.com/fzrain/ProjectWithAbp/raw/master/doc/Home.png)
用户管理列表
![image](https://github.com/fzrain/ProjectWithAbp/raw/master/doc/UserManager.png)
审计日志及详情：
![image](https://github.com/fzrain/ProjectWithAbp/raw/master/doc/Audit.png)
权限设置
![image](https://github.com/fzrain/ProjectWithAbp/raw/master/doc/PermissionSetting.png)
角色管理
![image](https://github.com/fzrain/ProjectWithAbp/raw/master/doc/Role.png)
##涉及技术
###服务器端
* Asp.Net.Mvc 5.2.3
* EntityFramework 6.1.3
* AutoMapper 4.1.1
* Castle 3.3.3
* AspNet.Identity 2.2.1
* Newtonsoft.Json 7.0.1
* log4net 1.2.10
* Abp 7.4.1

###客户端
* angular 1.4.4
* bootstrap 3.3.5
* jquery 2.1.4
* jstree 3.0.9
* ng-table 0.8.3
* sweetalert 1.0.1
* material-design-iconic-font 2.1.2
* nouislider 8.0.2

##蓝图
现在只是一个雏形，在未来一步一步完善，目标做成一个成熟的平台
计划：
* 合格的单元测试
* 完整的文档
* ASP.NET5(MVC6 EF7)
* 更多的功能模块

<a href='https://shenghuo.alipay.com/send/payment/fill.htm?optEmail=250970574@qq.com&payAmount=20'> <img src='https://img.alipay.com/sys/personalprod/style/mc/btn-index.png' /> </a>

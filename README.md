# Asp.Net Web Template

## 后端

使用 ASP.NET 作为 UI ，EntityFrameworkCore 作为数据访问 

### 1. WebTemplate

UI 项目

### 2. WebTemplate.Application

领域模型(实体、值类型、领域服务)和应用服务

### 3. WebTemplate.Infrastructure

接口的具体技术实现

### 4. WebTemplate.Shared

业务无关的多个项目共享的代码

## 前端

ESM方式开发，核心组件：Vue + Element Plus

## 代码清理

IDE 配置:工具-选项-文本编辑器-代码清理,选中"保存时运行代码清理文件"并确定

Husky.NET 配置:
```sh
dotnet new tool-manifest
dotnet tool install Husky
dotnet husky install
dotnet husky add pre-commit -c "dotnet format && git add ."
```

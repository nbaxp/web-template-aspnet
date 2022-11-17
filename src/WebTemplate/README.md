<https://hasura.io/blog/best-practices-of-using-jwt-with-graphql/>
<https://code.visualstudio.com/docs/containers/quickstart-aspnet-core>
<https://github.com/Microsoft/api-guidelines/blob/vNext/Guidelines.md>
<https://help.aliyun.com/document_detail/25491.html>
<https://cloud.google.com/apis/design/errors>
<https://cloud.baidu.com/doc/BCC/s/Ojwvyo6nc>

# ASP.NET WebAPI

1. 基于 EF Core 定义实体和关系，关系表如采用自动id（不使用外键作为联合主键），则此id不可用于其他表充当外键
1. 启动时生成数据库并初始化
1. 实体类如无控制器则默认映射到泛型控制器
1. 基于 url 或 Accept-Language 确认 locale

## 国际化

1. 设置默认语言
2. 提供语言选择列表，不同语言的 url 不同
3. 和默认语言相同的 url 不包含区域部分

## 实体关系配置

1. 实体使用唯一主键
1. 关联如果不使用外键作为联合主键，在其主键被其他实体用作外键时，只能添加删除，不可编辑，否则会导致数据逻辑问题。如：用户和机构关联时使用自动生成的id，用户和角色关联时，不可以使用该id和角色id进行建模，因为此id对应的机构可能会变化。
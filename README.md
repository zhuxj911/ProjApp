# ProjApp

使用 CommunityToolkit.Mvvm (Microsoft.Toolkit) 架构完成界面的绑定（binding）

2025.11.3
将 PointExtension 修改为 PointExtensions;
增加接口 IPoint，使Azimuth扩展改为针对 IPoint；
将方位角计算中的Azimuth变为 A.Azimuth(B)

将测试变为xUnit；



2026.4.23

添加了UTM Proj 的 实现

去除了WinUI组件，修改为纯WPF的教学程序

将IProj与相应部分 修改为 英文形式 X->n, Y->e   B->lat, L ->lon ， BLtoXY -> Forward, XYtoBL -> Inverse

添加了数据文件，将Ellipsoid的Id修改与pyproj的相对应




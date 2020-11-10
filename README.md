# Ragdoll
用给定的素材实现了一个简单的竞速游戏。玩家操作一个机器人进行车辆选择，并根据选择的车辆进入赛道。驾驶时正上方为耗时，右上方为速度。赛道上有一些障碍物，普通障碍物需要避开，机器人形状的障碍物撞到可以加速，请善加利用。
操作方式：wasd控制移动，鼠标控制镜头，靠近后按F进入载具。
生成的可执行文件位于/IMDT/Game目录下。
代码文件位于/IMDT/Assets/IMDT/Scripts目录下，TimeCount.cs控制上方的计时器，InCar.cs有进行操作模式切换、车辆触边处理、摩擦力实现、UI变化等功能，Collide.cs主要将碰撞发生反馈给InCar.cs处理，此外还修改了控制机器人的BzThirdPersonRigid.cs，使机器人被碰撞时变成布娃娃且车辆加速。
场景位于/IMDT/Assets/IMDT/COURSE/Scenes目录下，为Game.unity文件。

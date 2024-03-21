# Pulse
Pulse is a miniature 2D physics engine developed in C#, tailored for games that do not require realistic physics simulations. The name "Pulse" is derived from its origins as a collision detection library, capturing a nature of physics interactions.<br/>
**Pulse 是用 C# 编写的迷你物理引擎，适用于 2D 且不需要拟真物理效果的游戏。名字取自于“脉冲”，由最初的碰撞检测库扩展而来。**

# Inspiration
[FPPhysics2d](https://github.com/GameArki/GameArkiSetup/tree/main/Assets/com.gamearki.fpphysics2d)

# Readiness
Still in development and iteration, only basic functionalities have been initially completed.Have not been tested in commercial projects. It is not recommended for use in official projects. Suitable for small 2D projects or for reference and learning purposes.<br/>
**初步完成，仍需开发迭代。未经商业项目实验。不建议用于正式项目。可以用于 2D 小型项目或者参考学习。**

# Features
```
======== Legend ========

v Implemented  // 已实现
~ Partially Implemented  // 部分实现
- Planned  // 待实现
* Not In Plan  // 无计划

========================

Dynamics  // 动力学
    ~ Rigid Body Dynamics  // 刚体动力学
        v Translation  // 平动
        - Rotation  // 转动
        * Forces  // 施力

    * Constraint Dynamics  // 约束动力学
    * Soft Body Simulation  // 软体模拟
    * Fluid Simulation  // 流体模拟

Collision Detection
    - Broad Phase  // 粗检测
        - BVH
        - SAP
        - Grids, Quadtrees, etc.
    ~ Narrow Phase  // 精检测
        v SAT
        - GJK (Minkowski), EPA, MPR, etc.
        - CCD (Continuous Collision Detection)

Other Todos
    - OBB vs Sphere Collision Resolution
    - OBB vs OBB Collision Resolution
```

# Samples
```
PhysicalCore core;
float gravity;

void Start() {

    core = new PhysicalCore();
    core.SetGravity(new FVector2(0, gravity));

    var box1 = core.Rigidbody_CreateBox(new FVector2(0, 0), FVector2.one);
    box1.SetIsStatic(true);

    var box2 = core.Rigidbody_CreateBox(new FVector2(0, 2), FVector2.one);
    box2.SetIsStatic(false);
    box1.SetVelocity(new FVector2(0, 1));
    var mat = new PhysicalMaterial();
    mat.SetRestitution(1f);
    box1.SetMaterial(mat);

    var circle1 = core.Rigidbody_CreateCircle(new FVector2(0, 4), 0.5f);
    circle1.SetIsStatic(true);

    var circle2 = core.Rigidbody_CreateCircle(new FVector2(0, 6), 0.5f);
    circle2.SetIsStatic(false);
    circle2.SetVelocity(new FVector2(-1, 1));
    circle2.SetMass(1);

    core.EventCenter.OnCollisionEnterHandle = (a, b) => { Debug.Log($"OnCollisionEnter: {a.ID} {b.ID}"); };
    core.EventCenter.OnCollisionExitHandle = (a, b) => { Debug.Log($"OnCollisionExit: {a.ID} {b.ID}"); };
    core.EventCenter.OnCollisionStayHandle = (a, b) => { Debug.Log($"OnCollisionStay: {a.ID} {b.ID}"); };

    core.EventCenter.OnTriggerEnterHandle = (a, b) => { Debug.Log($"OnTriggerEnter: {a.ID} {b.ID}"); };
    core.EventCenter.OnTriggerExitHandle = (a, b) => { Debug.Log($"OnTriggerExit: {a.ID} {b.ID}"); };
    core.EventCenter.OnTriggerStayHandle = (a, b) => { Debug.Log($"OnTriggerStay: {a.ID} {b.ID}"); };

}

void FixedUpdate() {
    core.Tick(Time.fixedDeltaTime);
}
```

# Project Sample
[Ping_Server](https://github.com/onovich/Ping_Server)

# Dependency
Math library
[Abacus](https://github.com/onovich/Abacus)

# UPM URL
ssh://git@github.com/onovich/Pulse.git?path=/Assets/com.mortise.pulse#main

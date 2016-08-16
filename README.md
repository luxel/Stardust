# Stardust
Stardust is a framework and set of useful libraries built for Unity3D games. The goal of Stardust is to setup a common framwork that can be widely used in different Unity3D game projects, resolve the common problems and chanllenges faced across different projects. Most of its contents are built on top of our experience gained from our past game projects.
Stardust是一个用于打造Unity3D游戏的通用框架，以及一系列非常有用的代码库。Stardust的目标是建立一个可以被不同的基于Unity3D的游戏项目共享的通用游戏框架，并且解决不同的游戏项目中会碰到的一些共同的问题和挑战。Stardust的大多数内容都建立在过往项目中总结的经验之上。

Stardust resolves below problems:
Dependency Injection
Backend service - Communication with backend servers
Config service - centeral management for config files
Text service - setup and management of localized text
Tick service - a global component to execute ticking codes to replace the usage of "Update()"
Log service - a central interface for writing logs
Serialization - best object serialization option for both json and binary
Caching - in memory object caching mechanism
Pooling - simple object pooling for frequently used objects
Object DB - in memory object database for simple query/insert/delete and which can be stored on file system as binary file

Stardust is built on top of below third party open source repositories:

strangeioc
protobufnet
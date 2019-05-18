# unity_tools

自己在项目中使用的一些工具类，为了防止以后找不到，放到github上。里面有一些是自己写的，有的是在网上搜集的，有侵权的，请联系我，我会立刻删掉。随着项目的增加，这个工具类的内容可能会越来越多。

---

## 工程说明

工程文件里分两大文件夹

- **Example** : 示例场景和脚本
- **Scripts** : 工具类核心脚本

    其中所有的示例场景都在Example-Scenes中，我会慢慢补充，所有示例场景用的脚本都在Example-Scripts中

---

## 工具类说明

- **FindChildTool** : 查找子物体，包括子物体的子物体，不管是激活还是未激活状态都能找到，支持泛型查找
- **FolderTools** : 文件夹操作工具，这个工具我的github中有一个专门的案例，详见[这里](https://github.com/lfzl000/Folder.git)
- **GameObjectPool** : 对象池工具，简单好用，一般对象池操作都有，代码很好理解
- **InitRoot** : 初始化根，返回跟物体的第一层子物体
- **MonoSingleton** : 单例，项目中单例模式就靠他，不用在构造函数中声明，哪个脚本想要单例就继承此工具类
- **ProcessTools** : windows进程工具类，可以在程序运行时打开，关闭和查找外部应用进程
- **StringTools** : 字符串工具类，可以很方便的进行字符串各种转码操作
- **TimerTool** : 计时器工具类，很方便好用的计时器
- **TimerTrigger** : 链式计时器工具，功能齐全,调用非常方便. 来源[http://www.manew.com/thread-139640-1-1.html](http://www.manew.com/thread-139640-1-1.html)
- **CutAudioClipNullEditor** : 切除音频文件首尾空白音编辑器工具，使用方法，选中要切除的音效（可多选），点击工具栏 Tools-AudioClip Cut Null即可，会生成一个新的音频文件

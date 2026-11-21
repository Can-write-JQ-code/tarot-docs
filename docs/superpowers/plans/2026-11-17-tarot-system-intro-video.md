# 塔罗 EP04 S.E.E. 直觉读牌教学视频 Implementation Plan

> 历史制作记录。原 EP04 工程、预览成片和教程原稿已移入回收站。文中的路径对应制作时的文件布局。

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 严格根据教程汇编的 EP04 示范脚本，交付一份 15 分钟、1920×1080、带中文旁白和字幕的 S.E.E. 三步直觉读牌教学视频。

**Architecture:** 使用独立 `tarot-see-method-ep04/` HyperFrames 工程，以宝剑二贯穿“白描—入戏—镜像追问”案例；权杖九用于愚人呼吸，星币五用于描述与解释对照。采用黑丝绒桌面、真实牌面局部标注和练习纸，不复用旧视频的内容、Art Deco 风格或版式。

**Tech Stack:** HyperFrames、HTML/CSS/JavaScript、GSAP、Python、Edge TTS、NumPy、FFmpeg。

**Spec:** `塔罗牌系统教程与全资料汇编.md` 第二篇 2.3；`tarot-see-method-ep04/SPEC.md`

## Global Constraints

- 中文旁白、中文字幕，1920×1080，30 fps，总时长 15:00。
- 保留痛点钩子、5 秒愚人呼吸、S.E.E. 三步、税务案例、行动建议与课后打卡。
- 牌图仅使用公有领域的 1910 年 Rider–Waite–Smith 图像。
- 区分可观察事实与解释，不作宿命断言，行动由本人选择。
- 不修改教程原文，不执行 Git 提交。

## Review Focus

- 宝剑二、权杖九、星币五必须使用正确牌图；由素材目检覆盖。
- 观察、解释和行动建议必须分层；由旁白与画面联合审校覆盖。
- 字幕与语音来自同一 WordBoundary 时间轴；由 SRT 时间检查覆盖。
- 互动计时必须有真实停顿；由关键帧与最终 MP4 抽帧覆盖。
- 最终 MP4 必须可完整解码且包含音视频流；由 FFprobe/FFmpeg 验证覆盖。

---

### Task 1: 独立教学内容与视觉工程

- [x] 按文档 EP04 编写 16 段旁白与 15 分钟时间轴。
- [x] 新建 HyperFrames 工程、准备三张牌图与本地字体。
- [x] 编写黑丝绒桌面、练习纸、牌面标注、互动计时和课后模板。

### Task 2: 配音、字幕与预览复核

- [x] 生成全新 Edge 中文配音、合成背景音和 89 组词级字幕。
- [x] 运行 HyperFrames check；修复字号、对比度、可编辑 ID 与重叠问题。
- [x] 查看全部场景中点、关键结束帧和排版修复帧。
- [x] 启动 `http://localhost:3002/#project/tarot-see-method-ep04` 可编辑预览。

### Task 3: 渲染与交付验证

- [ ] 运行 `build.sh` 渲染完整视频并输出响度统一的 H.264/AAC MP4。
- [ ] 用 FFprobe 确认 900 秒、1920×1080、30 fps 和音视频流。
- [ ] 完整解码最终 MP4，测量响度与峰值，检查 SRT 时间单调。
- [ ] 从最终 MP4 抽取各教学阶段画面目检，生成封面并清理临时验证文件。

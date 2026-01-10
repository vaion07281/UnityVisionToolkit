# UnityVisionToolkit 🛠️

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Unity](https://img.shields.io/badge/Unity-6.3%2B-black)](https://unity.com/)

**UnityVisionToolkit** 是一個為 Unity 6.3+ 開發者設計的模組化工具庫，旨在加速獨立遊戲與小型團隊的開發流程。

本專案專注於 2D 專案常用的功能封裝，提供開箱即用的 Utility 與架構方案，採用標準 Unity Package Manager (UPM) 格式，方便跨專案共用。

## 🌟 特色 (Features)

* **Unity 6 Ready**: 針對 Unity 6.3+ 版本環境測試與優化。
* **Core Utilities**: 提供常用的 Extension Methods 與 Helper Class，簡化日常開發。
* **Singleton Pattern**: 內建執行緒安全且支援 `DontDestroyOnLoad` 的泛型單例基類 (MonoSingleton)。
* **Modular Architecture**: 嚴格區分 `Runtime` 與 `Editor` 程式碼，確保打包發布時不會報錯。
* **Zero Dependencies**: 盡量減少外部依賴，保持輕量化。

## 📦 安裝 (Installation)

本套件支援透過 Unity Package Manager 直接安裝。

### 方法一：透過 Git URL 安裝 (推薦)

1. 打開 Unity 6 專案。
2. 前往上方選單 `Window` > `Package Manager`。
3. 點擊視窗左上角的 `+` 號，選擇 **"Add package from git URL..."**。
4. 輸入此 Repository 的 Git 網址：https://github.com/vaion07281/UnityVisionToolkit.git
5. 點擊 **Add**，等待安裝完成。

### 方法二：修改 manifest.json

在你的專案資料夾中找到 `Packages/manifest.json` 檔案，並在 `dependencies`區塊中加入以下內容：

```json
{
"dependencies": {
 "com.vaion07281.unityvisiontoolkit": "https://github.com/vaion07281/UnityVisionToolkit.git",
  ...
}
}
```
## 🚀 使用範例 (Usage)

### 建立單例模式 (Singleton)

只需繼承 `Singleton<T>` 即可自動擁有單例特性：

```csharp
using UnityVisionToolkit.Runtime;

public class GameManager : Singleton<GameManager>
{
    public void StartGame()
    {
        Debug.Log("Game Started!");
    }
}

// 使用時：
// GameManager.Instance.StartGame();
```

## 🤝 貢獻 (Contributing)

歡迎提交 Pull Request 或回報 Issue。開發時請確保遵循專案內的程式碼風格規範。

## 📝 授權 (License)

本專案採用 [MIT License](LICENSE) 授權。
Copyright (c) 2025 vaion07281

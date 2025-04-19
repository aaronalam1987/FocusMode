# 🖥️ Focus Mode – Stay Sharp, One Screen at a Time

Working with multiple monitors is a productivity dream—until it's not. Distractions creep in from every angle, making it hard to zero in on the task at hand. **Focus Mode** helps you reclaim your concentration by blacking out all screens *except* your selected primary display. No unplugging. No fiddling with settings. Just pure focus, on demand.

## ✨ Why not just turn other screens off?

Sure, you *could* power down other displays, but what about fixed setups like built-in laptop screens? Plus, who has time for all that hassle?

---

## ⚙️ Features

- 🖱️ **Tray Icon Interface** – Easily accessible with right-click options.
- 🎯 **Custom Primary Display Selection** – Choose which screen stays active.
- 🌑 **Blacks Out All Other Screens** – Focus like never before.
- 🔄 **Toggle On/Off Anytime** – One click to activate or deactivate.

---

## 🚀 Running the Project from Source

To build and run **Focus Mode** from the source code:

1. **Install Visual Studio 2022** (Community, Professional, or Enterprise).
2. Make sure the following workloads are installed:
   - **.NET Desktop Development**
   - **WPF Development**
3. Clone this repository.
4. Open the `.sln` file in Visual Studio 2022.
5. Include NuGet packages. This project requires the following package:

   **Hardcodet.NotifyIcon.Wpf**   
   You can install it via the NuGet Package Manager UI, or by using the Package Manager Console:
   ````
   Install-Package Hardcodet.NotifyIcon.Wpf
   ````

7. Set the build configuration to Release or Debug.
8. Press F5 or click Start to run the app.

Once running, the app will appear in your system tray.

---

## 📦 Prebuilt Release

Want to skip the build? A fully functional release version is available under the [**Releases**](./releases) section.  
Simply download the latest `.zip`, extract it, and run the executable — no installation required.

---

## 📝 Notes

- Admin permissions may be required depending on your system's display policies.
- The app currently supports **Windows** environments only.

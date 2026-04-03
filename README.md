# Dear ImGui Integration Sample

[**Dear ImGui**](https://github.com/ocornut/imgui) is a fast, minimalistic, and highly portable immediate-mode GUI library primarily used for creating in-game and real-time development tools. It's designed to be simple to integrate into existing applications and is especially popular in the game development, graphics, and visualization communities.

![Dear ImGui](https://documentation-api.unigine.com/en/docs/latest/code/integration/dear_imgui.png)

Rather than creating traditional GUI layouts, **Dear ImGui** allows developers to quickly build dynamic interfaces - perfect for debugging tools, editors, data visualizations, and real-time control panels. It focuses on responsiveness and ease of use, making it ideal for prototyping and tools where performance and simplicity matter.

## How to Run the Sample

### Prerequisites

- [**UNIGINE SDK Browser**](https://developer.unigine.com/en/docs/latest/start/installing_sdk?rlang=cpp) (latest version)
- **UNIGINE SDK Community** or **Engineering** edition (**Sim** upgrade supported)
- **Visual Studio 2022** (recommended)

### Step-by-Step Guide

To get started with the **Dear Imgui C# Sample**:


1. **Clone or download** the sample.

2. **Open SDK Browser** and make sure you have the latest version.

3. **Add the sample project to SDK Browser**:
   - Go to the *My Projects* tab.
   - Click *Add Existing*, select the `.project` file from the cloned folder (matching your OS - `*-win-*`/`*-lin-*`, edition, precision), and click *Import Project*.

     ![Add Project](https://documentation-api.unigine.com/en/docs/latest/sdk/api_samples/third_party/photon/add_project.png)

> [!NOTE]
> If you're using **UNIGINE SDK *Sim***, select the ***Engineering*** `*-eng-sim-*.project` file when importing the sample. After import, you can upgrade the project to the **Sim** version directly in SDK Browser - just click *Upgrade*, choose the SDK **Sim** version, and adjust any additional settings you want to use in the configuration window that opens.

4. **Repair the project**:
   - After importing, you'll see a **Repair** warning - this is expected, as only essential files are stored in the Git repository. SDK Browser will restore the rest.
   
   ![Repair Project](https://documentation-api.unigine.com/en/docs/latest/sdk/api_samples/third_party/repair_project.png)
   - Click *Repair* and then *Configure Project*.

5. **Open the project in Visual Studio**
   - Launch **Visual Studio 2022** and open the `.sln` file
   - If a warning appears under `Dependencies -> Assemblies -> ImGui.NET`, you’ll need to install the required package.

6. **Install the required NuGet package**
   - In Visual Studio, go to **Tools → NuGet Package Manager → Manage NuGet Packages for Solution...**
   - Search for and install `ImGui.NET` (version **1.86.0**)
     
![NuGet Setup](https://documentation-api.unigine.com/en/docs/latest/sdk/api_samples/third_party/imgui_nuget_setup.png)

7. **Build** and **Run** the project.
  
### If the sample fails to run:
  - Double-check all setup steps above to make sure nothing was skipped.
  - Ensure the correct `.project` file is used for your platform and SDK edition.
  - Make sure your SDK version is not older than the project's specified version.
  - Verify `ImGui.NET` is installed via NuGet.
  - If build errors occur, try right-clicking the project and selecting **Rebuild**.
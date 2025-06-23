using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unigine;
using ImGuiNET;

namespace UnigineApp
{
    class AppSystemLogic : SystemLogic
    {
        private Texture custom_texture;

        public override bool Init()
        {
            Engine.BackgroundUpdate = Engine.BACKGROUND_UPDATE.BACKGROUND_UPDATE_RENDER_NON_MINIMIZED;
            Unigine.Console.Run("world_load data/imgui");

            // create texture
            custom_texture = new Texture();
            custom_texture.Load("core/textures/common/checker_d.texture");

            // initialize ImGui backend, set up the Engine
            ImGuiImpl.Init();
            return true;
        }

        public override bool Update()
        {
            EngineWindow main_window = WindowManager.MainWindow;
            if (main_window == null)
            {
                Engine.Quit();
                return true;
            }

            ImGuiImpl.NewFrame();

            // feel free to use ImGui API here...
            ImGui.ShowDemoWindow();

            // show custom texture with ImGui
            ImGui.Begin("Texture Test");
            ImGui.Image(custom_texture.GetPtr(), new System.Numerics.Vector2(custom_texture.GetWidth(), custom_texture.GetHeight()));
            ImGui.End();

            // implementation render
            ImGuiImpl.Render();
            return true;
        }

        public override bool Shutdown()
        {
            // shutdown ImGui backend
            ImGuiImpl.Shutdown();

            return true;
        }
    }
}

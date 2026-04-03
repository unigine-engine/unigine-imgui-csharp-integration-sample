using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Unigine;
using ImGuiNET;
using System.Numerics;

public class ImGuiImpl
{
	static Texture font_texture;
	static MeshDynamic imgui_mesh;
	static Material imgui_material;
	static ImDrawDataPtr frame_draw_data;
	static ImGuiKey[] keymap = new ImGuiKey[(int)Input.KEY.NUM_KEYS];
	static Input.MOUSE_HANDLE prev_mouse_handle;

	struct StyleSizes
	{
		public Vector2 WindowPadding;
		public float WindowRounding;
		public Vector2 WindowMinSize;
		public float ChildRounding;
		public float PopupRounding;
		public Vector2 FramePadding;
		public float FrameRounding;
		public Vector2 ItemSpacing;
		public Vector2 ItemInnerSpacing;
		public Vector2 CellPadding;
		public Vector2 TouchExtraPadding;
		public float IndentSpacing;
		public float ColumnsMinSpacing;
		public float ScrollbarSize;
		public float ScrollbarRounding;
		public float GrabMinSize;
		public float GrabRounding;
		public float LogSliderDeadzone;
		public float TabRounding;
		public float TabMinWidthForCloseButton;
		public Vector2 DisplayWindowPadding;
		public Vector2 DisplaySafeAreaPadding;
		public float MouseCursorScale;
	}

	static StyleSizes sourceSizes = new StyleSizes();
	static float lastScale = 1.0f;

	static EventConnections connections = new EventConnections();

	public static void Init()
	{
		prev_mouse_handle = Input.MouseHandle;

		ImGui.CreateContext();

		Input.EventKeyDown.Connect(connections, on_key_pressed);
		Input.EventKeyUp.Connect(connections, on_key_released);
		Input.EventMouseDown.Connect(connections, on_button_pressed);
		Input.EventMouseUp.Connect(connections, on_button_released);
		Input.EventTextPress.Connect(connections, on_unicode_key_pressed);
		Engine.EventBeginRender.Connect(connections, before_render_callback);
		Unigine.Render.EventEndScreen.Connect(connections, draw_callback);

		var io = ImGui.GetIO();
		io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;
		io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

        keymap[(int)Input.KEY.ESC] = ImGuiKey.Escape;
        keymap[(int)Input.KEY.F1] = ImGuiKey.F1;
        keymap[(int)Input.KEY.F2] = ImGuiKey.F2;
        keymap[(int)Input.KEY.F3] = ImGuiKey.F3;
        keymap[(int)Input.KEY.F4] = ImGuiKey.F4;
        keymap[(int)Input.KEY.F5] = ImGuiKey.F5;
        keymap[(int)Input.KEY.F6] = ImGuiKey.F6;
        keymap[(int)Input.KEY.F7] = ImGuiKey.F7;
        keymap[(int)Input.KEY.F8] = ImGuiKey.F8;
        keymap[(int)Input.KEY.F9] = ImGuiKey.F9;
        keymap[(int)Input.KEY.F10] = ImGuiKey.F10;
        keymap[(int)Input.KEY.F11] = ImGuiKey.F11;
        keymap[(int)Input.KEY.F12] = ImGuiKey.F12;
        keymap[(int)Input.KEY.PRINTSCREEN] = ImGuiKey.None;
        keymap[(int)Input.KEY.SCROLL_LOCK] = ImGuiKey.None;
        keymap[(int)Input.KEY.PAUSE] = ImGuiKey.None;
        keymap[(int)Input.KEY.BACK_QUOTE] = ImGuiKey.None;
        keymap[(int)Input.KEY.DIGIT_1] = ImGuiKey._1;
        keymap[(int)Input.KEY.DIGIT_2] = ImGuiKey._2;
        keymap[(int)Input.KEY.DIGIT_3] = ImGuiKey._3;
        keymap[(int)Input.KEY.DIGIT_4] = ImGuiKey._4;
        keymap[(int)Input.KEY.DIGIT_5] = ImGuiKey._5;
        keymap[(int)Input.KEY.DIGIT_6] = ImGuiKey._6;
        keymap[(int)Input.KEY.DIGIT_7] = ImGuiKey._7;
        keymap[(int)Input.KEY.DIGIT_8] = ImGuiKey._8;
        keymap[(int)Input.KEY.DIGIT_9] = ImGuiKey._9;
        keymap[(int)Input.KEY.DIGIT_0] = ImGuiKey._0;
        keymap[(int)Input.KEY.MINUS] = ImGuiKey.Minus;
        keymap[(int)Input.KEY.EQUALS] = ImGuiKey.Equal;
        keymap[(int)Input.KEY.BACKSPACE] = ImGuiKey.Backspace;
        keymap[(int)Input.KEY.TAB] = ImGuiKey.Tab;
        keymap[(int)Input.KEY.Q] = ImGuiKey.Q;
        keymap[(int)Input.KEY.W] = ImGuiKey.W;
        keymap[(int)Input.KEY.E] = ImGuiKey.E;
        keymap[(int)Input.KEY.R] = ImGuiKey.R;
        keymap[(int)Input.KEY.T] = ImGuiKey.T;
        keymap[(int)Input.KEY.Y] = ImGuiKey.Y;
        keymap[(int)Input.KEY.U] = ImGuiKey.U;
        keymap[(int)Input.KEY.I] = ImGuiKey.I;
        keymap[(int)Input.KEY.O] = ImGuiKey.O;
        keymap[(int)Input.KEY.P] = ImGuiKey.P;
        keymap[(int)Input.KEY.LEFT_BRACKET] = ImGuiKey.LeftBracket;
        keymap[(int)Input.KEY.RIGHT_BRACKET] = ImGuiKey.RightBracket;
        keymap[(int)Input.KEY.ENTER] = ImGuiKey.Enter;
        keymap[(int)Input.KEY.CAPS_LOCK] = ImGuiKey.CapsLock;
        keymap[(int)Input.KEY.A] = ImGuiKey.A;
        keymap[(int)Input.KEY.S] = ImGuiKey.S;
        keymap[(int)Input.KEY.D] = ImGuiKey.D;
        keymap[(int)Input.KEY.F] = ImGuiKey.F;
        keymap[(int)Input.KEY.G] = ImGuiKey.G;
        keymap[(int)Input.KEY.H] = ImGuiKey.H;
        keymap[(int)Input.KEY.J] = ImGuiKey.J;
        keymap[(int)Input.KEY.K] = ImGuiKey.K;
        keymap[(int)Input.KEY.L] = ImGuiKey.L;
        keymap[(int)Input.KEY.SEMICOLON] = ImGuiKey.Semicolon;
        keymap[(int)Input.KEY.QUOTE] = ImGuiKey.None;
        keymap[(int)Input.KEY.BACK_SLASH] = ImGuiKey.Backslash;
        keymap[(int)Input.KEY.LEFT_SHIFT] = ImGuiKey.LeftShift;
        keymap[(int)Input.KEY.LESS] = ImGuiKey.None;
        keymap[(int)Input.KEY.Z] = ImGuiKey.Z;
        keymap[(int)Input.KEY.X] = ImGuiKey.X;
        keymap[(int)Input.KEY.C] = ImGuiKey.C;
        keymap[(int)Input.KEY.V] = ImGuiKey.V;
        keymap[(int)Input.KEY.B] = ImGuiKey.B;
        keymap[(int)Input.KEY.N] = ImGuiKey.N;
        keymap[(int)Input.KEY.M] = ImGuiKey.M;
        keymap[(int)Input.KEY.COMMA] = ImGuiKey.Comma;
        keymap[(int)Input.KEY.DOT] = ImGuiKey.Comma;
        keymap[(int)Input.KEY.SLASH] = ImGuiKey.None;
        keymap[(int)Input.KEY.RIGHT_SHIFT] = ImGuiKey.RightShift;
        keymap[(int)Input.KEY.LEFT_CTRL] = ImGuiKey.LeftCtrl;
        keymap[(int)Input.KEY.LEFT_CMD] = ImGuiKey.LeftSuper;
        keymap[(int)Input.KEY.LEFT_ALT] = ImGuiKey.LeftAlt;
        keymap[(int)Input.KEY.SPACE] = ImGuiKey.Space;
        keymap[(int)Input.KEY.RIGHT_ALT] = ImGuiKey.RightAlt;
        keymap[(int)Input.KEY.RIGHT_CMD] = ImGuiKey.RightSuper;
        keymap[(int)Input.KEY.MENU] = ImGuiKey.None;
        keymap[(int)Input.KEY.RIGHT_CTRL] = ImGuiKey.RightCtrl;
        keymap[(int)Input.KEY.INSERT] = ImGuiKey.Insert;
        keymap[(int)Input.KEY.DELETE] = ImGuiKey.Delete;
        keymap[(int)Input.KEY.HOME] = ImGuiKey.Home;
        keymap[(int)Input.KEY.END] = ImGuiKey.End;
        keymap[(int)Input.KEY.PGUP] = ImGuiKey.PageUp;
        keymap[(int)Input.KEY.PGDOWN] = ImGuiKey.PageDown;
        keymap[(int)Input.KEY.UP] = ImGuiKey.UpArrow;
        keymap[(int)Input.KEY.LEFT] = ImGuiKey.LeftArrow;
        keymap[(int)Input.KEY.DOWN] = ImGuiKey.DownArrow;
        keymap[(int)Input.KEY.RIGHT] = ImGuiKey.RightArrow;
        keymap[(int)Input.KEY.NUM_LOCK] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIVIDE] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_MULTIPLY] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_MINUS] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_7] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_8] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_9] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_PLUS] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_4] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_5] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_6] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_1] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_2] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_3] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_ENTER] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DIGIT_0] = ImGuiKey.None;
        keymap[(int)Input.KEY.NUMPAD_DOT] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_SHIFT] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_CTRL] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_ALT] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_CMD] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_UP] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_LEFT] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_DOWN] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_RIGHT] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_ENTER] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_DELETE] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_INSERT] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_HOME] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_END] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_PGUP] = ImGuiKey.None;
        keymap[(int)Input.KEY.ANY_PGDOWN] = ImGuiKey.None;

		create_font_texture();
		create_imgui_mesh();
		create_imgui_material();

		{
			ImGui.StyleColorsDark();
			var style = ImGui.GetStyle();

			sourceSizes.WindowPadding = style.WindowPadding;
			sourceSizes.WindowRounding = style.WindowRounding;
			sourceSizes.WindowMinSize = style.WindowMinSize;
			sourceSizes.ChildRounding = style.ChildRounding;
			sourceSizes.PopupRounding = style.PopupRounding;
			sourceSizes.FramePadding = style.FramePadding;
			sourceSizes.FrameRounding = style.FrameRounding;
			sourceSizes.ItemSpacing = style.ItemSpacing;
			sourceSizes.ItemInnerSpacing = style.ItemInnerSpacing;
			sourceSizes.CellPadding = style.CellPadding;
			sourceSizes.TouchExtraPadding = style.TouchExtraPadding;
			sourceSizes.IndentSpacing = style.IndentSpacing;
			sourceSizes.ColumnsMinSpacing = style.ColumnsMinSpacing;
			sourceSizes.ScrollbarSize = style.ScrollbarSize;
			sourceSizes.ScrollbarRounding = style.ScrollbarRounding;
			sourceSizes.GrabMinSize = style.GrabMinSize;
			sourceSizes.GrabRounding = style.GrabRounding;
			sourceSizes.LogSliderDeadzone = style.LogSliderDeadzone;
			sourceSizes.TabRounding = style.TabRounding;
			sourceSizes.TabMinWidthForCloseButton = style.TabMinWidthForCloseButton;
			sourceSizes.DisplayWindowPadding = style.DisplayWindowPadding;
			sourceSizes.DisplaySafeAreaPadding = style.DisplaySafeAreaPadding;
			sourceSizes.MouseCursorScale = style.MouseCursorScale;
		}
	}

	public static void NewFrame()
	{
		EngineWindow main_window = WindowManager.MainWindow;
		if (main_window == null)
		{
			Engine.Quit();
			return;
		}

		var io = ImGui.GetIO();

		io.DisplaySize = new Vector2(main_window.ClientRenderSize.x, main_window.ClientRenderSize.y);
		io.DeltaTime = Engine.IFps;

		if (Input.MouseGrab == false)
		{
			ControlsApp.Enabled = !io.WantCaptureKeyboard;
			if (io.WantCaptureKeyboard)
			{
				ControlsApp.MouseDX = 0;
				ControlsApp.MouseDY = 0;
			}

			if (io.WantSetMousePos)
			{
				Input.MousePosition = new ivec2((int)io.MousePos.X, (int)io.MousePos.Y);
			}

			io.MousePos = new Vector2(Input.MousePosition.x - main_window.ClientPosition.x, Input.MousePosition.y - main_window.ClientPosition.y);
			io.MouseWheel += Input.MouseWheel;
			io.MouseWheelH += Input.MouseWheelHorizontal;

			Input.MouseHandle = io.WantCaptureMouse ? Input.MOUSE_HANDLE.USER : prev_mouse_handle;
		}

		float scale = main_window.DpiScale;
		if (MathLib.Equals(lastScale, scale) == false)
		{
			var style = ImGui.GetStyle();
			lastScale = scale;

			style.WindowPadding = sourceSizes.WindowPadding * scale;
			style.WindowRounding = sourceSizes.WindowRounding * scale;
			style.WindowMinSize = sourceSizes.WindowMinSize * scale;
			style.ChildRounding = sourceSizes.ChildRounding * scale;
			style.PopupRounding = sourceSizes.PopupRounding * scale;
			style.FramePadding = sourceSizes.FramePadding * scale;
			style.FrameRounding = sourceSizes.FrameRounding * scale;
			style.ItemSpacing = sourceSizes.ItemSpacing * scale;
			style.ItemInnerSpacing = sourceSizes.ItemInnerSpacing * scale;
			style.CellPadding = sourceSizes.CellPadding * scale;
			style.TouchExtraPadding = sourceSizes.TouchExtraPadding * scale;
			style.IndentSpacing = sourceSizes.IndentSpacing * scale;
			style.ColumnsMinSpacing = sourceSizes.ColumnsMinSpacing * scale;
			style.ScrollbarSize = sourceSizes.ScrollbarSize * scale;
			style.ScrollbarRounding = sourceSizes.ScrollbarRounding * scale;
			style.GrabMinSize = sourceSizes.GrabMinSize * scale;
			style.GrabRounding = sourceSizes.GrabRounding * scale;
			style.LogSliderDeadzone = sourceSizes.LogSliderDeadzone * scale;
			style.TabRounding = sourceSizes.TabRounding * scale;
			style.TabMinWidthForCloseButton = sourceSizes.TabMinWidthForCloseButton * scale;
			style.DisplayWindowPadding = sourceSizes.DisplayWindowPadding * scale;
			style.DisplaySafeAreaPadding = sourceSizes.DisplaySafeAreaPadding * scale;
			style.MouseCursorScale = sourceSizes.MouseCursorScale * scale;

			io.FontGlobalScale = scale;
		}

		ImGui.NewFrame();
	}

	public static void Render()
	{
		ImGui.Render();
		frame_draw_data = ImGui.GetDrawData();
	}

	public static void Shutdown()
	{
		imgui_material.DeleteLater();

		connections.DisconnectAll();

		ImGui.DestroyContext();
	}

	static void on_key_event(Input.KEY key, bool down)
	{
		var io = ImGui.GetIO();
        io.AddKeyEvent(keymap[(int)key], down);

        switch (keymap[(int)key])
        {
            case ImGuiKey.LeftCtrl:
            case ImGuiKey.RightCtrl: io.AddKeyEvent(ImGuiKey.ModCtrl, down); break;
            case ImGuiKey.LeftShift:
            case ImGuiKey.RightShift: io.AddKeyEvent(ImGuiKey.ModShift, down); break;
            case ImGuiKey.LeftAlt:
            case ImGuiKey.RightAlt: io.AddKeyEvent(ImGuiKey.ModAlt, down); break;
            case ImGuiKey.LeftSuper:
            case ImGuiKey.RightSuper: io.AddKeyEvent(ImGuiKey.ModSuper, down); break;
            default: break;
        }
    }

	static void on_key_pressed(Input.KEY key)
	{
		on_key_event(key, true);

    }

	static void on_key_released(Input.KEY key)
	{
        on_key_event(key, false);
    }

	static void on_button_event(Input.MOUSE_BUTTON button, bool down)
	{
        var io = ImGui.GetIO();

        switch (button)
        {
            case Input.MOUSE_BUTTON.LEFT: io.AddMouseButtonEvent((int)ImGuiMouseButton.Left, down); break;
            case Input.MOUSE_BUTTON.RIGHT: io.AddMouseButtonEvent((int)ImGuiMouseButton.Right, down); break;
            case Input.MOUSE_BUTTON.MIDDLE: io.AddMouseButtonEvent((int)ImGuiMouseButton.Middle, down); break;
            default: break;
        }
    }

	static void on_button_pressed(Input.MOUSE_BUTTON button)
	{
		on_button_event(button, true);

    }

	static void on_button_released(Input.MOUSE_BUTTON button)
	{
        on_button_event(button, false);
    }

	static void on_unicode_key_pressed(uint key)
	{
		var io = ImGui.GetIO();
		io.AddInputCharacter(key);
	}

	static unsafe void create_font_texture()
	{
		var io = ImGui.GetIO();

		io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out int width, out int height, out int bytesPerPixel);
		var pixels = new byte[width * height * bytesPerPixel];
		Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length);

		font_texture = new Texture();
		font_texture.Create2D(width, height, Texture.FORMAT_RGBA8, Texture.SAMPLER_FILTER_LINEAR);

		var blob = new Blob();
		blob.SetData(pixels, Convert.ToUInt64(width) * Convert.ToUInt64(height) * 32);
		font_texture.SetBlob(blob);
		blob.SetData(null, 0);

		io.Fonts.TexID = font_texture.GetPtr();
	}

	static unsafe void create_imgui_mesh()
	{
		imgui_mesh = new MeshDynamic(MeshDynamic.USAGE_DYNAMIC_ALL);

		MeshDynamic.Attribute[] attributes = new MeshDynamic.Attribute[3];
		attributes[0].offset = 0;
		attributes[0].size = 2;
		attributes[0].type = MeshDynamic.TYPE_FLOAT;
		attributes[1].offset = 8;
		attributes[1].size = 2;
		attributes[1].type = MeshDynamic.TYPE_FLOAT;
		attributes[2].offset = 16;
		attributes[2].size = 4;
		attributes[2].type = MeshDynamic.TYPE_UCHAR;
		imgui_mesh.SetVertexFormat(attributes);

		Debug.Assert(imgui_mesh.GetVertexSize() == sizeof(ImDrawVert), "Vertex size of MeshDynamic is not equal to size of ImDrawVert");
	}

	static void create_imgui_material()
	{
		imgui_material = Materials.FindManualMaterial("imgui").Inherit();
		//imgui_material.SetTexture("imgui_texture", font_texture);
	}

	static void before_render_callback()
	{
		var io = ImGui.GetIO();
		if (io.WantCaptureMouse)
		{
			Gui.GetCurrent().MouseButtons = 0;
		}
	}

	static unsafe void draw_callback()
	{
		if (frame_draw_data.Equals(null))
			return;

		var draw_data = frame_draw_data;
		if (draw_data.DisplaySize.X <= 0.0f || draw_data.DisplaySize.Y <= 0.0f)
			return;

		var io = ImGui.GetIO();

		var render_target = Unigine.Render.GetTemporaryRenderTarget();
		render_target.BindColorTexture(0, Renderer.TextureColor);

		// Render state
		RenderState.SaveState();
		RenderState.ClearStates();
		RenderState.SetBlendFunc(RenderState.BLEND_SRC_ALPHA, RenderState.BLEND_ONE_MINUS_SRC_ALPHA, RenderState.BLEND_OP_ADD);
		RenderState.PolygonCull = RenderState.CULL_NONE;
		RenderState.DepthFunc = RenderState.DEPTH_NONE;
		RenderState.SetViewport((int)draw_data.DisplayPos.X, (int)draw_data.DisplayPos.Y, (int)draw_data.DisplaySize.X, (int)draw_data.DisplaySize.Y);

		// Orthographic projection matrix
		float left = draw_data.DisplayPos.X;
		float right = draw_data.DisplayPos.X + draw_data.DisplaySize.X;
		float top = draw_data.DisplayPos.Y;
		float bottom = draw_data.DisplayPos.Y + draw_data.DisplaySize.Y;

		mat4 proj = new mat4();
		proj.m00 = 2.0f / (right - left);
		proj.m03 = (right + left) / (left - right);
		proj.m11 = 2.0f / (top - bottom);
		proj.m13 = (top + bottom) / (bottom - top);
		proj.m22 = 0.5f;
		proj.m23 = 0.5f;
		proj.m33 = 1.0f;

		Renderer.Projection = proj;
		var shader = imgui_material.GetShaderForce("imgui");
		var pass = imgui_material.GetRenderPass("imgui");
		Renderer.SetShaderParameters(pass, shader, imgui_material, false);

		imgui_mesh.Bind();

		// Write vertex and index data into dynamic mesh
		imgui_mesh.ClearVertex();
		imgui_mesh.ClearIndices();
		imgui_mesh.AllocateVertex(draw_data.TotalVtxCount);
		imgui_mesh.AllocateIndices(draw_data.TotalIdxCount);
		for (int i = 0; i < draw_data.CmdListsCount; ++i)
		{
			ImDrawListPtr cmd_list = draw_data.CmdLists[i];

			imgui_mesh.AddVertexArray(cmd_list.VtxBuffer.Data, cmd_list.VtxBuffer.Size);

			// TODO:
			// use the imgui_mesh.AddIndicesArray(cmd_list.IdxBuffer.Data, cmd_list.IdxBuffer.Size);
			// instead of it:
			for (int j = 0; j < cmd_list.IdxBuffer.Size; j++)
				imgui_mesh.AddIndex(cmd_list.IdxBuffer[j]);
			// We need to use it that way now because IdxBuffer uses 16-bit format indices, but MeshDynamic uses 32-bit
		}
		imgui_mesh.FlushVertex();
		imgui_mesh.FlushIndices();

		render_target.Enable();
		{
			int global_idx_offset = 0;
			int global_vtx_offset = 0;
			ivec2 clip_off = new ivec2((int)draw_data.DisplayPos.X, (int)draw_data.DisplayPos.Y);

			// Draw command lists
			for (int i = 0; i < draw_data.CmdListsCount; ++i)
			{
				ImDrawListPtr cmd_list = draw_data.CmdLists[i];
				for (int j = 0; j < cmd_list.CmdBuffer.Size; ++j)
				{
					ImDrawCmdPtr cmd = cmd_list.CmdBuffer[j];

					if (cmd.UserCallback == IntPtr.Zero)
					{
						float width = (cmd.ClipRect.Z - cmd.ClipRect.X) / draw_data.DisplaySize.X;
						float height = (cmd.ClipRect.W - cmd.ClipRect.Y) / draw_data.DisplaySize.Y;
						float x = (cmd.ClipRect.X - clip_off.x) / draw_data.DisplaySize.X;
						float y = 1.0f - height - (cmd.ClipRect.Y - clip_off.y) / draw_data.DisplaySize.Y;

						if (cmd.TextureId == IntPtr.Zero || cmd.TextureId == io.Fonts.TexID)
						{
							RenderState.SetTexture(RenderState.BIND_FRAGMENT, 0, font_texture);
						}
						else
						{
							Texture texture = Texture.UnsafeCreate(cmd.TextureId);
							RenderState.SetTexture(RenderState.BIND_FRAGMENT, 0, texture);
						}

						RenderState.SetScissorTest(x, y, width, height);
						RenderState.FlushStates();

						imgui_mesh.RenderInstancedSurface(MeshDynamic.MODE_TRIANGLES,
							(int)(cmd.VtxOffset + global_vtx_offset),
							(int)(cmd.IdxOffset + global_idx_offset),
							(int)(cmd.IdxOffset + global_idx_offset + cmd.ElemCount), 1);
					}
				}
				global_vtx_offset += cmd_list.VtxBuffer.Size;
				global_idx_offset += cmd_list.IdxBuffer.Size;
			}

			RenderState.SetScissorTest(0.0f, 0.0f, 1.0f, 1.0f);
		}
		render_target.Disable();
		imgui_mesh.Unbind();

		RenderState.RestoreState();

		render_target.UnbindColorTexture(0);
		Unigine.Render.ReleaseTemporaryRenderTarget(render_target);
	}
}
using Hexa.NET.ImGui;
using Avalonia.Data;

# if WINDOWS
using System.Runtime.InteropServices;
# endif

namespace ETS2LA.Overlay
{
    public struct WindowDefinition
    {
        public string Title;
        public Optional<ImGuiWindowFlags> Flags;
        public Optional<int> Width;
        public Optional<int> Height;
        public Optional<Func<(int, int)>> SizingFunction;
        public Optional<int> X;
        public Optional<int> Y;
        public Optional<Func<(int, int)>> LocationFunction;
        public Optional<float> Alpha;
        public Optional<bool> Open;
        /// <summary>
        ///  This might be useful if you want a reliable callback to when
        ///  the overlay is rendered. Setting this variable to true will mean
        ///  the OverlayHandler will call the render outside of the ImGui system.
        ///  WARNING: If you want a window, you have to create it yourself!
        /// </summary>
        public Optional<bool> NoWindow;
    }

    public static class GameWindowManager
    {
        public struct WindowRect { public int X, Y, Width, Height; }

        #if WINDOWS
            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct POINT
            {
                public int X;
                public int Y;
            }

            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

            [DllImport("user32.dll")]
            static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

            [DllImport("user32.dll")]
            static extern bool IsIconic(IntPtr hWnd);

            private static readonly string[] GameWindowTitles =
            {
                "Euro Truck Simulator 2",
                "American Truck Simulator"
            };

            public static IntPtr FindGameWindow()
            {
                foreach (var title in GameWindowTitles)
                {
                    IntPtr hWnd = FindWindow(null, title);
                    if (hWnd != IntPtr.Zero)
                        return hWnd;
                }
                return IntPtr.Zero;
            }

            /// <summary>
            /// 游戏窗口被最小化（常见于 Alt+Tab 切出）时，覆盖层 OpenGL 呈现容易卡住。
            /// </summary>
            public static bool IsGameWindowMinimized()
            {
                IntPtr hWnd = FindGameWindow();
                return hWnd != IntPtr.Zero && IsIconic(hWnd);
            }

            public static WindowRect GetGameWindowRect()
            {
                IntPtr hWnd = FindGameWindow();
                if (hWnd == IntPtr.Zero)
                    return new WindowRect { X = 0, Y = 0, Width = 1, Height = 1 };

                GetClientRect(hWnd, out RECT rect);
                POINT topLeft = new() { X = rect.Left, Y = rect.Top };
                ClientToScreen(hWnd, ref topLeft);

                return new WindowRect
                {
                    X = topLeft.X,
                    Y = topLeft.Y,
                    Width = rect.Right - rect.Left,
                    Height = rect.Bottom - rect.Top
                };
            }
        #else
            // No reliable way to get the game rect on Linux/Mac
            // Well with a reasonable amount of security that is...
            // TODO: Fix this?
            public static WindowRect GetGameWindowRect()
            {
                float width = OverlayHandler.Current.OverlayWidth;
                float height = OverlayHandler.Current.OverlayHeight;
                return new WindowRect { X = 0, Y = 0, Width = (int)width, Height = (int)height };
            }
        #endif
    }
}
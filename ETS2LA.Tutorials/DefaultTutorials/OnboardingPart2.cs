using Hexa.NET.ImGui;
using ETS2LA.Overlay;
using ETS2LA.Controls;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Numerics;
using ETS2LA.Backend.Events;

namespace ETS2LA.Tutorials.DefaultTutorials;

public class OnboardingPart2
{
    public Tutorial Create()
    {
        return new Tutorial("OnboardingPart2", "从插件安装完成到启用插件的新手引导。", "ETS2LA", new List<TutorialSection>
        {
            new TutorialSection
            {
                Title = "用户界面介绍",
                Actions = new List<TutorialAction>
                {
                    new ShowMessageAction
                    {
                        Message = "很好！\n接下来进入设置页面。",
                        ScreenPositionCallback = () =>
                        {
                            var position = ETS2LAWindowLocation();
                            var size = ETS2LAWindowSize();
                            return (position.Item1 + 220, position.Item2 + size.Item2 - 70);
                        }
                    },
                    new WaitForEventAction
                    {
                        EventId = "ETS2LA.UI.SwitchedPage.Settings"
                    },
                    new ShowMessageAction
                    {
                        Message = "让我们查看控制设置。",
                        ScreenPositionCallback = () =>
                        {
                            var position = ETS2LAWindowLocation();
                            var size = ETS2LAWindowSize();
                            return (position.Item1 + 220, position.Item2 + 320);
                        }
                    },
                    new WaitForEventAction
                    {
                        EventId = "ETS2LA.UI.SwitchedPage.Settings.Controls"
                    },
                    new ExecuteFunctionAction
                    {
                        Function = () =>
                        {
                            OverlayHandler.Current.OpenWindow("State Info");
                        }
                    },
                    new ShowMessageAction
                    {
                        Message = "这里可以看到 ETS2LA 的所有控制项。我们已打开「状态信息」窗口，方便查看当前设置。\n试试按下 SET 键切换 ACC。",
                        ScreenPositionCallback = () =>
                        {
                            var position = ETS2LAWindowLocation();
                            var size = ETS2LAWindowSize();
                            return (position.Item1 + 442, position.Item2 + 3);
                        }
                    },
                    new WaitForEventAction
                    {
                        EventId="ETS2LA.State.AssistsPaused"
                    },
                    new ShowMessageAction
                    {
                        Message = "这里可以看到 ETS2LA 的所有控制项。我们已打开「状态信息」窗口，方便查看当前设置。\n你也可以使用 ASSIST 键切换转向模式。",
                        ScreenPositionCallback = () =>
                        {
                            var position = ETS2LAWindowLocation();
                            var size = ETS2LAWindowSize();
                            return (position.Item1 + 442, position.Item2 + 3);
                        }
                    },
                    new WaitForEventAction
                    {
                        EventId="ETS2LA.State.SteeringLevel.None"
                    },
                    new ShowMessageAction
                    {
                        Message = "很好！\n接下来进入插件管理页面。",
                        ScreenPositionCallback = () =>
                        {
                            var position = ETS2LAWindowLocation();
                            var size = ETS2LAWindowSize();
                            return (position.Item1 + 15, position.Item2 + 202);
                        }
                    },
                    new WaitForEventAction
                    {
                        EventId = "ETS2LA.UI.SwitchedPage.Manager"
                    },
                    new ShowMessageAction
                    {
                        Message = "这里显示所有已安装的插件。请先启用「车道辅助」和「自适应巡航控制」。",
                        ScreenPositionCallback = () =>
                        {
                            var position = ETS2LAWindowLocation();
                            var size = ETS2LAWindowSize();
                            return (position.Item1 + 230, position.Item2 + 1);
                        }
                    },
                    new WaitForEventAction
                    {
                        EventId = "ETS2LA.Backend.Enabled.tumppi066.adaptivecruisecontrol"
                    },
                    new ShowMessageWaitNextAction
                    {
                        Message = "新手引导到此结束。如有疑问，请查看 YouTube 频道或 Discord 获取更多信息。\n所有链接可在仪表盘页面找到。",
                        ScreenPositionCallback = () =>
                        {
                            var position = ETS2LAWindowLocation();
                            var size = ETS2LAWindowSize();
                            return (position.Item1 + 230, position.Item2 + 3);
                        }
                    },
                    new SendNotificationAction
                    {
                        Title = "教程完成",
                        Message = "欢迎使用 ETS2LA！",
                        Level = Notifications.NotificationLevel.Success,
                        CloseAfter = 5f
                    },
                }
            }
        });
    }

    private (int, int) ETS2LAWindowLocation()
    {
        if (Application.Current == null || Application.Current.ApplicationLifetime == null)
            return (0, 0);

        var window = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;
        if (window == null)
            return (0, 0);
        
        return (window.Position.X, window.Position.Y);
    }

    private (int, int) ETS2LAWindowSize()
    {
        if (Application.Current == null || Application.Current.ApplicationLifetime == null)
            return (0, 0);

        var window = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;
        if (window == null || window.FrameSize == null)
            return (0, 0);
        
        var size = ((int)window.FrameSize.Value.Width, (int)window.FrameSize.Value.Height);
        return size;
    }
}

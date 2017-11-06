using Windows.System.Profile;
using Windows.UI.ViewManagement;

namespace RasPiHomeDock.Utils {
    public static class DeviceTypeHelper {
        public static DeviceType GetDeviceType() {
            switch ( AnalyticsInfo.VersionInfo.DeviceFamily ) {
                case "Windows.Mobile":
                    return DeviceType.Phone;
                case "Windows.Desktop":
                    return UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Mouse
                               ? DeviceType.Desktop
                               : DeviceType.Tablet;
                case "Windows.IoT":
                    return DeviceType.IoT;
                case "Windows.Team":
                    return DeviceType.SurfaceHub;
                default:
                    return DeviceType.Other;
            }
        }
    }


    public enum DeviceType {
        Phone,
        Desktop,
        Tablet,
        IoT,
        SurfaceHub,
        Other
    }
}
// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace NumberTracker.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

		#endregion

		public static readonly string CAMERA = "Camera";
		public static readonly string GALLERY = "Gallery";
		public static readonly string DefaultCameraActionKey = "default_camera_action_key";
		public static readonly string DefaultCameraSelectionKey = "default_camera_selection_key";


		public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }

		public static string DefaultCameraSelection
		{
			get { return AppSettings.GetValueOrDefault<string>(DefaultCameraSelectionKey, CAMERA); }
			set { AppSettings.AddOrUpdateValue<string>(DefaultCameraSelectionKey, value); }
		}

		public static bool EnableDefaultAction
		{
			get { return AppSettings.GetValueOrDefault<bool>(DefaultCameraActionKey, false); }
			set { AppSettings.AddOrUpdateValue<bool>(DefaultCameraActionKey, value); }
		}

  }
}
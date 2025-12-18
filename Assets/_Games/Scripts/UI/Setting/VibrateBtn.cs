namespace PuzzleGames
{
    using HapticController = HapticController;

    public class VibrateBtn : SettingButton
    {
        protected override bool ChangeSetting()
        {
            var enableHaptic = GetCurrentSetting();
            HapticController.instance.SetState(enableHaptic ? 0 : 1);

            if (!enableHaptic)
            {
                HapticController.instance.Play();
            }
            
            return !enableHaptic;
        }
        protected override bool GetCurrentSetting() { return HapticController.instance.IsActive(); }
    }
}
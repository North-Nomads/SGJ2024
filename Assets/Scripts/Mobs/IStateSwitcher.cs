namespace SGJ.Mobs
{
    internal interface IStateSwitcher
    {
        public void SwitchState<T>() where T : MobState;
    }
}